namespace OrderAPI.Models
{
    public class OrderHeader
    {
        public Guid OrderHeaderId { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public int CartTotalItems { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
