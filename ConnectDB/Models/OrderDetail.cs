using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        // FK
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}