using Application.Servces.RequestService;
using Application.Servces.RequestService.DTOs;
using Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize(Roles = "TechnicianAdmin,Employee")]
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _requestService.GetAllCategories();
            return Ok(categories);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] SaveRequestInputDto input)
        {
            await _requestService.CreateRequest(input);
            return Ok();
        }

        [Authorize(Roles = "Employee")]
        [HttpPut("UpdateRequest")]
        public async Task<IActionResult> UpdateRequest([FromBody] SaveRequestInputDto input)
        {
            await _requestService.UpdateRequest(input);
            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPost("CancelRequest")]
        public async Task<IActionResult> CancelRequest(int requestId, [FromBody] string note)
        {
            await _requestService.CancelRequest(requestId, note);
            return Ok();
        }


        [Authorize(Roles = "TechnicianAdmin")]
        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests(string? title, int? categoryId, RequestStatus? status, RequestPriority? priority)
        {
            var requests = await _requestService.GetAllRequests(title, categoryId, status, priority);
            return Ok(requests);
        }


        [Authorize(Roles = "TechnicianAdmin")]
        [HttpPost("AssignTechnicianToRequest")]
        public async Task<IActionResult> AssignTechnicianToRequest(int requestId, int technicianId, [FromBody] string note)
        {
            await _requestService.AssignTechnicianToRequest(requestId, technicianId, note);
            return Ok();
        }


        [Authorize(Roles = "Technician")]
        [HttpPost("InProgressRequest")]
        public async Task<IActionResult> InProgressRequest(int requestId, [FromBody] string note)
        {
            await _requestService.InProgressRequest(requestId, note);
            return Ok();
        }

        [Authorize(Roles = "Technician")]
        [HttpPost("ResolvedRequest")]
        public async Task<IActionResult> ResolvedRequest(int requestId, [FromBody] string note)
        {
            await _requestService.ResolvedRequest(requestId, note);
            return Ok();
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("CloseRequest")]
        public async Task<IActionResult> CloseRequest(int requestId, [FromBody] string note)
        {
            await _requestService.CloseRequest(requestId, note);
            return Ok();
        }

        [Authorize(Roles = "Employee,Technician")]
        [HttpGet("GetAllRequestsForCurrentUser")]
        public async Task<IActionResult> GetAllRequestsForCurrentUser(string? title, int? categoryId, RequestStatus? status, RequestPriority? priority)
        {
            var requests = await _requestService.GetAllRequestsForCurrentUser(title, categoryId, status, priority);
            return Ok(requests);
        }
    }
}
