using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;

namespace OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderAppDbContext _dbContext;

        public OrderRepository(OrderAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            //await using var _db = new OrderAppDbContext(_dbContext);
            _dbContext.OrderHeaders.Add(orderHeader);
            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
