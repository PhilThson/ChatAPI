using System.Security.Claims;
using ChatAPI.Domain.Helpers;
using ChatAPI.Infrastructure.Exceptions;

namespace ChatAPI.Api.Extensions
{
    public static class GeneralExtensions
	{
		public static int GetId(this ClaimsPrincipal user)
		{
			var userIdString = user?.FindFirst(c => c.Type == ChatConstants.UserIdClaim)?
				.Value ??
				throw new UnknownUserException();

			if (!int.TryParse(userIdString, out int userId))
                throw new UnknownUserException($"Invalid User Id ({userIdString})");

			return userId;
        }

		public static string GetName(this ClaimsPrincipal user) =>
            //można w ten sposób, albo z Claimsów dołączonych na etapie tworzenia tokenu
            //context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			//tutaj inna koncepcja metody rozszerzającej
			//Dlaczego claim dodany jako JwtRegisteredClaimName.Sub
			//wpisał się jako nameidentifier? Nie wiem.
			user?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)
				?.Value ??
				throw new UnknownUserException();

		public static string GetToken(this HttpContext httpContext) =>
            httpContext.Request.Headers["Authorization"]
				.FirstOrDefault()
				?.Split(" ")
				.Last();
    }
}

