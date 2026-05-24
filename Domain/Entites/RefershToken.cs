namespace Domain.Entites
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Expiration { get; set; }
    }
}
