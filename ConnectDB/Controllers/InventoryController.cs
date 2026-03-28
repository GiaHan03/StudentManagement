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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Inventories
                .Include(i => i.Product)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Inventory inventory)
        {
            if (id != inventory.ProductId) return BadRequest();

            inventory.NgayCapNhat = DateTime.Now;

            _context.Entry(inventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(inventory);
        }
    }
}