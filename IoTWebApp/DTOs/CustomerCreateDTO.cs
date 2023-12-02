using System.ComponentModel.DataAnnotations;

namespace CustomerBackend.DTOs
{
    public class CustomerCreateDTO
    {
        public string? Email { get;set; }
        public string? Name { get; set; }

    }
    
}
