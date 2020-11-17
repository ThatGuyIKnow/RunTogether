using Bunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RunTogether;
using RunTogether.Areas.Identity;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RunTogetherTests
{
    public class RunnerPageTests : TestContext
    {
        private TestContext ctx;
        private ApplicationDbContext dbContext;
        //private UserManager<ApplicationUser> userManager;
        private Stage activeStage = new Stage();
        private StageAssignment activeRunner = new StageAssignment();
        private Run assignedRun = new Run();
        private string qrCode;
        private string cameraCSS;
        private string startRunCSS;

        public RunnerPageTests()
        {
            ctx = new TestContext();
            ctx.Services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("testDB"); //skapa denna databas!
            });

            dbContext = ctx.Services.GetService<ApplicationDbContext>();
        }

        [Fact]
        public void CheckCode_QrCorrect_ShouldHideCameraAndShowStartRun()
        {

        }
    }
}
