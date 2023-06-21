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
            _mapper.Map<IEnumerable<T>>(
                await _unitOfWork.Message.GetByRoomId(roomId));

        public async Task<T> Create<T>(CreateMessageDto messageDto, int userId)
        {
            _ = messageDto ?? throw new DataValidationException("Message is empty");
            if (!_unitOfWork.Room.Exists(r => r.Id == messageDto.RoomId))
                throw new DataValidationException("Room does not exists");

            var message = _mapper.Map<Message>(messageDto);
            message.UserId = userId;
            _unitOfWork.Message.Add(message);
            _unitOfWork.Save();

            var created = await _unitOfWork.Message.GetById(message.Id) ??
                throw new NotFoundException($"Message not found ({message.Id})");

            return _mapper.Map<T>(created);
        }
    }
}

