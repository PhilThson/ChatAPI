using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatAPI.Domain.Models
{
    public class Message
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

		[StringLength(2048)]
		public string? Content { get; set; }

		public int RoomId { get; set; }

		public int UserId { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(UserId))]
		public virtual User? User { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(RoomId))]
		public virtual Room? Room { get; set; }
	}
}

