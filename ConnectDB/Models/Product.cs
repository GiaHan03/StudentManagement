using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ConnectDB.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên bánh không được để trống")]
        [StringLength(150, ErrorMessage = "Tên bánh tối đa 150 ký tự")]
        public string TenBanh { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal Gia { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int SoLuong { get; set; }

        // ✅ Foreign key (QUAN TRỌNG cho API)
        [Required(ErrorMessage = "CategoryId là bắt buộc")]
        public int CategoryId { get; set; }

        // ✅ Navigation (nullable để tránh lỗi binding)
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [JsonIgnore]
        public Inventory? Inventory { get; set; }
    }
}