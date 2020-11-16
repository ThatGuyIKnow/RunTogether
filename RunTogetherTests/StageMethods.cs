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
        public void StageToPointListCheck()
        {
            // Arrange
            RunRoute Route = new RunRoute()
            {
                Stages = new List<Stage>
                {
                    new Stage()
                    {
                        StartPoint = new StartPoint(1.1F, 1.2F),
                        ThroughPoints =  new List<ThroughPoint> { new ThroughPoint(2.1F,2.2F), new ThroughPoint(3.1F, 3.2F) },
                        EndPoint = new EndPoint(4.1F, 4.2F)
                    },
                    new Stage()
                    {
                        StartPoint = new StartPoint(5.1F, 5.2F),
                        ThroughPoints =  new List<ThroughPoint> { new ThroughPoint(6.1F,6.2F), new ThroughPoint(7.1F, 7.2F) },
                        EndPoint = new EndPoint(8.1F, 8.2F)
                    },
                    new Stage()
                    {
                        StartPoint = new StartPoint(9.1F, 9.2F),
                        ThroughPoints =  new List<ThroughPoint> { new ThroughPoint(10.1F,10.2F), new ThroughPoint(11.1F, 11.2F) },
                        EndPoint = new EndPoint(12.1F, 12.2F)
                    }

                },
            };

            // Act
            List<List<float>> result = Route.ToPointList();


            // Expected
            List<List<float>> expected = new List<List<float>>
            {
                new List<float> {1.1F, 1.2F },
                new List<float> {2.1F, 2.2F },
                new List<float> {3.1F, 3.2F },
                new List<float> {4.1F, 4.2F },
                new List<float> {5.1F, 5.2F },
                new List<float> {6.1F, 6.2F },
                new List<float> {7.1F, 7.2F },
                new List<float> {8.1F, 8.2F },
                new List<float> {9.1F, 9.2F },
                new List<float> {10.1F, 10.2F },
                new List<float> {11.1F, 11.2F },
                new List<float> {12.1F, 12.2F }
            };

            // Assert
            Assert.Equal(expected, result);
        }

    }
}
