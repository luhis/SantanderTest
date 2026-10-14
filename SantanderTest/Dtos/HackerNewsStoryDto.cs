namespace SantanderTest.Dtos;

public class HackerNewsStoryDto
{
    public required string Title { get; init; }
    public string Url { get; init; } = string.Empty;
    public required string By { get; init; }
    public required int Time { get; init; }
    public required int Score { get; init; }
    public required int Descendants { get; init; }
}
