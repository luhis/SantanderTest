using SantanderTest.Dtos;

namespace SantanderTest.Clients;

public interface IHackerNewsClient
{
    public Task<IEnumerable<int>> GetBestStories(CancellationToken cancellationToken = default);

    public Task<HackerNewsStoryDto> GetIndividualStory(int id, CancellationToken cancellationToken = default);
}
