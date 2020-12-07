using System;
using Xunit;
using Bunit;
using RunTogether;
using System.Collections.Generic;
using RunTogether.Data;

namespace RunTogetherTests
{
    public class StageMethodsTests : TestContext
    {

        [Fact]
        public void GetPreviousStageCheck()
        {
            // Arrange
            RunRoute Route = new RunRoute()
            {
                Stages = new List<Stage>
                {
                },
            };

            Stage Stage1 = new Stage()
            {
                StartPoint = new StartPoint(1.1F, 1.2F),
                ThroughPoints = new List<ThroughPoint> { new ThroughPoint(2.1F, 2.2F), new ThroughPoint(3.1F, 3.2F) },
                EndPoint = new EndPoint(4.1F, 4.2F),
                RunRoute = Route
            };
            Stage Stage2 = new Stage()
            {
                StartPoint = new StartPoint(5.1F, 5.2F),
                ThroughPoints = new List<ThroughPoint> { new ThroughPoint(6.1F, 6.2F), new ThroughPoint(7.1F, 7.2F) },
                EndPoint = new EndPoint(8.1F, 8.2F),
                RunRoute = Route
            };
            Stage Stage3 = new Stage()
            {
                StartPoint = new StartPoint(9.1F, 9.2F),
                ThroughPoints = new List<ThroughPoint> { new ThroughPoint(10.1F, 10.2F), new ThroughPoint(11.1F, 11.2F) },
                EndPoint = new EndPoint(12.1F, 12.2F),
                RunRoute = Route
            };




            Route.Stages.Add(Stage1);
            Route.Stages.Add(Stage2);
            Route.Stages.Add(Stage3);



            // Act
            Stage result = Route.Stages[1].GetPreviousStage();
            Stage result2 = Route.Stages[0].GetPreviousStage();


            // Expected
            Stage expected = Stage1;
            Stage expected2 = Stage1;


            // Assert
            Assert.Equal(expected, result);
            Assert.Equal(expected2, result2);
        }

        [Fact]
        public void StageMethod_GetCurrentRunner_FindFirstActiveRunne()
        {
            Stage stage = new Stage()
            {
                StartPoint = new StartPoint(1.1F, 1.2F),
                EndPoint = new EndPoint(4.1F, 4.2F)
            };

            StageAssignment runner1 = new StageAssignment()
            {
                Order = 1,
                Status = RunningStatus.Active,
                Stage = stage
            };

            StageAssignment runner2 = new StageAssignment()
            {
                Order = 2,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            StageAssignment runner3 = new StageAssignment()
            {
                Order = 3,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            stage.AssignedRunners.Add(runner1);
            stage.AssignedRunners.Add(runner2);
            stage.AssignedRunners.Add(runner3);

            StageAssignment result = stage.GetCurrentRunner();
            StageAssignment expected = runner1;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void StageMethod_GetCurrentRunner_FindFirstNotActiveRunner()
        {
            Stage stage = new Stage()
            {
                StartPoint = new StartPoint(1.1F, 1.2F),
                EndPoint = new EndPoint(4.1F, 4.2F)
            };

            StageAssignment runner1 = new StageAssignment()
            {
                Order = 1,
                Status = RunningStatus.Completed,
                Stage = stage
            };

            StageAssignment runner2 = new StageAssignment()
            {
                Order = 2,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            StageAssignment runner3 = new StageAssignment()
            {
                Order = 3,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            stage.AssignedRunners.Add(runner1);
            stage.AssignedRunners.Add(runner2);
            stage.AssignedRunners.Add(runner3);

            StageAssignment result = stage.GetCurrentRunner();
            StageAssignment expected = runner2;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void StageMethod_GetLastRunner_FindLastRunner()
        {
            Stage stage = new Stage()
            {
                StartPoint = new StartPoint(1.1F, 1.2F),
                EndPoint = new EndPoint(4.1F, 4.2F)
            };

            StageAssignment runner1 = new StageAssignment()
            {
                Order = 1,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            StageAssignment runner2 = new StageAssignment()
            {
                Order = 2,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            StageAssignment runner3 = new StageAssignment()
            {
                Order = 3,
                Status = RunningStatus.NotStarted,
                Stage = stage
            };

            stage.AssignedRunners.Add(runner1);
            stage.AssignedRunners.Add(runner2);
            stage.AssignedRunners.Add(runner3);

            StageAssignment result = stage.GetLastRunner();
            StageAssignment expected = runner3;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void StageMethod_GetNextStage_FindNextStage()
        {
            RunRoute route = new RunRoute()
            {
                Stages = new List<Stage>
                {
                }
            };

            Stage stage1 = new Stage()
            {
                StartPoint = new StartPoint(1.1F, 1.2F),
                EndPoint = new EndPoint(4.1F, 4.2F),
                RunRoute = route
            };

            Stage stage2 = new Stage()
            {
                StartPoint = new StartPoint(5.1F, 5.2F),
                EndPoint = new EndPoint(8.1F, 8.2F),
                RunRoute = route
            };

            route.Stages.Add(stage1);
            route.Stages.Add(stage2);

            Stage result = stage1.GetNextStage();
            Stage expected = stage2;

            Assert.Equal(expected, result);
        }
    }


}
