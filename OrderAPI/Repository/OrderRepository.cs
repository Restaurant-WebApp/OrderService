using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;

namespace OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<OrderAppDbContext> _dbContext;

        public OrderRepository(DbContextOptions<OrderAppDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new OrderAppDbContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;

        }
    }
}
