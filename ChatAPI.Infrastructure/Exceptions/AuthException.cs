using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Infrastructure.Exceptions
{
    public class AuthException : HubException
	{
		public AuthException(string message) : base(message)
		{
		}
	}
}

