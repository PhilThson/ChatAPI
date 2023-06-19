using ChatAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatAPI.Api.Extensions;
using ChatAPI.Domain.DTOs;
using ChatAPI.Domain.Helpers;

namespace ChatAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = ChatConstants.TokenPolicy)]
        [HttpGet]
        public KeyValuePair<string, string> Get()
        {
            var userId = HttpContext.GetUserId();
            return new KeyValuePair<string, string>("UserId", userId);
        }

        [AllowAnonymous]
        [HttpPost("authenticate"), HttpOptions]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto model)
        {
            var response = await _userService.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var response = await _userService.RefreshToken(refreshToken);
            return Ok(response);
        }
    }
}

