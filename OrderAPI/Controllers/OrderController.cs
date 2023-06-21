using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;

namespace OrderAPI.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderAppDbContext _db;

        public OrderController(OrderAppDbContext context)
        {
            _db = context;
        }

        [HttpDelete]
        [Route("/DeleteUserData")]
        public IActionResult DeleteUserData(string email)
        {
            try
            {
                var orderHeaders = _db.OrderHeaders.Where(o => o.Email == email).ToList();

                if (orderHeaders.Count > 0)
                {
                    var orderHeaderIds = orderHeaders.Select(o => o.OrderHeaderId).ToList();

                    var orderDetails = _db.OrderDetails.Where(od => orderHeaderIds.Contains(od.OrderHeaderId)).ToList();
                    _db.OrderDetails.RemoveRange(orderDetails);

                    _db.OrderHeaders.RemoveRange(orderHeaders);

                    _db.SaveChanges();

                    return Ok("Order headers and associated order details deleted successfully.");
                }
                else
                {
                    return NotFound("No order headers found for the specified email.");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting order headers and order details: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("/DeleteOrderDetails")]
        public IActionResult DeleteOrderDetails(Guid orderDetailsId)
        {
            try
            {                
                var orderDetail = _db.OrderDetails.FirstOrDefault(od => od.OrderDetailsId == orderDetailsId);

                if (orderDetail != null)
                {
                    _db.OrderDetails.Remove(orderDetail);
                    _db.SaveChanges();

                    return Ok("Order detail deleted successfully.");
                }
                else
                {
                    return NotFound("No order detail found for the specified ID.");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the order detail: {ex.Message}");
            }
        }

    }


}
