using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Services;
using App.Areas.Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Admin.User
{
    [Area("Admin")]
    //  [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<MyUserRole> _userManager;
        public IndexModel(UserManager<MyUserRole> userManager)
        {
            _userManager = userManager ;

        }
        [TempData]
        public string StatusMessage{set;get;}

        public class UserAndRole : MyUserRole
        {
            public string RoleUser{set;get;}

        }

        public List<UserAndRole> user {set;get;}

        public async Task  OnGet()
        {
            var qr =  _userManager.Users.OrderBy(p => p.UserName);
             var qr1 =  qr.Select(u => new UserAndRole()
            {
                Id = u.Id,
                UserName = u.UserName
            });
            user = await qr1.ToListAsync();
            
            foreach (var users in user)
            {
                var RoleNames = await _userManager.GetRolesAsync(users);
                users.RoleUser = string.Join('.', RoleNames); 

            }
         
        }
        public void OnPost() => RedirectToPage();
    }
}
