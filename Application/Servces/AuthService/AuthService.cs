using Application.Repositories;
using Application.Servces.AuthService.DTOs;
using Application.Servces.CurrentUserService;
using Application.Servces.UserService.DTOs;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Servces.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        public AuthService(IGenericRepository<User> userRepository, IGenericRepository<RefreshToken> refreshTokenRepository, IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginInputDto input)
        {
            var user = await _userRepository.GetAll()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == input.Username.ToLower().Trim());

            if (user == null)
            {
                throw new Exception("Invalid user name or password");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid user name or password");
            }

            return new LoginResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.Code,
                Token = GenerateToken(user),
                RefershToken = await GenerateRefershToken(user),
                TokenExpiration = DateTime.UtcNow.AddMinutes(15)
            };
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("phoneNumber", user.PhoneNumber),
                new Claim("role", user.Role.Code.ToString())
            };

            var token = new JwtSecurityToken
            (
                claims: claims,
                issuer: _configuration["jwt:issuer"],
                audience: _configuration["jwt:audience"],
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetAll()
                .Include(x => x.User)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (existingToken == null || existingToken.Expiration < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token");
            }

            var newAccessToken = GenerateToken(existingToken.User);

            return newAccessToken;

        }

        public async Task LogoutAsync()
        {
            var userId = _currentUserService.UserId;

            var existingTokens = await _refreshTokenRepository.GetAll()
                .Where(rt => rt.UserId == userId).ToListAsync();

            if (existingTokens != null)
            {
                _refreshTokenRepository.DeleteRange(existingTokens);
                await _refreshTokenRepository.SaveChangesAsync();
            }
        }


        private async Task<string> GenerateRefershToken(User user)
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);

            var refershToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Convert.ToBase64String(random),
                Expiration = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.InsertAsync(refershToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return Convert.ToBase64String(random);
        }


        public async Task ChangePasswordAsync(ChangePasswordInputDto input)
        {
            var userId = _currentUserService.UserId;

            var user = await _userRepository.GetByIdAsync(userId.Value);

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, input.OldPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Old password is incorrect");
            }
            if (input.NewPassword != input.ConfirmNewPassword)
            {
                throw new Exception("New password and confirm new password do not match");
            }
            user.Password = passwordHasher.HashPassword(user, input.NewPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();   
        }

        public async Task UpdateCurrentUserProfile(UpdateUserDto input)
        {
            var userId = _currentUserService.UserId;

            var user = await _userRepository.GetByIdAsync(userId.Value);

            user.Email = input.Email;
            user.Name = input.Name;
            user.PhoneNumber = input.PhoneNumber;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
