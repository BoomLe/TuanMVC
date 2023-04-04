using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.Identity.Models
{
    public class MyUserRole : IdentityUser
    {
        [Column(TypeName ="nvarchar")]
        [StringLength(200)]
        public string? HomeAddress{set;get;}

        [DataType(DataType.Date)]
        public DateTime? BrithDay{set;get;}
        
    }
}