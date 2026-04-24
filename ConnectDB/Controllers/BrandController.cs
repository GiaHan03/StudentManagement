using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/brand
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Brands.ToListAsync());
        }

        // GET: api/brand/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            return Ok(brand);
        }

        // POST: api/brand
        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return Ok(brand);
        }

        // PUT: api/brand/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Brand brand)
        {
            if (id != brand.BrandId) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(brand).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(brand);
        }

        // DELETE: api/brand/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
