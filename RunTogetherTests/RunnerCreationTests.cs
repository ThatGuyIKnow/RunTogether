using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Xunit.Sdk;

namespace RunTogetherTests
{
    public class RunnerCreationTests : TestContext, IDisposable
    {
        private TestContext ctx;
        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> userManager;
        private UserCreationHelper helper;

        public RunnerCreationTests()
        {
            ctx = new TestContext();
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

            ctx.Services.AddTransient<UserCreationHelper>();

            dbContext = ctx.Services.GetService<ApplicationDbContext>();
            userManager = ctx.Services.GetService<UserManager<ApplicationUser>>();
            helper = ctx.Services.GetService<UserCreationHelper>();
        }

        public override void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            userManager?.Dispose();
            helper?.Dispose();
            ctx?.Dispose();
            base.Dispose();
        }

        [Fact]
        public async void CreateRunner_ValidInformation_Created()
        {
            Run run = new Run();
            var updatedRun = await dbContext.Runs.AddAsync(run);
            await dbContext.SaveChangesAsync();

            await helper.CreateRunner("Tommy", "Dada", "bobo@baba.com", updatedRun.Entity);

            var user = await userManager.FindByEmailAsync("bobo@baba.com");
            Assert.NotNull(user);
        }

        [Fact]
        public async void CreateRunner_InvalidEmail_NotCreated()
        {
            Run run = new Run();
            var updatedRun = await dbContext.Runs.AddAsync(run);
            await dbContext.SaveChangesAsync();

            await helper.CreateRunner("Tommy", "Dada", "bobobaba.com", updatedRun.Entity);

            var user = await userManager.FindByEmailAsync("bobobaba.com");
            Assert.Null(user);
        }

        [Fact]
        public async void CreateRunner_RunDoesNOTExistsInDatabase_NotCreated()
        {
            Run run = new Run();
            run.ID = 9999;

            await helper.CreateRunner("Tommy", "Dada", "bobo@baba.com", run);

            var user = await userManager.FindByEmailAsync("bobo@baba.com");
            Assert.Null(user);
        }

        [Fact]
        public async void CreateRunner_DuplicateEmail_NotCreated()
        {
            Run run = new Run();
            var updatedRun = await dbContext.Runs.AddAsync(run);
            await dbContext.SaveChangesAsync();

            await helper.CreateRunner("Christy", "Lala", "bobo@baba.com", updatedRun.Entity);
            await helper.CreateRunner("Tommy", "Dada", "bobo@baba.com", updatedRun.Entity);
            bool exceptionCaught = false;
            try
            {
                // Throws if multiple of the same email is found
                await userManager.FindByEmailAsync("bobo@baba.com");
            }
            catch(Exception e) { exceptionCaught = true; }

            Assert.False(exceptionCaught);
        }

        [Fact]
        public async void CreateRunner_UniquePassword_NotIdentical()
        {
            Run run = new Run();
            var updatedRun = await dbContext.Runs.AddAsync(run);
            await dbContext.SaveChangesAsync();

            await helper.CreateRunner("Christy", "Lala", "nana@baba.com", updatedRun.Entity);
            await helper.CreateRunner("Tommy", "Dada", "bobo@baba.com", updatedRun.Entity);

            var user1 = await userManager.FindByEmailAsync("nana@baba.com");
            var user2 = await userManager.FindByEmailAsync("bobo@baba.com");

            Assert.NotEqual(user1.PasswordHash, user2.PasswordHash);
        }
    }
}
