﻿using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Infrastructure.Helpers
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
        }
    }
}

