using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            return Ok(product);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // check category tồn tại
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return BadRequest("Category không tồn tại");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest("Id không khớp");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound("Không tìm thấy sản phẩm");

            // check category
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return BadRequest("Category không tồn tại");

            // update field
            existingProduct.TenBanh = product.TenBanh;
            existingProduct.Gia = product.Gia;
            existingProduct.SoLuong = product.SoLuong;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }

        // DELETE: api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}