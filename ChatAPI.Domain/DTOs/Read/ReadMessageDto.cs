﻿namespace ChatAPI.Domain.DTOs.Read
{
    public class ReadMessageDto
	{
		public long Id { get; set; }
		public string Content { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
	}
}

