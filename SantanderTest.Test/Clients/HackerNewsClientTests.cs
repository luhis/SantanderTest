using AwesomeAssertions;
using Microsoft.Extensions.Caching.Memory;
using SantanderTest.Clients;
using SantanderTest.Dtos;
using System.Net;
using System.Net.Http.Headers;

namespace SantanderTest.Test.Clients;

public class HackerNewsClientTests
{
    private static IHackerNewsClient CreateClient(
       HttpMessageHandler handler,
       IMemoryCache? cache = null)
    {
        var factory = new StubHttpClientFactory(() => new HttpClient(handler));
        return new HackerNewsClient(
            factory,
            cache ?? new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async Task GetBestStories_Success()
    {
        var json = """[1]"""u8.ToArray();
        var handler = new StubHttpHandler(json, "application/json");

        var client = CreateClient(handler);
        var firstResult = await client.GetBestStories();
        var secondResult = await client.GetBestStories();

        firstResult.Should().BeEquivalentTo(new[] { 1 });
        firstResult.Should().BeSameAs(secondResult);
        handler.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetBestStories_Error()
    {
        var json = """[1]"""u8.ToArray();
        var handler = new StubHttpHandler(json, "application/json", HttpStatusCode.InternalServerError);

        var client = CreateClient(handler);
        var act = () => client.GetBestStories();

        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Response status code does not indicate success: 500 (Internal Server Error).");
        handler.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetIndividualStory_Success()
    {
        var json = """
            {
              "by": "ismaildonmez",
              "descendants": 588,
              "id": 21233041,
              "kids": [21233229, 21233577, 21235077, 21233633, 21233159, 21233523, 21233181, 21233289, 21233377, 21233166, 21233206, 21233208, 21233255, 21234044, 21233896, 21233310, 21233141, 21233760, 21233999, 21235018, 21233232, 21233654, 21233381, 21233122, 21233374, 21237776, 21233580, 21234174, 21233332, 21235834, 21235067, 21233542, 21235125, 21233213, 21238388, 21234665, 21233727, 21235822, 21240691, 21233734, 21233138, 21237367, 21234121, 21234429, 21234089, 21233271, 21234699, 21234294, 21233302, 21234992, 21234519, 21233329, 21234722, 21234730, 21234796, 21233371, 21235229, 21235101, 21233351, 21235325, 21234625, 21238572, 21234957, 21233509, 21234596, 21233359, 21233909, 21237619, 21233635, 21235112, 21251217, 21233357, 21235371, 21233939, 21233484, 21233578],
              "score": 1757,
              "time": 1570887781,
              "title": "A uBlock Origin update was rejected from the Chrome Web Store",
              "type": "story",
              "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745"
            }
            """u8.ToArray();
        var handler = new StubHttpHandler(json, "application/json");

        var client = CreateClient(handler);
        var firstResult = await client.GetIndividualStory(21233041);
        var secondResult = await client.GetIndividualStory(21233041);

        firstResult.Should().BeEquivalentTo(new HackerNewsStoryDto
        {
            By = "ismaildonmez",
            Descendants = 588,
            Time = 1570887781,
            Title = "A uBlock Origin update was rejected from the Chrome Web Store",
            Url = "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
            Score = 1757
        });
        firstResult.Should().BeSameAs(secondResult);
        handler.CallCount.Should().Be(1);
    }


    [Fact]
    public async Task GetIndividualStory_Error()
    {
        var json = """
            {
            }
            """u8.ToArray();
        var handler = new StubHttpHandler(json, "application/json", HttpStatusCode.InternalServerError);

        var client = CreateClient(handler);
        var act = () => client.GetIndividualStory(21233041);

        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Response status code does not indicate success: 500 (Internal Server Error).");
        handler.CallCount.Should().Be(1);
    }
}

public sealed class StubHttpHandler(byte[] body, string contentType, HttpStatusCode statusCode = HttpStatusCode.OK)
    : HttpMessageHandler
{
    public int CallCount { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CallCount++;
        var content = new ByteArrayContent(body);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
        return Task.FromResult(new HttpResponseMessage(statusCode) { Content = content });
    }
}

public sealed class StubHttpClientFactory(Func<HttpClient> client) : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => client();
}
