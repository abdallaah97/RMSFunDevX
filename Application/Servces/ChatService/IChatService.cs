using Application.Servces.ChatService.DTOs;

namespace Application.Servces.ChatService
{
    public interface IChatService
    {
        Task<List<ChatMessageDto>> GetChatHistoryAsync(int currentUserId, int otherUserId);
        Task<List<RecentChatDto>> GetRecentChatsAsync(int currentUserId);
        Task<ChatMessageDto> SendMessageAsync(int senderId, int receiverId, string content);
        Task MarkMessagesAsReadAsync(int currentUserId, int senderId);
    }
}
