using Application.Servces.TechnicianUserService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianUserController : ControllerBase
    {
        private readonly ITechnicianUserService _technicianUserService;
        public TechnicianUserController(ITechnicianUserService technicianUserService)
        {
            _technicianUserService = technicianUserService;
        }

        [HttpGet("GetTechnicianUsers")]
        public async Task<IActionResult> GetTechnicianUsers()
        {
            var technicians = await _technicianUserService.GetTechnicianUsers();
            return Ok(technicians);
        }
    }
}
