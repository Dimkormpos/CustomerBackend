using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CustomerBackend.DTOs
{
    public class CreatePurchaseDTO
    {
        [Required]
        public int CustomerId { get; set; }
        public List<ProductQuantity> Products { get; set; }
        public DateTime PurchaseTimestamp { get; set; }

        public class ProductQuantity{
        
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }   

    }
}
