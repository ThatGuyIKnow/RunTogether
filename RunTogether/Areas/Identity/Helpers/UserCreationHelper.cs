using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using RunTogether.Data;

namespace RunTogether.Areas.Identity.Helpers
{
    public class UserCreationHelper
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private ApplicationDbContext _dbContext { get; }

        public UserCreationHelper(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> CreateRunner(string firstName, string lastName, string email, Run run)
        {
            IdentityResult userInfoResult = await ValidateUserInformation(email, IdentityRoleTypes.Runner, run);
            if (!userInfoResult.Succeeded) { return userInfoResult; }
            
            int newRunnerId = run.GetNextRunnerId();
            string usernamePrefix = run.ID.ToString();

            ApplicationUser user = new ApplicationUser(usernamePrefix + "-" + email)
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                RunnerId = newRunnerId
            };
            IdentityResult result = await _userManager.CreateAsync(user, CreateRandomPassword(12));
            
            if(result.Succeeded)
            {
                run.Runners.Add(user);
                run.IncrementRunnerId();
                //_dbContext.Runs.Find(run.ID).Runners.Add(user);
                //_dbContext.Runs.Find(run.ID).IncrementRunnerId();
                await _dbContext.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, IdentityRoleTypes.Runner);
            }

            return result;
        }


        public async Task<IdentityResult> CreateOrganiser(string firstName, string lastName, string email)
        {
            IdentityResult userInfoResult = await ValidateUserInformation(email, IdentityRoleTypes.Organiser);
            if (!userInfoResult.Succeeded) { return userInfoResult; }

            ApplicationUser user = new ApplicationUser(email)
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            }; 
            
            IdentityResult result = await _userManager.CreateAsync(user, CreateRandomPassword(12));

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, IdentityRoleTypes.Organiser);
            }

            return result;
        }

        //private async Task<IdentityResult> CreateUser(string firstName, string lastName, string email, string username)
        //{


        //    ApplicationUser user = new ApplicationUser(username)
        //    {
        //        Email = email, FirstName = firstName, LastName = lastName
        //    };

        //    return await _userManager.CreateAsync(user, CreateRandomPassword(12));

        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(user, role);
        //    }

        //    return result;
        //}

        private async Task<IdentityResult> ValidateUserInformation(string email, string role, Run? run = null)
        {
            bool isUsedEmail = false;
            string newNormEmail = _userManager.NormalizeEmail(email);

            if (role == IdentityRoleTypes.Runner)
            {
                isUsedEmail = run.Runners.Any(runner => runner.NormalizedEmail == newNormEmail);
            }
            else if (role == IdentityRoleTypes.Organiser)
            {
                IList<ApplicationUser> organisers = await _userManager.GetUsersInRoleAsync(role);
                isUsedEmail = organisers.Any(org => org.NormalizedEmail == newNormEmail);
            }

            if (isUsedEmail)
            {
                IdentityError error = new IdentityError();
                error.Description = "Error creating user: Email is already in use!";
                return IdentityResult.Failed(error);
            }

            return IdentityResult.Success;
        }


        private string CreateRandomPassword(int length)
        {
            string[] allowed_characters = new[]{"a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                "A","B","C","D","E","F","G", "H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y",
                "Z","1","2","3","4","5","6","7","8","9","0", ".", ","};
            string result = "";

            // Not cryptographically secure, but good enough for this
            Random rand = new Random(Environment.TickCount);

            for (int i = 0; i < length; i++)
            {
                int r = rand.Next(62);
                result += allowed_characters[r];
            }

            return result;
        }
    }
}
