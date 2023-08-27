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
            IConfiguration configuration)
		{
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            //przechowywane w secrets.json
            var jwtKey = configuration.GetSection(nameof(ChatConstants.JwtKey)).Value;

            var tokenValidationParameters = new TokenValidationParameters
            {
                //określenie jak będzie przebiegało uwierzytelnianie
                //na chronionych endpointach
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                //Ważne: sprawdzanie zgodnie z zapamiętanym kluczem
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
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
                    o.Events = new()
                    {
                        OnMessageReceived = (context) =>
                        {
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/chathub"))
                            {
                                var accessToken = context.Request.Query["access_token"];
                                if (string.IsNullOrEmpty(accessToken))
                                    accessToken = context.HttpContext.GetToken();

                                if (!string.IsNullOrWhiteSpace(accessToken))
                                {
                                    context.Token = accessToken;
                                }
                            }

                            return Task.CompletedTask;
                        },
                    };
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

        public static IServiceCollection EnableCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(ChatConstants.CorsPolicy, builder => builder
                    .SetIsOriginAllowed(isOriginAllowed: _ => true)
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:3000", "https://localhost:7129")
                    .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
                    );
            });
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IMessageService, MessageService>();
        }

        public static void AddSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }
    }
}

