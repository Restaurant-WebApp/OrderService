namespace OrderAPI.Messages
{
    public class CartDetails
    {
        public Guid CartDetailsId { get; set; }
        public Guid CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Count { get; set; }
    }
}