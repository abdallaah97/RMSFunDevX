namespace Domain.Entites
{
    public class Request : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public RequestStatus Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int EmployeeId { get; set; }
        public User Employee { get; set; }
        public int? TechnicianId { get; set; }
        public User Technician { get; set; }
        public DateTime? TechnicianAssignDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public RequestPriority Priority { get; set; }
        public string? EmployeeNote { get; set; }
        public string? TechnicianNote { get; set; } 
        public string? PhotoUrl { get; set; }
        public ICollection<RequestHistory> RequestHistories { get; set; }
    }

    public enum RequestStatus
    {
        Pending = 1,
        Assigned = 2,
        InProgress = 3,
        Resolved = 4,
        Closed = 5,
        Cancelled = 6,
    }

    public enum RequestPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
