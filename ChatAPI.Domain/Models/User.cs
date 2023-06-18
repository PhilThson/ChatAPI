using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatAPI.Domain.Models
{
    public class User
	{
		public User()
		{
			UserMessages = new HashSet<Message>();
			UserRooms = new HashSet<Room>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[StringLength(64)]
		public string Name { get; set; }

		[StringLength(512)]
		public string? PasswordHash { get; set; }

		[StringLength(512)]
        public string? RefreshToken { get; set; }

		public DateTime? RefreshTokenExpiration { get; set; }

		public bool RefreshTokenIsRevoked { get; set; } = false;

		public bool IsActive { get; set; } = true;

		[JsonIgnore]
		public virtual ICollection<Message> UserMessages { get; set; }

		[JsonIgnore]
		public virtual ICollection<Room> UserRooms { get; set; }
	}
}

