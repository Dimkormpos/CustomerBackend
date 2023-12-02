using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomerBackend.Database
{
    public class Customers
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("name", TypeName = "text")]
        public string Name { get; set; }

        [Column("email", TypeName = "text")]
        public string Email { get; set; }

        [InverseProperty("Customer")]
        public virtual List<Purchases> Purchases { get; set; }
    }
}
