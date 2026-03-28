using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Ten { get; set; }

        public string SoDienThoai { get; set; }

        public string DiaChi { get; set; }

        // Navigation
        public ICollection<Order> Orders { get; set; }
    }
}