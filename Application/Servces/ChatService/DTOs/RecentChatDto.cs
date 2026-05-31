using System;

namespace Application.Servces.ChatService.DTOs
{
    public class RecentChatDto
    {
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime LastMessageDate { get; set; }
        public int UnreadCount { get; set; }
    }
}
