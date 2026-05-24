namespace Application.Servces.AuthService.DTOs
{
    public class ChangePasswordInputDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
