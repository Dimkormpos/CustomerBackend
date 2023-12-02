using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomerBackend.Database
{
    public class Purchases
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column("purchase_date", TypeName = "timestamp")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column("product_id", TypeName = "int")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]  
        public virtual Products Product { get; set; }

        [Column("customer_id", TypeName = "int")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }
    }
}
