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
            ConfigurationManager configuration)
		{
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            var jwtKey = configuration.GetSection(nameof(JwtKey)).Get<JwtKey>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                //określenie jak będzie przebiegało uwierzytelnianie
                //na chronionych endpointach
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                //Ważne: sprawdzanie zgodnie z zapamiętanym kluczem
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey.Value)),
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

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddSettings(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            services.Configure<JwtKey>(configuration.GetSection(nameof(JwtKey)));
        }
    }
}

