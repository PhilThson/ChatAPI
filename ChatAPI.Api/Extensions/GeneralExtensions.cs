using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatAPI.Domain.Helpers;
using ChatAPI.Infrastructure.Exceptions;

namespace ChatAPI.Api.Extensions
{
    public static class GeneralExtensions
	{
		public static int GetUserId(this HttpContext httpContext)
		{
			var userIdString = httpContext.User
				?.Claims
				?.FirstOrDefault(c => c.Type == ChatConstants.UserIdClaim)
				?.Value ??
				throw new UnknownUserException();

			if (!int.TryParse(userIdString, out int userId))
                throw new UnknownUserException($"Invalid User Id ({userIdString})");

			return userId;
        }

		public static string GetUserName(this ClaimsPrincipal user) =>
            //można w ten sposób, albo z Claimsów dołączonych na etapie tworzenia tokenu
            //context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			//tutaj inna koncepcja metody rozszerzającej
			user?.FindFirst(c => c.Type == JwtRegisteredClaimNames.Sub)
				?.Value ??
				throw new UnknownUserException();
    }
}

