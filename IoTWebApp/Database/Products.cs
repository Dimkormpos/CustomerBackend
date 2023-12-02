using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerBackend.Database
{
    public class Products
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("name", TypeName = "text")]
        public string Name { get; set; }

        [Column("price", TypeName = "float(24)")]
        public float Price { get; set; }

        [InverseProperty("Product")]
        public virtual List<Purchases> Purchases { get; set; }
    }
}
