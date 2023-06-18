using ChatAPI.Api.CustomMiddleware;

namespace ChatAPI.Api.Extensions
{
    public static class AppExtensions
	{
		public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder appBuilder)
		{
			return appBuilder.UseMiddleware<ExceptionMiddleware>();
		}
	}
}

