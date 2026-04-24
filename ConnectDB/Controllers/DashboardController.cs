using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders.SumAsync(o => o.TongTien);
            var totalCustomers = await _context.Customers.CountAsync();
            var totalProducts = await _context.Products.CountAsync();

            // Lấy danh sách sản phẩm sắp hết hàng (tồn kho < 10)
            var lowStockItems = await _context.Inventories
                .Include(i => i.Product)
                .Where(i => i.SoLuongTon < 10)
                .Select(i => new {
                    i.Product.TenBanh,
                    i.SoLuongTon
                })
                .ToListAsync();

            // Top 5 sản phẩm bán chạy nhất
            var topSelling = await _context.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(od => od.SoLuong),
                    ProductName = _context.Products.Where(p => p.ProductId == g.Key).Select(p => p.TenBanh).FirstOrDefault()
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToListAsync();

            return Ok(new
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                LowStockItems = lowStockItems,
                TopSellingProducts = topSelling
            });
        }
    }
}
