using ChatAPI.Domain.DTOs;

namespace ChatAPI.Domain.Interfaces.Services
{
    public interface IUserService
	{
        Task<AuthenticateResponseDto> Authenticate(AuthenticateRequestDto model);
        Task<AuthenticateResponseDto> RefreshToken(string refreshToken);
    }
}

