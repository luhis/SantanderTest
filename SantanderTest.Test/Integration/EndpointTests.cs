using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SantanderTest.Clients;
using SantanderTest.Dtos;
using System.Net.Http.Json;

namespace SantanderTest.Test.Integration;

public class EndpointTests
{
    private static HttpClient CreateClient(Action<Mock<IHackerNewsClient>> configure, out MockRepository mockRepo)
    {
        mockRepo = new MockRepository(MockBehavior.Strict);
        var mockApiClient = mockRepo.Create<IHackerNewsClient>();
        configure(mockApiClient);

        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHackerNewsClient));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddSingleton(mockApiClient.Object);
            }));

        return factory.CreateClient();
    }

    [Fact]
    public async Task App_StartsUp_Returns200ForOpenApiEndpoint()
    {
        var client = CreateClient(_ => { }, out var mockRepo);

        var response = await client.GetAsync("/openapi/v1.json");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        mockRepo.VerifyAll();
    }

    [Fact]
    public async Task App_StartsUp_Returns200ForGetBestEndpoint()
    {
        var client = CreateClient(client => {
            client.Setup(a => a.GetBestStories(It.IsAny<CancellationToken>())).ReturnsAsync([1]);
            client.Setup(a => a.GetIndividualStory(1, It.IsAny<CancellationToken>())).ReturnsAsync(new HackerNewsStoryDto() { By = "ismaildonmez", Title = "A uBlock Origin update was rejected from the Chrome Web Store", Url = "https://github.com/uBlockOrigin/uBlock-issues/issues/745", Time = 1570887781, Score = 1757, Descendants = 588 });
        }, out var mockRepo);

        var response = await client.GetAsync("/stories/best?limit=1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var story = await response.Content.ReadFromJsonAsync<IEnumerable<StoryDto>>();
        story.Should().BeEquivalentTo([new StoryDto() { CommentCount = 588, PostedBy = "ismaildonmez", Title = "A uBlock Origin update was rejected from the Chrome Web Store", Uri = "https://github.com/uBlockOrigin/uBlock-issues/issues/745", Time = new DateTimeOffset(new DateOnly(2019, 10, 12), new TimeOnly(13, 43, 01), TimeSpan.Zero), Score = 1757 }]);

        mockRepo.VerifyAll();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(501)]
    public async Task App_StartsUp_Returns400ForInvalidBestStoryLimit(int limit)
    {
        var client = CreateClient(_ => { }, out var mockRepo);

        var response = await client.GetAsync($"/stories/best?limit={limit}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        mockRepo.VerifyAll();
    }
}
