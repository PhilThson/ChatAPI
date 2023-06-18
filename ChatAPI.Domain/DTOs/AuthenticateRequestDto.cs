using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Domain.DTOs
{
    public class AuthenticateRequestDto
	{
        [Required]
        public string? Username { get; set; }

        //[Required]
        //public string? Password { get; set; }
    }
}

