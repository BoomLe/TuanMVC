using System.Collections.Generic;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using App.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.User
{
    public class AddRoledModel : PageModel
    {
        private readonly UserManager<MyUserRole> _userManager;
        private readonly SignInManager<MyUserRole> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoledModel(
            UserManager<MyUserRole> userManager,
            SignInManager<MyUserRole> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
    
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        [BindProperty]
        [Display(Name ="Cấp quyền thành viên :")]
        public string[] RoleName{set;get;} 

        public SelectList AllRoles {set;get;}
   
        public MyUserRole user{set;get;}

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound ("Không tìm thấy tài khoản");
            }

            user = await _userManager.FindByIdAsync(id);


           
            if (user == null)
            {
                return NotFound($"Không tìm thấy tài khoản : '{id}'.");
            }

            RoleName = (await _userManager.GetRolesAsync(user)).ToArray<string>();

            List<string> RoleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            AllRoles = new SelectList(RoleNames);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
             if(string.IsNullOrEmpty(id))
            {
                return NotFound ("Không tìm thấy tài khoản");
            }

            user = await _userManager.FindByIdAsync(id);


           
            if (user == null)
            {
                return NotFound($"Không tìm thấy tài khoản : '{id}'.");
            }
            //AddRole

            var OldRoleName = (await _userManager.GetRolesAsync(user)).ToArray<string>();

            var DeleteRoleName = OldRoleName.Where(r => !RoleName.Contains(r));


            var AddRoleName =  RoleName.Where(r => !OldRoleName.Contains(r));

            List<string> RoleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            AllRoles = new SelectList(RoleNames);

            var DeleteRole = await _userManager.RemoveFromRolesAsync(user, DeleteRoleName);
            if(!DeleteRole.Succeeded)
            {
                DeleteRole.Errors.ToList().ForEach(error =>{
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();

            }

            var AddRole = await _userManager.AddToRolesAsync(user, AddRoleName);
            if(!AddRole.Succeeded)
            {
                AddRole.Errors.ToList().ForEach(error => 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();

            }

            return RedirectToPage("./Index");


         
        }
    }
}
