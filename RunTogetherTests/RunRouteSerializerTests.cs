using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RunTogether;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Helpers;
using RunTogether.Data;
using Xunit;

namespace RunTogetherTests
{
    public class RunRouteSerializerTests : TestContext, IDisposable
    {
        private TestContext ctx;
        private ApplicationDbContext dbContext;

        public RunRouteSerializerTests()
        {
            ctx = new TestContext();
            ctx.Services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("testDB");
            });

            dbContext = ctx.Services.GetService<ApplicationDbContext>();
        }

        public override void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            ctx?.Dispose();
            base.Dispose();
        }

        [Fact]
        public async void RunRouteToJsonSerializable_ValidRouteSerialize_FormattedCorrectly()
        {
            Run run = new Run(){Name = "RunTogetherTestRun"};
            StartPoint p1 = new StartPoint(0F, 1F);
            EndPoint p2 = new EndPoint(2F, 3F);
            Stage stage = new Stage(){Date = DateTime.Now, StartPoint = p1, EndPoint = p2};
            RunRoute route = new RunRoute(){Run = run, Stages = new List<Stage>(){{stage}}};
            stage.RunRoute = route;
            await dbContext.Runs.AddAsync(run);
            await dbContext.Stages.AddAsync(stage);
            await dbContext.RunRoutes.AddAsync(route);
            await dbContext.SaveChangesAsync();

            var data = route.ToJsonSerializableViewer();

            Assert.NotEmpty((IEnumerable)data["Stages"]);
        }
    }
}
