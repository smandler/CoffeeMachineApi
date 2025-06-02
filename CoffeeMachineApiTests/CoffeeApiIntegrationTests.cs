using CoffeeMachineApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CoffeeMachineApiTests
{
    public class CoffeeApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CoffeeApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task BrewCoffee_ReturnsServiceUnavailable_EveryFifthRequest()
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < 5; i++)
            {
                response = await _client.GetAsync("/coffeeMachine/brew-coffee");
            }

            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task BrewCoffee_ReturnsOk_WithCorrectJsonStructure()
        {
            var response = await _client.GetAsync("/coffeeMachine/brew-coffee");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);

            Assert.True(json.TryGetProperty("message", out _));
            Assert.True(json.TryGetProperty("prepared", out _));
        }

        [Fact]
        public async Task BrewCoffee_ReturnsTeapot_OnAprilFirst()
        {
            // Arrange: Create a temporary WebApplicationFactory with mock DateProvider
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var mockDateProvider = new Mock<IDateProvider>();
                    mockDateProvider.Setup(dp => dp.UtcNow).Returns(new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc));

                    // Temporarily replace IDateProvider for this test only
                    services.AddSingleton<IDateProvider>(mockDateProvider.Object);
                });
            });

            var client = factory.CreateClient();

            // Act: Call API
            var response = await client.GetAsync("/coffeeMachine/brew-coffee");

            // Assert: Ensure response is 418 (I'm a Teapot)
            Assert.Equal((HttpStatusCode)418, response.StatusCode);
        }
    }

}




