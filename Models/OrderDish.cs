using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizza.Models
{
    [Table("OrderDishTable", Schema = "public")]
    //[Keyless]
    public class OrderDish
    {
        [Column("idOrder")]
        public Guid? IdOrder { get; set; }
        [Column("idDish")]
        public Guid? IdDish { get; set; }
        [Column("Quantity")]
        public int? Quantity { get; set; }
    }
}
