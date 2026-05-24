using Application.Servces.UserService.DTOs;
using Domain.Entites;

namespace Application.Servces.UserService
{
    public interface IUserService
    {
        Task CreateUser(CreateUserDto input);
        Task UpdateUser(UpdateUserDto input);
        Task<GetUserDto> GetUserById(int id);
        Task<List<GetUserDto>> GetAllUsers(string? name, SystemRole? role);
        Task ChangePasswordAsync(ChangeUserPasswordInputDto input);
    }
}
