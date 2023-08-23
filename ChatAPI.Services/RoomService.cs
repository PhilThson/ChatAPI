using AutoMapper;
using ChatAPI.Domain.DTOs.Update;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;
using ChatAPI.Domain.Models;
using ChatAPI.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services;

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

        var roomParticipants = await _unitOfWork.Participant
            .GetByConditionAsync(p => p.RoomId == id && p.IsActive)
            .ToListAsync();

        if (!roomParticipants.Any(p => p.UserId == userId))
            throw new AuthorizationException();

        room.Participants = roomParticipants;

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
            throw new NotFoundException($"Room does not exist ({update.Id})");

        room.Participants = await _unitOfWork.Participant
            .GetByConditionAsync(p => p.RoomId == room.Id && p.IsActive)
            .ToListAsync();

        if (room.Participants.FirstOrDefault(p => p.UserId == userId)?.IsAdmin == false)
            throw new DataValidationException("Only room admin can modify room data");

        room.Name = update.Name;

        await _unitOfWork.SaveAsync();

        return _mapper.Map<T>(room);
    }
}

