using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime NgayBan { get; set; } = DateTime.Now;

        public decimal TongTien { get; set; }

        // FK
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        // Navigation
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}