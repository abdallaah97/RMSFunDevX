namespace Application.Servces.UserService.DTOs
{
    public class ChangeUserPasswordInputDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
