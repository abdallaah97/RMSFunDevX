using Application.Repositories;
using Application.Servces.UserService.DTOs;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Servces.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUser(CreateUserDto input)
        {
            var user = new User
            {
                Name = input.Name,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Password = input.Password,
                RoleId = input.RoleId,
                CreatedAt = DateTime.UtcNow,
            };


            var isEmailExist = await _userRepository.GetAll().AnyAsync(u => u.Email == input.Email);
            if (isEmailExist)
            {
                throw new Exception("Email already exists.");
            }

            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, input.Password);

            await _userRepository.InsertAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUser(UpdateUserDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.Id);

            var isEmailExist = await _userRepository.GetAll().AnyAsync(u => u.Email == input.Email && u.Id != input.Id);
            if (isEmailExist)
            {
                throw new Exception("Email already exists.");
            }

            user.Name = input.Name;
            user.Email = input.Email;
            user.PhoneNumber = input.PhoneNumber;


            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<GetUserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetAll().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

            var userDto = new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.Code
            };

            return userDto;
        }


        public async Task<List<GetUserDto>> GetAllUsers(string? name, SystemRole? role)
        {
            var users = _userRepository.GetAll();

            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            if (role != null)
            {
                users = users.Where(u => u.Role.Code == role);
            }

            users = users.Include(u => u.Role);


            List<GetUserDto> userDto = users.Select(user => new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.Code
            }).ToList();

            return userDto;
        }
    }

}
