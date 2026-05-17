namespace Domain.Entites
{
    public class RequestHistory : BaseEntity
    {
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public RequestStatus OldStatus { get; set; }
        public RequestStatus NewStatus { get; set; }
        public string? Note { get; set; }
    }
}
