using Microsoft.Extensions.Caching.Memory;
using SantanderTest.Dtos;

namespace SantanderTest.Clients;

public class HackerNewsClient(IHttpClientFactory httpClientFactory, IMemoryCache cache) : IHackerNewsClient
{
    private const string BaseUrl = "https://hacker-news.firebaseio.com/v0";
    private readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    public async Task<IEnumerable<int>> GetBestStories(CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{nameof(IHackerNewsClient.GetBestStories)}";
        if (cache.TryGetValue(cacheKey, out IEnumerable<int>? cached) && cached is not null)
            return cached;

        using var client = httpClientFactory.CreateClient();
        using var response = await client.GetAsync($"{BaseUrl}/beststories.json", cancellationToken);
        response.EnsureSuccessStatusCode();

        var storyIds = await response.Content.ReadFromJsonAsync<IEnumerable<int>>(cancellationToken);
        cache.Set(cacheKey, storyIds, CacheDuration);
        return storyIds ?? throw new Exception("Failed to retrieve best stories.");
    }

    public async Task<HackerNewsStoryDto> GetIndividualStory(int id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{nameof(IHackerNewsClient.GetIndividualStory)}_{id}";
        if (cache.TryGetValue(cacheKey, out HackerNewsStoryDto? cached) && cached is not null)
            return cached;

        using var client = httpClientFactory.CreateClient();
        using var response = await client.GetAsync($"{BaseUrl}/item/{id}.json", cancellationToken);
        response.EnsureSuccessStatusCode();

        var story = await response.Content.ReadFromJsonAsync<HackerNewsStoryDto>(cancellationToken);
        cache.Set(cacheKey, story, CacheDuration);
        return story ?? throw new Exception($"Failed to retrieve individual story {id}.");
    }
}
