using System.ComponentModel.DataAnnotations;

namespace CustomerBackend.DTOs
{
    public class CustomerUpdateDTO : CustomerCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
