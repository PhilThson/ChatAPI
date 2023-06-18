using ChatAPI.Domain.Helpers;
using ChatAPI.Infrastructure.Exceptions;

namespace ChatAPI.Api.Extensions
{
    public static class GeneralExtensions
	{
		public static string GetUserId(this HttpContext httpContext) =>
            httpContext.User
				?.Claims
				?.FirstOrDefault(c => c.Type == ChatConstants.UserIdClaim)
				?.Value ??
				throw new UnknownUserException();
	}
}

