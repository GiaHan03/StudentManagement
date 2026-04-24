using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Employees.AnyAsync(e => e.Username == request.Username))
            {
                return BadRequest("Username đã tồn tại.");
            }

            var employee = new Employee
            {
                Username = request.Username,
                Password = request.Password, // Lưu ý: Thực tế nên hash password
                FullName = request.FullName,
                EmployeeCode = request.EmployeeCode,
                Phone = request.Phone,
                Address = request.Address,
                Position = request.Position,
                Birthday = request.Birthday
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!", username = employee.Username });
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Username == request.Username && e.Password == request.Password);

            if (employee == null)
            {
                return Unauthorized("Username hoặc Password không đúng.");
            }

            // Trả về thông tin nhân viên (không bao gồm password)
            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = new
                {
                    employee.Id,
                    employee.Username,
                    employee.FullName,
                    employee.Position,
                    employee.EmployeeCode
                }
            });
        }

        // PUT: api/account/update-profile
        [HttpPut("update-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, Employee updateData)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            employee.FullName = updateData.FullName;
            employee.Phone = updateData.Phone;
            employee.Address = updateData.Address;
            
            if (!string.IsNullOrEmpty(updateData.Password))
            {
                employee.Password = updateData.Password;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật hồ sơ thành công!", user = employee });
        }
    }
}
