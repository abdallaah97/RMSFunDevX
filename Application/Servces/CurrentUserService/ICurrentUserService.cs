namespace Application.Servces.CurrentUserService
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Name { get; }
        string? Email { get; }
        string? PhoneNumber { get; }
        string? Role { get; }
    }
}
