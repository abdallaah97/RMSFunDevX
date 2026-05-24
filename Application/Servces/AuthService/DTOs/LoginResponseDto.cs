using Application.Servces.UserService.DTOs;

namespace Application.Servces.AuthService.DTOs
{
    public class LoginResponseDto : GetUserDto
    {
        public string Token { get; set; }
        public string RefershToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
