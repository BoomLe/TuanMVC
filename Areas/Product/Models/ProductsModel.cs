using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.Identity.Models;

namespace App.Areas.Product.Models
{
    [Table("Product")]
    public class ProductsModel
    {
    [Key]
    public int ProductId {set; get;}

    [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
    [Display(Name = "Tiêu đề")]
    [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
    public string Title {set; get;}

    [Display(Name = "Mô tả ngắn")]
    public string? Description {set; get;}

    [Display(Name="Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
   
    [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
    [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
    public string? Slug {set; get;}

    [Display(Name = "Nội dung")]
    [Required(ErrorMessage ="Vui lòng nhập nội dung")]
    public string Content {set; get;}

    [Display(Name = "Xuất bản")]
    public bool Published {set; get;}

   


    [Display(Name = "Tác giả")]
    public string? AuthorId {set; get;}

// fix thử user.id
    [ForeignKey("AuthorId")]
    [Display(Name = "Tác giả")]
    public MyUserRole? Author {set; get;}

    [Display(Name = "Ngày tạo")]
    public DateTime? DateCreated {set; get;}

    [Display(Name = "Ngày cập nhật")]
    public DateTime? DateUpdated {set; get;}

    [Display(Name =  "Giá sản phẩm ")]
    public decimal Price {set;get;}
     public List<ProductInCategory>?  ProductInCategories { get; set; }

     public List<ProductPhoto>? Photos{set;get;}




    }
    
}