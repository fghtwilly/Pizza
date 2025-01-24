using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pizza.Models;
using Pizza.Models.DBContext;
using Pizza.Models.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pizza.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]        
        [Authorize(Roles = "User")]
        public IActionResult Login_User(string username)
        {
            UserViewModel model = new UserViewModel();
            using (ApplicationContext db = new ApplicationContext())
            {
                var clients = db.Clients.ToList();
                foreach (var item in clients)
                {
                    if (item.UserName == username)
                    {
                        model.client = item;
                    }
                }
                model.menu = db.Menus.ToList();
                return PartialView("_User", model);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public IActionResult Login_Operator(string username)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                OrderHistoryViewModel model = new OrderHistoryViewModel();
                model.menu = db.Menus.ToList();
                List<OrderDish> orderDishes = db.orderDish.ToList();
                List<Order> orders = db.Orders.ToList();
                foreach(Order order in orders)
                {
                    List<OrderDish> orderDishAdd = new List<OrderDish>();
                    foreach(var dish in orderDishes)
                    {
                        if(dish.IdOrder == order.OrderId)
                            orderDishAdd.Add(dish);
                    }
                    model.orderDictionary.Add(order, orderDishAdd);
                }
                return PartialView("_Operator", model);
            }            
        }
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public IActionResult GetMenu()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Menu> menu = db.Menus.ToList();
                return PartialView("_ChangeMenu", menu);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public IActionResult SaveChangeMenu(string? jsonMenu)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Menus.ExecuteDelete();                                   //? конфликт истрии заказов и нового меню
                var options = new JsonSerializerOptions()
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString |
                    JsonNumberHandling.WriteAsString
                };
                List<Menu> menu = JsonSerializer.Deserialize<List<Menu>>(jsonMenu, options);  //? убрать json
                foreach(var item in menu)
                {
                    db.Menus.Add(item);
                }
                db.SaveChanges();
            }
            return Json(true);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult GetOrder(List<OrderDish> userOrder, string idUser)
        {
            Guid newIdOrder = Guid.NewGuid();
            int? discountClient = 0;
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var item in userOrder)
                {
                    item.IdOrder = newIdOrder;
                    db.orderDish.Add(item);
                }
                //db.AddRange(userOrder);
                var clients = db.Clients.ToList();
                foreach (var item in clients)
                {
                    if (item.Id == Guid.Parse(idUser))
                    {
                        discountClient = item.Discount;
                    }
                }
                Order newOrder = new Order();
                newOrder.OrderId = newIdOrder;
                newOrder.IdClient = Guid.Parse(idUser);
                newOrder.Discount = discountClient;
                newOrder.TimeOrder = DateTime.Now;
                db.Orders.Add(newOrder);
                db.SaveChanges();
                return Json(true);
            }                     
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult GetOrderHistory(string idUser)
        {
            OrderHistoryViewModel model = new OrderHistoryViewModel();
            //Dictionary<Order, List<OrderDish>> model = new Dictionary<Order, List<OrderDish>>();
            //List<Order> getOrderHistory = new List<Order>();
            //OrderHistoryViewModel model = new OrderHistoryViewModel();
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Order> getOrderHistory = db.Orders.ToList();
                List<OrderDish> getOrderDish = db.orderDish.ToList();
                foreach (var item in getOrderHistory)
                {
                    List<OrderDish> orderDishAdd = new List<OrderDish>();
                    if (item.IdClient == Guid.Parse(idUser))
                    {
                        foreach (var order in getOrderDish)
                        {
                            if (order.IdOrder == item.OrderId)
                                orderDishAdd.Add(order);
                        }
                        model.orderDictionary.Add(item, orderDishAdd);
                    }
                }
                model.menu = db.Menus.ToList();
                return PartialView("_HistoryOrder", model);
            }
        }
    }
}