namespace OrderAPI.Messages
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public Category CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
    public enum Category
    {
        Starter,
        Mains,
        Dessert
    }
}