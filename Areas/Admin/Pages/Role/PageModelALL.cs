using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin
{
    public class PageModelALL : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;

        protected readonly MyDbContext _context ;
        [TempData]
        public string StatusMessage{set;get;}

        public PageModelALL(RoleManager<IdentityRole> roleManage,MyDbContext context)
        {
            _roleManager = roleManage;
            _context = context;
        }
    }
    
}