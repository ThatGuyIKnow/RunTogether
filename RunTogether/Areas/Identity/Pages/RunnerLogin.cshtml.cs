using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace RunTogether.Areas.Identity.Pages
{
    [AllowAnonymous]
    public class RunnerLoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RunnerLoginModel> _logger;

        public RunnerLoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<RunnerLoginModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public string key { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string key = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }


            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await OnPostAsync(key);
        }

        public async Task<IActionResult> OnPostAsync(string key = null)
        {
            ApplicationUser user;
            try
            {
                user = (await _userManager.GetUsersInRoleAsync(IdentityRoleTypes.Runner))
                    .First(u => u.PasswordHash == key);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                Response.Redirect("/", false);
                return Page();
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            await _signInManager.SignInAsync(user, true);
            _logger.LogInformation("User logged in.");
            Response.Redirect("/runner", false);
            return LocalRedirect("/runner");
        }
    }
}
