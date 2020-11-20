using System;
using Xunit;
using Bunit;
using RunTogether;
using System.Collections.Generic;
using RunTogether.Data;

namespace RunTogetherTests
{
    public class StageMethods : TestContext
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
    }


}
