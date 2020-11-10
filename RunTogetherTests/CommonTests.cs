using System;
using Xunit;
using Bunit;

namespace RunTogetherTests
{
    public class CommonTests : TestContext
    {
        // Make sure tests are working
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
