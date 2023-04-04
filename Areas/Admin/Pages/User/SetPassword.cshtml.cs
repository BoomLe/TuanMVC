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

namespace App.Admin.User
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<MyUserRole> _userManager;
        private readonly SignInManager<MyUserRole> _signInManager;

        public SetPasswordModel(
            UserManager<MyUserRole> userManager,
            SignInManager<MyUserRole> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = " {0} phải {2} đến tối đa {1} ký tự .", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string NewPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Lặp lại mật khẩu")]
            [Compare("NewPassword", ErrorMessage = "Mật khẩu lặp lại không đúng.")]
            public string ConfirmPassword { get; set; }
        }
   
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


            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userManager.RemovePasswordAsync(user);
            
            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
           
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

           
           StatusMessage = $"Đặt lại mật khẩu thành công {user.UserName}";

            return RedirectToPage("./Index");
             
        }
    }
}
