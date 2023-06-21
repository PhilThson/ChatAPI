using AutoMapper;
using ChatAPI.Domain.DTOs.Create;
using ChatAPI.Domain.Models;

namespace ChatAPI.Api.MappingProfiles
{
    public class CreateProfile : Profile
	{
		public CreateProfile()
		{
			CreateMap<CreateMessageDto, Message>();
		}
	}
}

