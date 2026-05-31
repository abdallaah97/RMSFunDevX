using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Providers
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("id")?.Value;
        }
    }
}
