using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.Identity.Models;

namespace App.Areas.Post.Models
{
    [Table("Post")]
public class PostModel : PostBaseModel
{

    
    [Display(Name = "Tác giả")]
    public string? AuthorId {set; get;}

    [ForeignKey("AuthorId")]
    [Display(Name = "Tác giả")]
    public MyUserRole? Author {set; get;}

    [Display(Name = "Ngày tạo")]
    public DateTime? DateCreated {set; get;}

    [Display(Name = "Ngày cập nhật")]
    public DateTime? DateUpdated {set; get;}
}
    
}