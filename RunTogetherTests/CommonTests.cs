using System;
using Xunit;
using Bunit;

namespace RunTogetherTests
{
    public class CommonTests : TestContext
    {
        [Fact]
        // Make sure tests are working
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
