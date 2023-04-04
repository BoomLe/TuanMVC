using System.ComponentModel.DataAnnotations;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Role
{
    public class EditModel : PageModelALL
    {
        public EditModel(RoleManager<IdentityRole> roleManage, MyDbContext context) : base(roleManage, context)
        {
        }
           public class InputModel
        {
            [Required(ErrorMessage ="Vui lòng nhập tên")]
            [Display(Name ="Nhập Tên")]
            [StringLength(50, MinimumLength =3, ErrorMessage ="{0} từ {2} đến {1} ký tự")]
            public string Name{set;get;}

        }
        [BindProperty]
        public InputModel Input{set;get;}

        [BindProperty]
        public IdentityRole role{set;get;}
         public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy thông tin");

            role = await _roleManager.FindByIdAsync(roleid);

            if(role != null)
            {
                Input = new InputModel(){
                    Name = role.Name
                };
                return Page();
            }
            return NotFound("Không tìm thấy thông tin");
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy thông tin");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null) return NotFound("Không tìm thấy thông tin");
            if(!ModelState.IsValid)
            {
                return Page();

            }
            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                StatusMessage = $"Bạn đã chỉnh sửa {Input.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>{
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }

            return Page();

        }
    }
}
