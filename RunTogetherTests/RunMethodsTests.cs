using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using RunTogether;
using RunTogether.Data;

namespace RunTogetherTests
{
    public class RunMethodsTests
    {
        [Fact]
        public void RunMethods_GetCurrentStage_FindActiveStage()
        {
            Run run = new Run();
            RunRoute route = new RunRoute()
            {
                Stages = new List<Stage>
                {
                },
            };

            Stage stage1 = new Stage()
            {
                StartPoint = new StartPoint(1.1F, 1.2F),
                EndPoint = new EndPoint(4.1F, 4.2F),
                Status = RunningStatus.Active
            };

            Stage stage2 = new Stage()
            {
                StartPoint = new StartPoint(2.1F, 1.2F),
                EndPoint = new EndPoint(5.1F, 4.2F),
                Status = RunningStatus.NotStarted
            };

            route.Stages.Add(stage1);
            route.Stages.Add(stage2);
            run.Route = route;

            Stage result = run.GetCurrentStage();
            Stage expected = stage1;

            Assert.Equal(expected, result);
        }
    }
}
