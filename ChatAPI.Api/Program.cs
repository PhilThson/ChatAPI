using System.Reflection;
using System.Text.Json.Serialization;
using ChatAPI.Api.Extensions;
using ChatAPI.Domain.Helpers;
using ChatAPI.Infrastructure.DataAccess;
using ChatAPI.Infrastructure.Hubs;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o
        .JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Chat"));
});

var configuration = builder.Configuration;

builder.Services.AddSettings(configuration);

builder.Services.EnableCors();
builder.Services.AddTokenAuthentication(configuration);
builder.Services.AddTokenAuthorizationPolicy();

builder.Services.AddServices();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseExceptionMiddleware();

app.UseCors(ChatConstants.CorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.MapGet("/", async (context) =>
{
    await context.Response.WriteAsync(" - Hello in root - ");
});

app.MapGet("/token", async ctx =>
{
    ctx.Response.StatusCode = 200;
    await ctx.Response
        .WriteAsync(ctx.User?.Claims
            .FirstOrDefault(x => x.Type == ChatConstants.UserIdClaim)
            ?.Value);
}).RequireAuthorization(ChatConstants.TokenPolicy);

app.Run();
