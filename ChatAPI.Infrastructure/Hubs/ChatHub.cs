using ChatAPI.Domain.DTOs;
using ChatAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = "CustomTokenScheme")]
    public class ChatHub : Hub, IChatHub
	{
		public ChatHub()
		{
		}

		public async Task SendMessage(SendMessageDto message)
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
    }
}

