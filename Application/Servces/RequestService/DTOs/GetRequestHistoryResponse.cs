using Domain.Entites;

namespace Application.Servces.RequestService.DTOs
{
    public class GetRequestHistoryResponse
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string UserName { get; set; }
        public RequestStatus OldStatus { get; set; }
        public RequestStatus NewStatus { get; set; }
        public string OldStatusName { get { return OldStatus.ToString(); } }
        public string NewStatusName { get { return NewStatus.ToString(); } }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
