using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Xml.Linq;

namespace Pizza.Models
{
    [Table("ClientTable", Schema = "public")]
    public class Client
    {
        [Column("idClient")]
        public Guid Id { get; set; }
        [Column("UserName")]
        public string? UserName { get; set; }
        [Column("Password")]
        public string? Password { get; set; }
        [Column("Name")]
        public string? Name { get; set; }
        [Column("MiddleName")]
        public string? MiddleName { get; set; }
        [Column("LastName")]
        public string? LastName { get; set; }
        [Column("Adress")]
        public string? Adress { get; set; }
        [Column("Phone")]
        public int? Phone { get; set; }
        [Column("Discount")]
        public int? Discount { get; set; }
        //public string? MyOrders { get; set; }
        [Column("Role")]
        public string Role { get; set; }
        public Client()
        {
            Id = Guid.NewGuid();
        }
        
        public Client(string stringGuid)
        {
            Id = Guid.Parse(stringGuid);           
        }
        /*public IEnumerable<Claim> GetUserClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserName, ClaimValueTypes.String),
                new Claim(ClaimTypes.Role, Role)
            };
            //добавляем роли пользователя в список                    
            return claims;
        }*/
    }
}
