using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Pizza.Models
{
    [Table("Menu", Schema = "public")]
    public class Menu
    {
        [Key, Column("idDish")]
        public Guid IdDish { get; set; }
        [Column("Dish")]
        public string? Dish { get; set; }
        [Column("Description")]
        public string? Description { get; set; }
        [Column("Price")]
        public int? Price { get; set; }
    }
}
