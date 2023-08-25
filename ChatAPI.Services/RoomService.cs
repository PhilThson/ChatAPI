using AutoMapper;
using ChatAPI.Domain.DTOs.Update;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace ChatAPI.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RoomService> _logger;

    public RoomService(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<RoomService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetAll<T>(string userName)
    {
        var rooms = await _unitOfWork.Room.GetAllAsync();
        _logger.LogInformation("User ({userName}) see all rooms", userName);
        return _mapper.Map<IEnumerable<T>>(rooms);
    }

    public async Task<T> GetById<T>(int roomId, int userId)
    {
        var room = await _unitOfWork.Room.GetByIdWithMessagesAsync(roomId) ??
            throw new NotFoundException($"Room does not exists ({roomId})");

        room.Participants = await _unitOfWork.Participant.GetRoomParticipants(roomId);

        if (!room.Participants.Any(p => p.UserId == userId))
            throw new AuthorizationException();

        return _mapper.Map<T>(room);
    }

    public async Task<T> Create<T>(string name, int userId)
    {
        if (string.IsNullOrEmpty(name))
            throw new DataValidationException("Room name can not be empty");

        if (_unitOfWork.Room.Exists(r => r.Name == name.Trim()))
            throw new DataValidationException("Given room name is already taken");

        var room = new Room { Name = name.Trim() };
        room.Participants.Add(new Participant
        {
            UserId = userId,
            IsAdmin = true
        });

        await _unitOfWork.Room.AddAsync(room);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<T>(room);
    }

    public async Task<T> UpdateName<T>(DictionaryDto<int> update, int userId)
    {
        update.Name = update.Name!.Trim();

        if (_unitOfWork.Room.Exists(r => r.Name == update.Name))
            throw new DataValidationException("Given room name is already taken");

        var room = await _unitOfWork.Room.FindByIdAsync(update.Id) ??
            throw new NotFoundException($"Room does not exists ({update.Id})");

        room.Participants = await _unitOfWork.Participant.GetRoomParticipants(room.Id);

        if (room.Participants.FirstOrDefault(p => p.UserId == userId)?.IsAdmin == false)
            throw new DataValidationException("Only room admin can modify room data");

        room.Name = update.Name;

        await _unitOfWork.SaveAsync();

        return _mapper.Map<T>(room);
    }

    public async Task Delete(int roomId, int userId)
    {
        var room = await _unitOfWork.Room.FindByIdAsync(roomId) ??
            throw new NotFoundException($"Room does not exists ({roomId})");

        var participants = await _unitOfWork.Participant.GetRoomParticipants(roomId);
        if (room.Participants.FirstOrDefault(p => p.UserId == userId)?.IsAdmin == false)
            throw new DataValidationException("Only room admin can delete room");

        _unitOfWork.Room.Delete(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task Join(int roomId, int userId)
    {
        if (!_unitOfWork.Room.Exists(r => r.Id == roomId))
            throw new NotFoundException($"Room does not exists ({roomId})");

        var roomParticipant =
            await _unitOfWork.Participant.GetFirstAsync(p =>
                p.RoomId == roomId && p.UserId == userId);

        if (roomParticipant is null)
        {
            _unitOfWork.Participant.Add(new Participant
            {
                RoomId = roomId,
                UserId = userId
            });
        }

        if (roomParticipant?.IsActive == false)
            roomParticipant.IsActive = true;

        await _unitOfWork.SaveAsync();
    }
}

