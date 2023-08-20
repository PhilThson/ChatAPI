using AutoMapper;
using ChatAPI.Domain.DTOs.Create;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.Exceptions;

namespace ChatAPI.Services
{
    public class MessageService : IMessageService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<T> GetById<T>(int id)
        {
            var message = await _unitOfWork.Message.GetFirstAsync(m => m.Id == id) ??
                throw new NotFoundException($"Message not found ({id})");

            return _mapper.Map<T>(message);
        }

        public async Task<IEnumerable<T>> GetByRoomId<T>(int roomId) =>
            _mapper.Map<IEnumerable<T>>(await _unitOfWork.Message.GetByRoomId(roomId));

        public async Task<T> Create<T>(CreateMessageDto messageDto, int userId)
        {
            _ = messageDto ?? throw new DataValidationException("Message is empty");
            if (!_unitOfWork.Room.Exists(r => r.Id == messageDto.RoomId))
                throw new NotFoundException("Room does not exist");

            var participant = await _unitOfWork.Participant.GetFirstAsync(p =>
                p.RoomId == messageDto.RoomId
                && p.UserId == userId
                && p.IsActive);

            if (participant is null)
                throw new NotFoundException("Participant not exist");

            var message = _mapper.Map<Message>(messageDto);
            message.SenderId = participant.Id;

            _unitOfWork.Message.Add(message);
            _unitOfWork.Save();

            return _mapper.Map<T>(message);
        }
    }
}

