using AutoMapper;
using ChatAPI.Domain.DTOs.Read;
using ChatAPI.Domain.Models;

namespace ChatAPI.Api.MappingProfiles
{
    public class ReadProfile : Profile
	{
		public ReadProfile()
		{
			CreateMap<Room, ReadRoomDto>();

			CreateMap<Room, ReadSimpleRoomDto>();

			CreateMap<Message, ReadMessageDto>();

			CreateMap<Participant, ReadParticipantDto>();
		}
	}
}

