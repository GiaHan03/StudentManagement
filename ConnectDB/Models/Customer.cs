using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ConnectDB.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(100)]
        public string Ten { get; set; }

        [Phone]
        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        [StringLength(200)]
        public string? DiaChi { get; set; }

        // Navigation
        public ICollection<Order>? Orders { get; set; }
    }
}