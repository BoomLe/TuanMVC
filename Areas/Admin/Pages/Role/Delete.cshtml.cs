using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Role
{
    public class DeleteModel : PageModelALL
    {
        public DeleteModel(RoleManager<IdentityRole> roleManage, MyDbContext context) : base(roleManage, context)
        {
        }
          [BindProperty]
        public IdentityRole role{set;get;}

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid == null) return NotFound ("Không tìm thấy thông tin");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role ==null)
            {
                 return NotFound ("Không tìm thấy thông tin");
            }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if(roleid == null) return NotFound ("Không tìm thấy thông tin");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null ) return NotFound ("Không tìm thấy thông tin");



            var result = await _roleManager.DeleteAsync(role);
            if(result.Succeeded)
            {
                StatusMessage = "Xóa Quyền Truy Cập Thành Công !";
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
