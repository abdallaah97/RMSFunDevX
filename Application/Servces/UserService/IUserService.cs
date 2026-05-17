using Application.Servces.UserService.DTOs;

namespace Application.Servces.UserService
{
    public interface IUserService
    {
        Task CreateUser(CreateUserDto input);
    }
}
