using System.ComponentModel;

namespace App.Areas.Post.Models
{
    public class CreatedPostModel : PostModel
    {
        [DisplayName("Chuyên mục Blog")]
        public int[] ? CategoryIDs{set;get;}

    }
}