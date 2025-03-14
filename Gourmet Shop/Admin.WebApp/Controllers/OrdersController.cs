using GourmetShop.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly GourmetShopDbContext _context;

        public OrdersController(GourmetShopDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexAsync()
        {
            
            var orders = await _context.Orders
                                       .AsNoTracking()
                                       .Include(o => o.UserInfo)
                                       .Include(o => o.OrderItems)
                                       .ThenInclude(oi => oi.Product)
                                       .ToListAsync();

            Console.WriteLine($"Orders count: {orders.Count}");
            return View(orders);
        }
    }
}
