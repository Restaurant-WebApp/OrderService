using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public Guid OrderDetailsId { get; set; }
        public Guid OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
