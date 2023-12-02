using System.ComponentModel.DataAnnotations;

namespace CustomerBackend.DTOs
{
    public class ProductsUpdateDTO:ProductsCreateDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
