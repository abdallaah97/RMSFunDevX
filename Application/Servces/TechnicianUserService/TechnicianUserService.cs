using Application.Repositories;
using Application.Servces.TechnicianUserService.DTOs;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Application.Servces.TechnicianUserService
{
    public class TechnicianUserService : ITechnicianUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        public TechnicianUserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<GetAllTechnicianUsersResponse>> GetTechnicianUsers()
        {
            var technicians = await _userRepository.GetAll()
                .Where(u => u.Role.Code == SystemRole.Technician).ToListAsync();
            return technicians.Select(t => new GetAllTechnicianUsersResponse
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
        }
    }
}
