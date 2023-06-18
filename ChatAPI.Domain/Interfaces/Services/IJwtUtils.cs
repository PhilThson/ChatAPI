using ChatAPI.Domain.Models;

namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IJwtUtils
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}