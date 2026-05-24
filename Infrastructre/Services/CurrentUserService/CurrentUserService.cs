using Application.Servces.CurrentUserService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;
                return userId != null ? int.Parse(userId) : (int?)null;
            }
        }

        public string? Name => _httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value;

        public string? Email => _httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value;

        public string? PhoneNumber => _httpContextAccessor.HttpContext?.User.FindFirst("phoneNumber")?.Value;

        public string? Role => _httpContextAccessor.HttpContext?.User.FindFirst("role")?.Value;
    }
}
