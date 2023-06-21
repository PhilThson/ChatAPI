using AutoMapper;
using ChatAPI.Domain.DTOs.Read;
using ChatAPI.Domain.Models;

namespace ChatAPI.Api.MappingProfiles
{
    public class ReadProfile : Profile
	{
		public ReadProfile()
		{
			CreateMap<User, ReadUserDto>();

			CreateMap<Room, ReadRoomDto>();

			CreateMap<Message, ReadMessageDto>();
		}
	}
}

