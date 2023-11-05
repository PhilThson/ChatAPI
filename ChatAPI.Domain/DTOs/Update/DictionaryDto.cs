using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Domain.DTOs.Update
{
    public class DictionaryDto<T>
	{
		[Required]
		public T? Id { get; set; }

		[Required]
        [StringLength(64)]
        public string? Name { get; set; }
	}
}

