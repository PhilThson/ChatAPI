namespace ChatAPI.Domain.DTOs.Read
{
    public class ReadRoomDto : ReadSimpleRoomDto
	{
        public IEnumerable<ReadParticipantDto> Participants { get; set; }
        public IEnumerable<ReadMessageDto> Messages { get; set; }
    }
}

