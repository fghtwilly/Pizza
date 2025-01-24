namespace Pizza.Models.ViewModel
{
    public class OrderHistoryViewModel
    {
        public Dictionary<Order, List<OrderDish>>? orderDictionary {  get; set; }
        public List<Menu>? menu { get; set; }
        public OrderHistoryViewModel()
        {
            orderDictionary = new Dictionary<Order, List<OrderDish>>();
            menu = new List<Menu>();
        }
    }
}
