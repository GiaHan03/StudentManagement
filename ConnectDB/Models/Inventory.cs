using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectDB.Models
{
    public class Inventory
    {
        [Key]
        public int ProductId { get; set; }

        public int SoLuongTon { get; set; }

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}