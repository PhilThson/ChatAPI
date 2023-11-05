using ChatAPI.Domain.DTOs;
using ChatAPI.Domain.Helpers;
using ChatAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Infrastructure.Hubs
{
    [Authorize(Policy = ChatConstants.TokenPolicy)]
    public class ChatHub : Hub, IChatHub
	{
		public ChatHub()
		{
		}

		public async Task SendMessage(MessageDto message)
		{
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public string AuthorizedResource()
        {
            var content = new
            {
                UserId = Context.UserIdentifier,
                Claims = Context.User.Claims.Select(x => new { x.Type, x.Value })
            };

            return "authorized resource";
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("SystemMessage", new MessageDto
            {
                RoomId = 2003,
                Content = $"{Context.UserIdentifier} joined.",
                Username = "System"
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("SystemMessage", new MessageDto
            {
                RoomId = 2003,
                Content = $"{Context.UserIdentifier} has left.",
                Username = "System"
            });

            await base.OnDisconnectedAsync(exception);
        }
    }
}

