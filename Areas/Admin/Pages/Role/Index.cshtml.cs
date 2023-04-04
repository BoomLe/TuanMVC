using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Services;
using Microsoft.AspNetCore.Authorization;

namespace App.Admin.Role
{
    [Area("Admin")]
    // [Authorize(Roles ="Admin")]
    public class IndexModel : PageModelALL
    {
        public IndexModel(RoleManager<IdentityRole> roleManage, MyDbContext context) : base(roleManage, context)
        {
        }

        public List<IdentityRole> roles {set;get;}

        public async Task  OnGet()
        {
            roles = await _roleManager.Roles.OrderBy(p => p.Name).ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
