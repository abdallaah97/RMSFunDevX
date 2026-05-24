using Domain.Entites;

namespace Application.Servces.UserService.DTOs
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public SystemRole Role { get; set; }
    }
}
