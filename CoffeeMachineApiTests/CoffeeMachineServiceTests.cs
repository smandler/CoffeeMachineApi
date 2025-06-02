using Moq;
using CoffeeMachineApi.Services;
using Microsoft.Extensions.Logging;

namespace CoffeeMachineApi.Tests
{
    /// <summary>
    /// Unit tests for CoffeeMachineService.
    /// Ensures correct responses for various scenarios.
    /// </summary>
    public class CoffeeMachineServiceTests
    {
        /// <summary>
        /// Test that the CoffeeMachineService returns a normal CoffeeMachine response (200 OK).
        /// </summary>
        [Fact]
        public void BrewCoffee_ReturnsReadyCoffeeMachine_Normally()
        {
            // Arrange - Mock dependencies
            var mockFactory = new Mock<ICoffeeMachineResponseFactory>();
            var mockDateProvider = new Mock<IDateProvider>();
            var mockLogger = new Mock<ILogger<CoffeeMachineService>>();

            // Mock the factory to always return "Ready" response
            mockFactory.Setup(f => f.GetResponse(It.IsAny<int>(), It.IsAny<DateTime>()))
                       .Returns((200, new { message = "Your piping hot coffee is ready", prepared = DateTime.UtcNow.ToString("o") }));

            var service = new CoffeeMachineService(mockFactory.Object, mockDateProvider.Object, mockLogger.Object);
            var result = service.BrewCoffee();

            // Assert - Validate response
            Assert.Equal(200, result.statusCode);
            Assert.NotNull(result.response);
        }

        /// <summary>
        /// Test that the CoffeeMachineService returns 418 "I'm a teapot" on April 1st.
        /// </summary>
        [Fact]
        public void BrewCoffee_ReturnsTeapot_OnAprilFirst()
        {
            // Arrange - Mock dependencies
            var mockFactory = new Mock<ICoffeeMachineResponseFactory>();
            var mockLogger = new Mock<ILogger<CoffeeMachineService>>();
            // Arrange - Mock date provider to return April 1st
            var mockDateProvider = new Mock<IDateProvider>();
            mockDateProvider.Setup(dp => dp.UtcNow).Returns(new DateTime(2025, 4, 1, 12, 0, 0, DateTimeKind.Utc));

            // Mock the factory to return "Teapot" response on April 1st
            mockFactory.Setup(f => f.GetResponse(It.IsAny<int>(), It.Is<DateTime>(d => d.Month == 4 && d.Day == 1 && d.Kind == DateTimeKind.Utc)))
                       .Returns((418, null));

            var service = new CoffeeMachineService(mockFactory.Object, mockDateProvider.Object, mockLogger.Object);
            // Act - Call BrewCoffee
            var result = service.BrewCoffee();

            // Assert - Validate response
            Assert.Equal(418, result.statusCode);
            Assert.Null(result.response);
        }

        /// <summary>
        /// Test that the CoffeeMachineService returns 503 "Service Unavailable" every 5th request.
        /// </summary>
        [Fact]
        public void BrewCoffee_ReturnsOutOfCoffeeMachine_EveryFifthRequest()
        {
            // Arrange - Mock dependencies
            var mockFactory = new Mock<ICoffeeMachineResponseFactory>();
            var mockDateProvider = new Mock<IDateProvider>();
            var mockLogger = new Mock<ILogger<CoffeeMachineService>>();

            // Mock factory to return "Out of CoffeeMachine" every 5th request
            mockFactory.Setup(f => f.GetResponse(It.Is<int>(count => count % 5 == 0), It.IsAny<DateTime>()))
                       .Returns((503, null));

            var service = new CoffeeMachineService(mockFactory.Object, mockDateProvider.Object, mockLogger.Object);

            // Perform multiple requests to reach the 5th request
            for (int i = 0; i < 4; i++) service.BrewCoffee();
            var result = service.BrewCoffee(); // 5th request

            // Assert - Validate response
            Assert.Equal(503, result.statusCode);
            Assert.Null(result.response);
        }
    }
}
