using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/inventory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Inventories
                .Include(i => i.Product)
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/inventory/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.ProductId == id);

            if (inventory == null)
                return NotFound("Không tìm thấy inventory");

            return Ok(inventory);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            // check product tồn tại
            var product = await _context.Products.FindAsync(inventory.ProductId);
            if (product == null)
                return BadRequest("Product không tồn tại");

            // ❗ tránh trùng
            var exist = await _context.Inventories.FindAsync(inventory.ProductId);
            if (exist != null)
                return BadRequest("Inventory đã tồn tại");

            inventory.NgayCapNhat = DateTime.Now;

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return Ok(inventory);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Inventory inventory)
        {
            if (id != inventory.ProductId)
                return BadRequest("ID không khớp");

            var existing = await _context.Inventories.FindAsync(id);
            if (existing == null)
                return NotFound("Inventory không tồn tại");

            // ✅ chỉ update field cần thiết
            existing.SoLuongTon = inventory.SoLuongTon;
            existing.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
                return NotFound("Không tìm thấy inventory");

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}