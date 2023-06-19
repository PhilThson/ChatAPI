using AutoMapper;
using ChatAPI.Domain.DTOs;
using ChatAPI.Domain.Models;

namespace ChatAPI.Api.MappingProfiles
{
	public class ReadProfile : Profile
	{
		public ReadProfile()
		{
			CreateMap<Room, ReadRoomDto>();
		}
	}
}

