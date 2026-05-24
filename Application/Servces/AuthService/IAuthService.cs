using Application.Servces.AuthService.DTOs;
using Application.Servces.UserService.DTOs;

namespace Application.Servces.AuthService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginInputDto input);
        Task<string> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync();
        Task ChangePasswordAsync(ChangePasswordInputDto input);
        Task UpdateCurrentUserProfile(UpdateUserDto input);
    }
}
