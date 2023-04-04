using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.Contacts.Models
{
    public class ContactModel
    {
        [Key]
        public int Id{set;get;}

        [DisplayName("Họ và Tên")]
        [Required(ErrorMessage ="Vui lòng nhập {0}")]
        [StringLength(100)]
        [Column(TypeName ="nvarchar")]
        public string FullName{set;get;}

        [DisplayName("Địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage ="Vui lòng nhập {0}")]
        public string Email{set;get;}

       [DataType(DataType.DateTime)]
        public DateTime SendDate{set;get;}

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage ="{0} không đúng dịnh dạng")]
        public string? PhoneNumber{set;get;}

        [DisplayName("Nội dung")]
        public string? Message{set;get;}
        
    }
}