using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RunTogether.Data;

namespace RunTogether.Areas.Identity.Helpers
{
    public class PlainTextPasswordHasher : PasswordHasher<ApplicationUser>
    {
        public override string HashPassword(ApplicationUser user, string password) { return password; }
    }

    public class UserCreationHelper : IDisposable
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private ApplicationDbContext _dbContext { get; }
        private IServiceScope _scope { get; }
        

        public UserCreationHelper(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();

            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _dbContext = _scope.ServiceProvider.GetService<ApplicationDbContext>();
        }

        public async Task<IdentityResult> CreateRunner(string firstName, string lastName, string email, Run run)
        {
            Run? selectedRun = await _dbContext.Runs.FindAsync(run.ID);
            IdentityResult userInfoResult = await ValidateUserInformation(email, selectedRun);
            if (!userInfoResult.Succeeded) { return userInfoResult; }

            _userManager.PasswordHasher = new PlainTextPasswordHasher();

            int newRunnerId = run.GetNextRunnerId();
            string usernamePrefix = run.ID.ToString();

            ApplicationUser user = new ApplicationUser(usernamePrefix + "-" + email)
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                RunnerId = newRunnerId
            };
            IdentityResult result = await _userManager.CreateAsync(user, CreateRandomPassword(32));
            
            if(result.Succeeded)
            {
                selectedRun.Runners.Add(user);
                selectedRun.IncrementRunnerId();
                await _dbContext.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, IdentityRoleTypes.Runner);
            }

            _userManager.PasswordHasher = new PasswordHasher<ApplicationUser>();
            return result;
        }



        private async Task<IdentityResult> ValidateUserInformation(string email, Run? run)
        {
            string newNormEmail = _userManager.NormalizeEmail(email);
            bool isUsedEmail = run?.Runners.Any(runner => 
                runner.NormalizedEmail == newNormEmail) != null;
            List<IdentityError> errors = new List<IdentityError>();


            if (isUsedEmail)
            {
                IdentityError error = new IdentityError();
                error.Description = "Error creating runner: Email is already in use!";
                errors.Add(error);
            }

            if (run == null)
            {
                IdentityError error = new IdentityError();
                error.Description = "Error creating runner: Could not find run by ID!";
                errors.Add(error);
            }

            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
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

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
