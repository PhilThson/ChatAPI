using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Domain.DTOs
{
    public class MessageDto
	{
		[Required]
		public int? RoomId { get; set; }

		[Required]
		[StringLength(2048)]
		public string? Content { get; set; }

		[Required]
		public string? Username { get; set; }
	}
}

