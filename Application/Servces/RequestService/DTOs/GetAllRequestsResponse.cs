using Domain.Entites;

namespace Application.Servces.RequestService.DTOs
{
    public class GetAllRequestsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public RequestStatus Status { get; set; }
        public string StatusName
        {
            get
            {
                return Status.ToString();
            }

        }
        public string CategoryName { get; set; }
        public string EmployeeName { get; set; }
        public string? TechnicianName { get; set; }
        public RequestPriority Priority { get; set; }
        public string ProiorityName
        {
            get
            {
                return Priority.ToString();
            }
        }
        public DateTime CreatedAt { get; set; }
    }
}
