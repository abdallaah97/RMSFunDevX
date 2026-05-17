using Application.Repositories;
using Application.Servces.UserService.DTOs;
using Domain.Entites;
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
                RoleId = input.RoleId
            };


            var isEmailExist = await _userRepository.GetAll().AnyAsync(u => u.Email == input.Email);
            if (isEmailExist)
            {
                throw new Exception("Email already exists.");
            }

            await _userRepository.InsertAsync(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
