using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pizza.Models;
using Pizza.Models.DBContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Pizza.Controllers
{
    public class Authentication : Controller
    {        
        public string pathClient = "C:\\Users\\Михаил\\ПАПКА\\IT\\C#\\aspnet\\Pizza\\wwwroot\\Clients";

        public IActionResult Index()
        {            
            return View();
        }

        
        [HttpPost]
        public IActionResult TakeLogin(string username, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var clients = db.Clients.ToList();
                foreach (var item in clients) 
                {
                    if (item.UserName == username && item.Password == password) 
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, item.UserName, ClaimValueTypes.String),
                            new Claim(ClaimTypes.Role, item.Role)
                        };
                        var jwt = new JwtSecurityToken(
                                issuer: AuthOptions.ISSUER,
                                audience: AuthOptions.AUDIENCE,
                                claims: claims,
                                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                        var response = new
                        {
                            role = item.Role,
                            access_token = encodedJwt,
                            username = item.UserName
                        };                        
                        return Json( response );
                    }
                }
            }
            return BadRequest("Неверные логин или пароль");
        }
        
        [HttpPost]
        public IActionResult RegistrationSave(Client newclient)   //сохранение зарегистрированного пользователя
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Clients.Add(newclient);
                db.SaveChanges();
            }
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, newclient.UserName, ClaimValueTypes.String),
                            new Claim(ClaimTypes.Role, newclient.Role)
                        };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                role = newclient.Role,
                access_token = encodedJwt,
                username = newclient.UserName
            };
            return Json(response);
        }
    }
}
