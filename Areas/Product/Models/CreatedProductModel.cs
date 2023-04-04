using System.ComponentModel;

namespace App.Areas.Product.Models
{
    public class CreatedProductModel : ProductsModel
    {
        [DisplayName("Chuyên mục sản phẩm")]
        public int[] ? CategoryIDs{set;get;}

    }
}