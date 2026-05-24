using Application.Repositories;
using Application.Servces.CurrentUserService;
using Application.Servces.RequestService.DTOs;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Application.Servces.RequestService
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Request> _requestRepository;
        private readonly IGenericRepository<RequestHistory> _requestHistoryRepository;
        private readonly ICurrentUserService _currentUserService;
        public RequestService(IGenericRepository<Category> categoryRepository, IGenericRepository<Request> requestRepository, IGenericRepository<RequestHistory> requestHistoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _requestRepository = requestRepository;
            _requestHistoryRepository = requestHistoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task CreateRequest(SaveRequestInputDto input)
        {
            var request = new Request
            {
                Title = input.Title,
                Description = input.Description,
                Priority = input.Priority,
                Location = input.Location,
                CategoryId = input.CategoryId,
                CreatedAt = DateTime.UtcNow,
                Status = RequestStatus.Pending,
                EmployeeId = _currentUserService.UserId.Value
            };

            await _requestRepository.InsertAsync(request);
            await _requestRepository.SaveChangesAsync();
        }


        public async Task UpdateRequest(SaveRequestInputDto input)
        {
            var request = await _requestRepository.GetByIdAsync(input.Id.Value);

            if (request.Status != RequestStatus.Pending)
            {
                throw new Exception("Cann't update this request");
            }

            request.Title = input.Title;
            request.Description = input.Description;
            request.Priority = input.Priority;
            request.Location = input.Location;
            request.CategoryId = input.CategoryId;

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task CancelRequest(int requestId, string note)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            var oldStatus = request.Status;

            request.Status = RequestStatus.Cancelled;

            await AddRecordHistory(request, oldStatus, note);

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task InProgressRequest(int requestId, string note)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request.TechnicianId != _currentUserService.UserId)
            {
                throw new Exception("Cann't change to inprogress");
            }

            var oldStatus = request.Status;

            request.Status = RequestStatus.InProgress;

            await AddRecordHistory(request, oldStatus, note);

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task ResolvedRequest(int requestId, string note)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request.TechnicianId != _currentUserService.UserId && request.Status != RequestStatus.InProgress)
            {
                throw new Exception("Cann't change to resolved");
            }

            var oldStatus = request.Status;

            request.Status = RequestStatus.Resolved;

            await AddRecordHistory(request, oldStatus, note);

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task CloseRequest(int requestId, string note)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request.EmployeeId != _currentUserService.UserId && request.Status != RequestStatus.Resolved)
            {
                throw new Exception("Cann't change to closed");
            }

            var oldStatus = request.Status;

            request.Status = RequestStatus.Closed;

            await AddRecordHistory(request, oldStatus, note);

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task<List<GetAllCategoriesResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();
            var result = new List<GetAllCategoriesResponse>();

            result = categories.Select(c => new GetAllCategoriesResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();

            return result;
        }

        public async Task<List<GetAllRequestsResponse>> GetAllRequests(string? title, int? categoryId, RequestStatus? status, RequestPriority? priority)
        {
            var requestsQuery = _requestRepository.GetAll()
                .Include(r => r.Category)
                .Include(r => r.Employee)
                .Include(r => r.Technician)
                .OrderByDescending(r => r.CreatedAt)
                .AsQueryable();


            if (!string.IsNullOrEmpty(title))
            {
                requestsQuery = requestsQuery.Where(r => r.Title.Trim().ToLower().Contains(title.Trim().ToLower()));
            }
            if (categoryId.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.CategoryId == categoryId.Value);
            }
            if (status.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.Status == status.Value);
            }
            if (priority.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.Priority == priority.Value);
            }

            var requests = await requestsQuery.ToListAsync();
            var result = requests.Select(r => new GetAllRequestsResponse
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Location = r.Location,
                Status = r.Status,
                CategoryName = r.Category.Name,
                EmployeeName = r.Employee.Name,
                TechnicianName = r.Technician != null ? r.Technician.Name : null,
                CreatedAt = r.CreatedAt,
                Priority = r.Priority
            }).ToList();
            return result;
        }

        public async Task<List<GetAllRequestsResponse>> GetAllRequestsForCurrentUser(string? title, int? categoryId, RequestStatus? status, RequestPriority? priority)
        {
            var requestsQuery = _requestRepository.GetAll()
                .Include(r => r.Category)
                .Include(r => r.Employee)
                .Include(r => r.Technician)
                .Where(r => r.EmployeeId == _currentUserService.UserId || r.TechnicianId == _currentUserService.UserId)
                .OrderByDescending(r => r.CreatedAt)
                .AsQueryable();


            if (!string.IsNullOrEmpty(title))
            {
                requestsQuery = requestsQuery.Where(r => r.Title.Trim().ToLower().Contains(title.Trim().ToLower()));
            }
            if (categoryId.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.CategoryId == categoryId.Value);
            }
            if (status.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.Status == status.Value);
            }
            if (priority.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.Priority == priority.Value);
            }

            var requests = await requestsQuery.ToListAsync();
            var result = requests.Select(r => new GetAllRequestsResponse
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Location = r.Location,
                Status = r.Status,
                CategoryName = r.Category.Name,
                EmployeeName = r.Employee.Name,
                TechnicianName = r.Technician != null ? r.Technician.Name : null,
                CreatedAt = r.CreatedAt,
                Priority = r.Priority
            }).ToList();
            return result;
        }

        public async Task AssignTechnicianToRequest(int requestId, int technicianId, string note)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request.Status != RequestStatus.Pending)
            {
                throw new Exception("Cann't change to assigned");
            }

            var oldStatus = request.Status;

            request.TechnicianId = technicianId;
            request.TechnicianAssignDate = DateTime.UtcNow;

            request.Status = RequestStatus.Assigned;

            await AddRecordHistory(request, oldStatus, note);

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        private async Task AddRecordHistory(Request request, RequestStatus oldStatus, string note)
        {
            var history = new RequestHistory
            {
                RequestId = request.Id,
                UserId = _currentUserService.UserId.Value,
                NewStatus = request.Status,
                OldStatus = oldStatus,
                Note = note,
                CreatedAt = DateTime.UtcNow
            };
            await _requestHistoryRepository.InsertAsync(history);
            await _requestHistoryRepository.SaveChangesAsync();
        }

        public async Task<List<GetRequestHistoryResponse>> GetRequestHistory(int requestId)
        {
            var history = await _requestHistoryRepository.GetAll()
                .Include(h => h.User)
                .Where(h => h.RequestId == requestId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            return history.Select(h => new GetRequestHistoryResponse
            {
                Id = h.Id,
                RequestId = h.RequestId,
                UserName = h.User.Name,
                NewStatus = h.NewStatus,
                OldStatus = h.OldStatus,
                Note = h.Note,
                CreatedAt = h.CreatedAt
            }).ToList();
        }
    }
}
