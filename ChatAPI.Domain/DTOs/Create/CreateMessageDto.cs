using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Domain.DTOs.Create
{
    public class CreateMessageDto
    {
        [Required]
        [StringLength(2048)]
        public string? Content { get; set; }

        [Required]
        public int? RoomId { get; set; }
    }
}