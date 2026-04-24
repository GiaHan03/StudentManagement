using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Order? Order { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}