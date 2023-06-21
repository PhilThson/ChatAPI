using ChatAPI.Domain.DTOs;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;
using ChatAPI.Domain.Settings;
using ChatAPI.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;

namespace ChatAPI.Services
{
    public class UserService : IUserService
	{
        private readonly IJwtUtils _jwtUtils;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;


        public UserService(IJwtUtils jwtUtils,
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> options)
        {
            _jwtUtils = jwtUtils;
            _unitOfWork = unitOfWork;
            _jwtSettings = options.Value;
        }

        public async Task<AuthenticateResponseDto> Authenticate(AuthenticateRequestDto model)
        {
            var user = await _unitOfWork.User.GetFirstAsync(u => u.Name == model.Username) ??
                throw new NotFoundException("User not exists");

            if (!user.IsActive)
                throw new UnauthorizedAccessException();

            var expirationDays = GetRefreshTokenExpirationTime();

            var jwtToken = _jwtUtils.GenerateToken(user);
            user.RefreshToken = _jwtUtils.GenerateRefreshToken();
            user.RefreshTokenExpiration = DateTime.Now.AddDays(expirationDays);
            user.RefreshTokenIsRevoked = false;

            _unitOfWork.Save();

            return new AuthenticateResponseDto
            {
                JwtToken = jwtToken,
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<AuthenticateResponseDto> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new DataValidationException("No refresh token provided");

            var user = await _unitOfWork.User.GetFirstAsync(u => u.RefreshToken == refreshToken) ??
                throw new DataValidationException("Invalid token provided");

            if (user.RefreshTokenIsRevoked)
                throw new AuthenticationException("Refresh token is revoked");

            if (user.RefreshTokenExpiration!.Value < DateTime.Now)
                throw new AuthenticationException("Refresh token expired");

            var expirationDays = GetRefreshTokenExpirationTime();

            var newRefreshToken = _jwtUtils.GenerateRefreshToken();
            var newJwtToken = _jwtUtils.GenerateToken(user);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.Now.AddDays(expirationDays);

            _unitOfWork.Save();

            return new AuthenticateResponseDto
            {
                JwtToken = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }

        private int GetRefreshTokenExpirationTime()
        {
            if (!int.TryParse(_jwtSettings.RefreshTokenExpirationTimeDays, out int expirationDays))
                throw new Exception("Error parsing refresh token expiration time");

            return expirationDays;
        }
    }
}

