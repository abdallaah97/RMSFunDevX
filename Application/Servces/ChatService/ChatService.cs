using Application.Repositories;
using Application.Servces.ChatService.DTOs;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Application.Servces.ChatService
{
    public class ChatService : IChatService
    {
        private readonly IGenericRepository<ChatMessage> _chatMessageRepository;

        public ChatService(IGenericRepository<ChatMessage> chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<List<ChatMessageDto>> GetChatHistoryAsync(int currentUserId, int otherUserId)
        {
            var query = _chatMessageRepository.GetAll();
            var messages = await query.Where(m =>
                (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead
                })
                .ToListAsync();

            return messages;
        }

        public async Task<List<RecentChatDto>> GetRecentChatsAsync(int currentUserId)
        {
            var query = _chatMessageRepository.GetAll()
                .Include(m => m.Sender)
                .Include(m => m.Receiver);

            var userMessages = await query
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .ToListAsync();

            var recentChats = userMessages
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g =>
                {
                    var lastMessage = g.OrderByDescending(m => m.SentAt).First();
                    var otherUser = lastMessage.SenderId == currentUserId ? lastMessage.Receiver : lastMessage.Sender;
                    var unreadCount = g.Count(m => m.ReceiverId == currentUserId && m.SenderId == otherUser.Id && !m.IsRead);

                    return new RecentChatDto
                    {
                        OtherUserId = otherUser.Id,
                        OtherUserName = otherUser.Name,
                        LastMessageContent = lastMessage.Content,
                        LastMessageDate = lastMessage.SentAt,
                        UnreadCount = unreadCount
                    };
                })
                .OrderByDescending(c => c.LastMessageDate)
                .ToList();

            return recentChats;
        }

        public async Task<ChatMessageDto> SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = System.DateTime.UtcNow,
                IsRead = false
            };

            await _chatMessageRepository.InsertAsync(message);
            await _chatMessageRepository.SaveChangesAsync();

            return new ChatMessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = message.IsRead
            };
        }

        public async Task MarkMessagesAsReadAsync(int currentUserId, int senderId)
        {
            var query = _chatMessageRepository.GetAll();
            var unreadMessages = await query.Where(m => m.ReceiverId == currentUserId && m.SenderId == senderId && !m.IsRead).ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                    _chatMessageRepository.Update(message);
                }
                await _chatMessageRepository.SaveChangesAsync();
            }
        }
    }
}
