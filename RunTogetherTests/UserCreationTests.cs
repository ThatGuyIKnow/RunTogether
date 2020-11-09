using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using Bunit;
using Bunit.Extensions;
using Bunit.Extensions.WaitForHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RunTogether;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Helpers;
using RunTogether.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace RunTogetherTests
{
    public class UserCreationTests : TestContext
    {
        private void SetupContext(TestContext ctx)
        {
            ctx.Services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("testDB");
            });
            ctx.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            ctx.Services.AddSingleton<UserManager<ApplicationUser>>();
            ctx.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

        }

        [Fact]
        public async void CreateRunner_ValidInformation_Created()
        {
            using var ctx = new TestContext();
            SetupContext(ctx);
            ctx.Services.AddTransient<UserCreationHelper>();
            var helper = ctx.Services.GetService<UserCreationHelper>();
            var dbContext = ctx.Services.GetService<ApplicationDbContext>();
            var userManager = ctx.Services.GetService<UserManager<ApplicationUser>>();
            Run run = new Run();
            var updatedRun = await dbContext.Runs.AddAsync(run);
            await dbContext.SaveChangesAsync();

            await helper.CreateRunner("Tommy", "Dada", "bobo@baba.com", updatedRun.Entity);

            var user = await userManager.FindByEmailAsync("bobo@baba.com");
            Assert.Equal("Tommy", user.FirstName);
        }

        [Fact]
        public async void CreateRunner_InvalidEmail_NotCreated()
        {
            using var ctx = new TestContext();
            SetupContext(ctx);
            ctx.Services.AddTransient<UserCreationHelper>();
            var helper = ctx.Services.GetService<UserCreationHelper>();
            var dbContext = ctx.Services.GetService<ApplicationDbContext>();
            var userManager = ctx.Services.GetService<UserManager<ApplicationUser>>();
            Run run = new Run();
            var updatedRun = await dbContext.Runs.AddAsync(run);
            await dbContext.SaveChangesAsync();

            await helper.CreateRunner("Tommy", "Dada", "bobobaba.com", updatedRun.Entity);

            var user = await userManager.FindByEmailAsync("bobobaba.com");
            Assert.Null(user);
        }
        
        public void CreateRunner_RunExistsInDatabase_Created()
        {

        }
        public void CreateRunner_RunDoesNOTExistsInDatabase_NotCreated()
        {

        }

        public void CreateRunner_DuplicateEmail_NotCreated()
        {

        }
        public void CreateRunner_UniquePassword_NotIdentical()
        {

        }
    }
}
