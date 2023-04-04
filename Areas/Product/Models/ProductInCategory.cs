using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using App.Areas.Identity.Models;

namespace App.Areas.Product.Models
{
    [Table("ProductInCategory")]
    public class ProductInCategory
{
    public int ProductID {set; get;}

    public int CategoryID {set; get;}

    [ForeignKey("ProductID")]
    public ProductsModel? Products {set; get;}

    [ForeignKey("CategoryID")]
    public CategoryProduct? Category {set; get;}


}
}