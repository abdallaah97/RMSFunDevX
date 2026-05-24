using Application.Servces.RequestService.DTOs;
using Domain.Entites;

namespace Application.Servces.RequestService
{
    public interface IRequestService
    {
        Task<List<GetAllCategoriesResponse>> GetAllCategories();
        Task CreateRequest(SaveRequestInputDto input);
        Task UpdateRequest(SaveRequestInputDto input);
        Task CancelRequest(int requestId, string note);
        Task<List<GetAllRequestsResponse>> GetAllRequests(string? title, int? categoryId, RequestStatus? status, RequestPriority? priority);
        Task<List<GetAllRequestsResponse>> GetAllRequestsForCurrentUser(string? title, int? categoryId, RequestStatus? status, RequestPriority? priority);
        Task AssignTechnicianToRequest(int requestId, int technicianId, string note);
        Task InProgressRequest(int requestId, string note);
        Task ResolvedRequest(int requestId, string note);
        Task<List<GetRequestHistoryResponse>> GetRequestHistory(int requestId);
        Task CloseRequest(int requestId, string note);
    }
}
