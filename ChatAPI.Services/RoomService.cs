using AutoMapper;
using ChatAPI.Domain.Interfaces.Repository;
using ChatAPI.Domain.Interfaces.Services;

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
    }
}

