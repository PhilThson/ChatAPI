using ChatAPI.Api.Extensions;
using ChatAPI.Domain.DTOs.Create;
using ChatAPI.Domain.DTOs.Read;
using ChatAPI.Domain.Helpers;
using ChatAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = ChatConstants.TokenPolicy)]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("room/{roomId:int}")]
        public async Task<IActionResult> GetByRoomId(int roomId)
        {
            return Ok(await _messageService.GetByRoomId<ReadMessageDto>(roomId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _messageService.GetById<ReadMessageDto>(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMessageDto messageDto)
        {
            var createdMessage =
                await _messageService.Create<ReadMessageDto>(messageDto, User.GetId());

            return CreatedAtAction(
                nameof(GetById), new { id = createdMessage.Id }, createdMessage);
        }
    }
}

