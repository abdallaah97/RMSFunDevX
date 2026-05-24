using Application.Servces.TechnicianUserService.DTOs;

namespace Application.Servces.TechnicianUserService
{
    public interface ITechnicianUserService
    {
        Task<List<GetAllTechnicianUsersResponse>> GetTechnicianUsers();
    }
}
