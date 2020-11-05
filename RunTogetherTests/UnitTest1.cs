using System;
using Xunit;

namespace RunTogetherTests
{
    public class UnitTest1
    {
        [Fact]
        public void SanityCheck()
        {
            // Arrange
            int two = 2;

            // Act
            int result = two + two;

            // Assert
            Assert.Equal(4, result);
        }
    }
}
