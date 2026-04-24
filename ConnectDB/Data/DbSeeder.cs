using ConnectDB.Models;
using Microsoft.EntityFrameworkCore;

namespace ConnectDB.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { TenDanhMuc = "Bánh Ngọt", HinhAnh = "https://images.unsplash.com/photo-1550617931-e17a7b70dce2?q=80&w=2070&auto=format&fit=crop" },
                    new Category { TenDanhMuc = "Bánh Mặn", HinhAnh = "https://images.unsplash.com/photo-1509440159596-0249088772ff?q=80&w=2072&auto=format&fit=crop" },
                    new Category { TenDanhMuc = "Bánh Kem", HinhAnh = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?q=80&w=2089&auto=format&fit=crop" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Seed Brands
            if (!context.Brands.Any())
            {
                var brands = new List<Brand>
                {
                    new Brand { TenThuongHieu = "ABC Bakery", MoTa = "Thương hiệu bánh hàng đầu Việt Nam" },
                    new Brand { TenThuongHieu = "Givral", MoTa = "Phong cách bánh Pháp quyến rũ" },
                    new Brand { TenThuongHieu = "Kinh Đô", MoTa = "Hương vị truyền thống" }
                };
                context.Brands.AddRange(brands);
                context.SaveChanges();
            }

            // Seed Customers
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer { Ten = "Nguyễn Văn A", SoDienThoai = "0901234567", DiaChi = "123 Quận 1, TP.HCM" },
                    new Customer { Ten = "Trần Thị B", SoDienThoai = "0123456789", DiaChi = "456 Quận 3, TP.HCM" },
                    new Customer { Ten = "Lê Văn C", SoDienThoai = "0987654321", DiaChi = "789 Quận 7, TP.HCM" }
                };
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var catNgot = context.Categories.First(c => c.TenDanhMuc == "Bánh Ngọt");
                var catMan = context.Categories.First(c => c.TenDanhMuc == "Bánh Mặn");
                var catKem = context.Categories.First(c => c.TenDanhMuc == "Bánh Kem");

                var brandABC = context.Brands.First(b => b.TenThuongHieu == "ABC Bakery");
                var brandGivral = context.Brands.First(b => b.TenThuongHieu == "Givral");
                var brandKinhDo = context.Brands.First(b => b.TenThuongHieu == "Kinh Đô");

                var products = new List<Product>
                {
                    new Product 
                    { 
                        TenBanh = "Croissant Bơ Phá", 
                        Gia = 25000, 
                        SoLuong = 50, 
                        CategoryId = catNgot.CategoryId, 
                        BrandId = brandABC.BrandId,
                        MoTa = "Bánh sừng bò thơm ngậy mùi bơ",
                        HinhAnh = "https://images.unsplash.com/photo-1555507036-ab1f4038808a?q=80&w=1926&auto=format&fit=crop"
                    },
                    new Product 
                    { 
                        TenBanh = "Bánh Mì Chà Bông", 
                        Gia = 15000, 
                        SoLuong = 30, 
                        CategoryId = catMan.CategoryId, 
                        BrandId = brandKinhDo.BrandId,
                        MoTa = "Bánh mì mềm mịn với lớp chà bông đậm đà",
                        HinhAnh = "https://images.unsplash.com/photo-1509440159596-0249088772ff?q=80&w=2072&auto=format&fit=crop"
                    },
                    new Product 
                    { 
                        TenBanh = "Tiramisu", 
                        Gia = 45000, 
                        SoLuong = 20, 
                        CategoryId = catKem.CategoryId, 
                        BrandId = brandGivral.BrandId,
                        MoTa = "Hương vị cà phê hòa quyện cùng phô mai béo ngậy",
                        HinhAnh = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?q=80&w=1974&auto=format&fit=crop"
                    },
                    new Product 
                    { 
                        TenBanh = "Bánh Su Kem", 
                        Gia = 10000, 
                        SoLuong = 100, 
                        CategoryId = catNgot.CategoryId, 
                        BrandId = brandABC.BrandId,
                        MoTa = "Vỏ bánh giòn nhẹ với nhân kem tan chảy",
                        HinhAnh = "https://images.unsplash.com/photo-1621236304198-651a2133309f?q=80&w=2070&auto=format&fit=crop"
                    },
                    new Product 
                    { 
                        TenBanh = "Bánh Kem Dâu Tây", 
                        Gia = 350000, 
                        SoLuong = 5, 
                        CategoryId = catKem.CategoryId, 
                        BrandId = brandGivral.BrandId,
                        MoTa = "Bánh sinh nhật trang trí dâu tây tươi mát",
                        HinhAnh = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?q=80&w=1965&auto=format&fit=crop"
                    }
                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Seed Inventories for products that don't have one
            var allProducts = context.Products.ToList();
            foreach (var p in allProducts)
            {
                if (!context.Inventories.Any(i => i.ProductId == p.ProductId))
                {
                    context.Inventories.Add(new Inventory 
                    { 
                        ProductId = p.ProductId, 
                        SoLuongTon = p.SoLuong, 
                        NgayCapNhat = DateTime.Now 
                    });
                }
            }
            context.SaveChanges();
            
            // Seed Employees
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(new List<Employee>
                {
                    new Employee { 
                        EmployeeCode = "NV01", 
                        FullName = "Gia Hân", 
                        Username = "giahan", 
                        Password = "123", 
                        Phone = "0900000001", 
                        Address = "TP.HCM", 
                        Position = "Manager", 
                        Birthday = new DateTime(2000, 1, 1) 
                    },
                    new Employee { 
                        EmployeeCode = "NV02", 
                        FullName = "Minh Tuấn", 
                        Username = "tuanminh", 
                        Password = "123", 
                        Phone = "0900000002", 
                        Address = "Hà Nội", 
                        Position = "Staff", 
                        Birthday = new DateTime(1995, 5, 15) 
                    }
                });
                context.SaveChanges();
            }
        }
    }
}
