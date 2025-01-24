using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza.Models
{
    [Table("OrderTable", Schema = "public")]
    public class Order
    {
        [Key, Column("idOrder")]
        public Guid? OrderId { get; set; }
        [Column("idClient")]
        public Guid? IdClient { get; set; }
        [Column("Discount")]
        public int? Discount { get; set; }
        [Column("TimeOrder")]
        public DateTime TimeOrder { get; set; }
    }
}
