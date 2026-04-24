using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConnectDB.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Tên thương hiệu không được để trống")]
        [StringLength(100, ErrorMessage = "Tên thương hiệu tối đa 100 ký tự")]
        public string TenThuongHieu { get; set; } = string.Empty;

        public string? HinhAnh { get; set; }

        public string? MoTa { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
