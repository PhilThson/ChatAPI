using ChatAPI.Domain.Helpers;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;
using ChatAPI.Domain.Settings;
using ChatAPI.Infrastructure.Repository;
using ChatAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatAPI.Api.Extensions
{
    public static class ServiceExtensions
	{
		public static AuthenticationBuilder AddTokenAuthentication(this IServiceCollection services,
            JwtSettings jwtSettings)
		{
            var tokenValidationParameters = new TokenValidationParameters
            {
                //określenie jak będzie przebiegało uwierzytelnianie
                //na chronionych endpointach
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                //Ważne: sprawdzanie zgodnie z zapamiętanym kluczem
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            //można zarejestrować w celu ponownego użycia w aplikacji
            //services.AddSingleton(tokenValidationParameters);

            //Specjalnie nie jest ustawiony domyślny schemat uwierzytelniania, 
            //bo będzie on nadpisany w HUBie
            return services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = tokenValidationParameters;

                    //o.Events = new JwtBearerEvents()
                    //{
                    //    OnMessageReceived = (context) =>
                    //    {
                    //        var path = context.HttpContext.Request.Path;
                    //        if (path.StartsWithSegments("/token"))
                    //        {
                    //normalnie wartość tokenu przychodzi w nagłówku Authorization
                    //ale na potrzeby SignalR może bedzie w Query
                    //            var accessToken = context.Request.Query["access_token"];

                    //            if (!string.IsNullOrWhiteSpace(accessToken))
                    //            {
                    //                //context.Token = accessToken;

                    //                var claims = new Claim[]
                    //                {
                    //                    new("user_id", accessToken),
                    //                    new("token", "token_claim"),
                    //                };
                    //                var identity = new ClaimsIdentity(claims, "CustomTokenScheme");
                    //                context.Principal = new ClaimsPrincipal(identity);
                    //                context.Success();
                    //            }
                    //        }

                    //        return Task.CompletedTask;
                    //    },
                    //};
                });
        }

        public static IServiceCollection AddTokenAuthorizationPolicy(this IServiceCollection services)
        {
            return services.AddAuthorization(c =>
            {
                c.AddPolicy(ChatConstants.TokenPolicy, policy => policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireClaim(ChatConstants.UserIdClaim)
                    //.RequireAuthenticatedUser()
                    );
            });
        }

        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder
                    .SetIsOriginAllowed(origin => true)
                    //.WithOrigins("http://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}

