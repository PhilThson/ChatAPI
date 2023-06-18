﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatAPI.Domain.Models
{
    public class Room
	{
		public Room()
		{
			Participants = new HashSet<User>();
			Messages = new HashSet<Message>();
		}

		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[StringLength(64)]
		public string Name { get; set; }

		[JsonIgnore]
		public virtual ICollection<User> Participants { get; set; }

		[JsonIgnore]
		public virtual ICollection<Message> Messages { get; set; }
	}
}

