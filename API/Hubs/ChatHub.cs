using Microsoft.AspNetCore.SignalR;
using Application.Servces.ChatService;
using System.Threading.Tasks;
using Application.Servces.CurrentUserService;
using Microsoft.AspNetCore.Authorization;

namespace API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public ChatHub(IChatService chatService, ICurrentUserService currentUserService)
        {
            _chatService = chatService;
            _currentUserService = currentUserService;
        }

        public async Task SendMessage(int receiverId, string message)
        {
            var senderId = _currentUserService.UserId;
            if (senderId != null)
            {
                var chatMessage = await _chatService.SendMessageAsync(senderId.Value, receiverId, message);
                
                await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatMessage);
                
                await Clients.Caller.SendAsync("ReceiveMessage", chatMessage);
            }
        }

        public async Task MarkMessagesAsReadAsync(int currentUserId, int senderId)
        {
            await _chatService.MarkMessagesAsReadAsync(currentUserId, senderId);

            await Clients.User(senderId.ToString()).SendAsync("MarkMessagesAsRead", senderId);

            await Clients.Caller.SendAsync("MarkMessagesAsRead", senderId);
        }
    }
}

