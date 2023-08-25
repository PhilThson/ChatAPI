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

			CreateMap<Message, ReadMessageDto>()
				.ForMember(d => d.UserId, o => o.MapFrom(s => s.Sender.UserId));

			CreateMap<Participant, ReadParticipantDto>();
		}
	}
}

