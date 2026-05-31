using Application.Servces.ChatService;
using Application.Servces.CurrentUserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public ChatController(IChatService chatService, ICurrentUserService currentUserService)
        {
            _chatService = chatService;
            _currentUserService = currentUserService;
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
    }
}
