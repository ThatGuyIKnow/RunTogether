using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Data;
using RunTogether.Data;

namespace RunTogether.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class OrganiserRegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrganiserRegisterModel> _logger;

        public OrganiserRegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext,
            ILogger<OrganiserRegisterModel> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string OrganiserKey { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string organiserKey = null)
        {
            OrganiserKey = organiserKey;
        }

        public async Task<IActionResult> OnPostAsync(string organiserKey = null)
        {
            organiserKey = organiserKey ?? Url.Content("~/");

            if (ModelState.IsValid && (await IsValidKey(organiserKey)).Succeeded)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName };
                var resultCreate = await _userManager.CreateAsync(user, Input.Password);
                var resultRole = await _userManager.AddToRoleAsync(user, IdentityRoleTypes.Organiser);

                if (resultCreate.Succeeded && resultRole.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    OrganiserCreationKey key = await _dbContext.OrganiserCreationKeys.FindAsync(organiserKey);
                    _dbContext.OrganiserCreationKeys.Remove(key);
                    await _dbContext.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    return LocalRedirect("/organiser");
                }
                foreach (var error in resultCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<IdentityResult> IsValidKey(string organiserKey)
        {
            try {
                OrganiserCreationKey key = await _dbContext.OrganiserCreationKeys.FirstAsync(o => o.Key == organiserKey);
                if (key.ExpirationDatetime.CompareTo(DateTime.Now) < 0)
                {
                    throw new Exception("Key Expired");
                }
            } catch (Exception e)
            {
                IdentityError err = new IdentityError(){Description = e.Message};
                IdentityResult result = IdentityResult.Failed(err);
                return result;
            }
            return IdentityResult.Success;
        }
    }
}
