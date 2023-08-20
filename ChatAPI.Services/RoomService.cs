using AutoMapper;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;
using ChatAPI.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class RoomService : IRoomService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAll<T>() =>
            _mapper.Map<IEnumerable<T>>(await _unitOfWork.Room.GetAllAsync());

        public async Task<T> GetById<T>(int id, int userId)
        {
            var room = await _unitOfWork.Room.GetByIdAsync(id) ??
                throw new NotFoundException($"Room does not exist ({id})");

            var roomParticipants = await _unitOfWork.Participant.GetByConditionAsync(
                    p => p.RoomId == id && p.IsActive)
                .ToListAsync();

            if (!roomParticipants.Any(p => p.UserId == userId))
                throw new AuthorizationException();

            room.Participants = roomParticipants;

            return _mapper.Map<T>(room);
        }
    }
}

