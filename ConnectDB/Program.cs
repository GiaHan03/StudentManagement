using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
namespace ConnectDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cấu hình Port từ biến môi trường của Render (mặc định 10000 nếu chạy local)
            var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
            builder.WebHost.UseUrls($"http://*:{port}");

            //Đăng ký SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Tự động tạo cơ sở dữ liệu SQLite nếu chưa tồn tại
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Bật Swagger trên tất cả môi trường để tiện cho việc kiểm thử API trên Render
            app.UseSwagger();
            app.UseSwaggerUI();

            // Chuyển hướng trang chủ sang giao diện Swagger
            app.MapGet("/", () => Results.Redirect("/swagger"));

            // app.UseHttpsRedirection(); // ĐÃ TẮT: Tránh lỗi Redirect loop trên Render do Render đã tự động lo phần HTTPS.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}