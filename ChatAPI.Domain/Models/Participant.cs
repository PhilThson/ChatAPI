using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatAPI.Domain.Models
{
    public class Participant
	{
		public Participant()
		{
			Messages = new HashSet<Message>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int UserId { get; set; }

		public int RoomId { get; set; }

		public bool IsAdmin { get; set; } = false;

		public bool IsActive { get; set; } = true;

		[JsonIgnore]
		[ForeignKey(nameof(RoomId))]
		public virtual Room Room { get; set; }

		[JsonIgnore]
		public virtual ICollection<Message> Messages { get; set; }
	}
}

