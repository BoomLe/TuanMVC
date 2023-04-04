using System.ComponentModel.DataAnnotations;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Services;
namespace App.Admin.Role
{
    public class CreatedModel : PageModelALL
    {
        public CreatedModel(RoleManager<IdentityRole> roleManage, MyDbContext context) : base(roleManage, context)
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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roleName = new IdentityRole(Input.Name);
            var createdRole = await _roleManager.CreateAsync(roleName);

            if(createdRole.Succeeded)
            {
                StatusMessage = "Bạn đã tạo tài khoản cấp quyền thành công !";
                return RedirectToPage("./Index");

            }
            else
            {
                createdRole.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });

            }
            return Page();

        }
    }
}
