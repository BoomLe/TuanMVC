using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.Product.Models
{
    [Table("ProductPhoto")]
    public class ProductPhoto
    {
        [Key]
        public int Id{set;get;}

        public string FileName{set;get;}

        public int ProductID{set;get;}

        [ForeignKey("ProductID")]
        public ProductsModel? Product{set;get;}
    }
}