using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RunTogether.Data;

namespace RunTogether.Areas.Identity
{
    public static class IdentitySeedInitializer
    {
            public static void SeedUsers(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
            if (userManager.FindByEmailAsync("abc@xyz.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = "abc@xyz.com",
                        Email = "abc@xyz.com",
                        FirstName = "Jonas",
                        LastName = "Jørgensen",
                        RunnerId = 0
                    };

                    IdentityResult result = userManager.CreateAsync(user, "password").Result;

                    if (result.Succeeded)
                    {
                        List<string> roles = new List<string>(){IdentityRoleTypes.Organiser, IdentityRoleTypes.SuperOrganiser};
                        userManager.AddToRolesAsync(user, roles).Wait();
                    }
                }
        }
    }
}
