using ChatAPI.Api.Extensions;
using ChatAPI.Domain.DTOs.Read;
using ChatAPI.Domain.DTOs.Update;
using ChatAPI.Domain.Helpers;
using ChatAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = ChatConstants.TokenPolicy)]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rooms = await _roomService.GetAll<ReadSimpleRoomDto>();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetById<ReadRoomDto>(id, User.GetId());
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            var room = await _roomService.Create<ReadRoomDto>(name, User.GetId());
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] DictionaryDto<int> update)
        {
            var updated = await _roomService.UpdateName<ReadRoomDto>(update, User.GetId());
            return Ok(updated);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}

