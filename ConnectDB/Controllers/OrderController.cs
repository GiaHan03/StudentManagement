using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();

            return Ok(orders);
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound("Không tìm thấy order");

            return Ok(order);
        }

        // POST: tạo đơn hàng
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (order.OrderDetails == null || !order.OrderDetails.Any())
                return BadRequest("Order phải có sản phẩm");

            // check customer
            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer == null)
                return BadRequest("Customer không tồn tại");

            decimal total = 0;

            foreach (var item in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return BadRequest($"Product {item.ProductId} không tồn tại");

                var inventory = await _context.Inventories.FindAsync(item.ProductId);
                if (inventory == null)
                    return BadRequest($"Chưa có tồn kho cho product {item.ProductId}");

                // ❗ check tồn kho
                if (inventory.SoLuongTon < item.SoLuong)
                    return BadRequest($"Không đủ hàng cho product {item.ProductId}");

                item.Gia = product.Gia;
                total += item.Gia * item.SoLuong;

                // ✅ trừ kho đúng
                inventory.SoLuongTon -= item.SoLuong;
                inventory.NgayCapNhat = DateTime.Now;
            }

            order.TongTien = total;
            order.NgayBan = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound("Không tìm thấy order");

            // ❗ hoàn kho khi xóa
            foreach (var item in order.OrderDetails)
            {
                var inventory = await _context.Inventories.FindAsync(item.ProductId);
                if (inventory != null)
                {
                    inventory.SoLuongTon += item.SoLuong;
                    inventory.NgayCapNhat = DateTime.Now;
                }
            }

            _context.OrderDetails.RemoveRange(order.OrderDetails);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return Ok("Xóa order thành công");
        }
    }
}