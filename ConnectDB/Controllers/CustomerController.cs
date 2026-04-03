using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound("Không tìm thấy customer");

            return Ok(customer);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        // PUT (FIX)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            if (id != customer.CustomerId)
                return BadRequest("ID không khớp");

            var existing = await _context.Customers.FindAsync(id);
            if (existing == null)
                return NotFound("Customer không tồn tại");

            // ✅ update từng field (an toàn)
            existing.Ten = customer.Ten;
            existing.SoDienThoai = customer.SoDienThoai;
            existing.DiaChi = customer.DiaChi;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE (FIX)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound("Không tìm thấy customer");

            // ❗ check có order không
            if (customer.Orders != null && customer.Orders.Any())
                return BadRequest("Customer đã có đơn hàng, không thể xóa");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}