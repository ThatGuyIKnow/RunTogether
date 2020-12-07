using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            IdentityResult userInfoResult = ValidateUserInformation(email, run);
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

            if (result.Succeeded)
            {
                try
                {
                    Run? selectedRun = await _dbContext.Runs.FindAsync(run.ID);
                    selectedRun.Runners.Add(user);
                    selectedRun.IncrementRunnerId();
                    await _dbContext.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, IdentityRoleTypes.Runner);
                }
                catch (Exception) { return IdentityResult.Failed(); }
            }

            _userManager.PasswordHasher = new PasswordHasher<ApplicationUser>();
            return result;
        }



        private IdentityResult ValidateUserInformation(string email, Run run)
        {
            const string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            string newNormEmail = _userManager.NormalizeEmail(email);
            bool isValidEmail = Regex.IsMatch(newNormEmail, pattern, RegexOptions.IgnoreCase);
            bool isUsedEmail = run.Runners.Any(runner =>
                    runner.NormalizedEmail == newNormEmail);
            bool runExists = _dbContext.Runs.Any(r => r.ID == run.ID);
            List<IdentityError> errors = new List<IdentityError>();


            if (!isValidEmail)
            {
                IdentityError error = new IdentityError();
                error.Description = "Error creating runner: Email is invalid!";
                errors.Add(error);
            }

            if (isUsedEmail)
            {
                IdentityError error = new IdentityError();
                error.Description = "Error creating runner: Email is already in use!";
                errors.Add(error);
            }

            if (!runExists)
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
            string result;
            int randOffset = 0;
            do
            {
                result = CreateRandomString(length, randOffset);
                randOffset++;
            } while (_userManager.Users.Any(u => u.PasswordHash == result));
            return result;
        }

        private string CreateRandomString(int length, int offset)
        {
            string[] allowedCharacters = new[]{"a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                "A","B","C","D","E","F","G", "H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y",
                "Z","1","2","3","4","5","6","7","8","9","0", ".", ","};
            string result = "";

            // Not cryptographically secure, but good enough for this
            Random rand = new Random(Environment.TickCount + offset);

            for (int i = 0; i < length; i++)
            {
                int r = rand.Next(62);
                result += allowedCharacters[r];
            }

            return result;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
