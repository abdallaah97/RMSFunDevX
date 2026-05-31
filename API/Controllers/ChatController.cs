using Application.Servces.ChatService;
using Application.Servces.ChatService.DTOs;
using Application.Servces.CurrentUserService;
using API.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, ICurrentUserService currentUserService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _currentUserService = currentUserService;
            _hubContext = hubContext;
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentChats()
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId == null) return Unauthorized();

            var recentChats = await _chatService.GetRecentChatsAsync(currentUserId.Value);
            return Ok(recentChats);
        }

        [HttpGet("history/{otherUserId}")]
        public async Task<IActionResult> GetHistory(int otherUserId)
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId == null) return Unauthorized();

            var messages = await _chatService.GetChatHistoryAsync(currentUserId.Value, otherUserId);
            return Ok(messages);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto request)
        {
            var senderId = _currentUserService.UserId;
            if (senderId == null) return Unauthorized();

            var chatMessage = await _chatService.SendMessageAsync(senderId.Value, request.ReceiverId, request.Content);

            await _hubContext.Clients.User(request.ReceiverId.ToString()).SendAsync("ReceiveMessage", chatMessage);
            await _hubContext.Clients.User(senderId.Value.ToString()).SendAsync("ReceiveMessage", chatMessage);

            return Ok(chatMessage);
        }

        [HttpPost("MarkMessagesAsRead/{senderId}")]
        public async Task<IActionResult> MarkMessagesAsRead(int senderId)
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId == null) return Unauthorized();

            await _chatService.MarkMessagesAsReadAsync(currentUserId.Value, senderId);

            await _hubContext.Clients.User(senderId.ToString()).SendAsync("MarkMessagesAsRead", currentUserId.Value);
            
            await _hubContext.Clients.User(currentUserId.Value.ToString()).SendAsync("MarkMessagesAsRead", senderId);

            return Ok();
        }
    }
}
