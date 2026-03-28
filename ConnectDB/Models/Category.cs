using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục tối đa 100 ký tự")]
        public string TenDanhMuc { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}