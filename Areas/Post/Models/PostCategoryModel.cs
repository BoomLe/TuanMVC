using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.Blog.Models;

namespace App.Areas.Post.Models
{
    [Table("PostCategory")]
    public class PostCategoryModel
{
    public int PostID {set; get;}

    public int CategoryID {set; get;}

    [ForeignKey("PostID")]
    public PostModel? Post {set; get;}

    [ForeignKey("CategoryID")]
    public Category? Category {set; get;}
}
}