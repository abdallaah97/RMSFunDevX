using Domain.Entites;

namespace Application.Servces.RequestService.DTOs
{
    public class SaveRequestInputDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int CategoryId { get; set; }
        public RequestPriority Priority { get; set; }
    }
}
