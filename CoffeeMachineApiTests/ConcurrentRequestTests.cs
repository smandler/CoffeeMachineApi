using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CoffeeMachineApiTests
{
    public class ConcurrentRequestTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ConcurrentRequestTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task BrewCoffee_HandlesConcurrentRequestsCorrectly()
        {
            var tasks = new List<Task<HttpResponseMessage>>();

            // Simulate 100 concurrent requests
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(_client.GetAsync("/coffeeMachine/brew-coffee"));
            }

            var results = await Task.WhenAll(tasks);

            // Ensure responses are valid HTTP status codes
            foreach (var result in results)
            {
                Assert.Contains(result.StatusCode, new[] { HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable, (HttpStatusCode)418 });
            }
        }
    }

}

