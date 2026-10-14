using SantanderTest.Clients;
using SantanderTest.Dtos;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHackerNewsClient, HackerNewsClient>();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/stories/best", async (IHackerNewsClient hackerNewsClient, int limit, CancellationToken ct) =>
{
    if (limit is < 1 or > 500)
        return Results.BadRequest("Query parameter 'limit' must be between 1 and 500.");

    var bestStories = await hackerNewsClient.GetBestStories(ct);
    var topStories = bestStories.Take(limit);

    var stories = await Task.WhenAll(topStories.Select(id => hackerNewsClient.GetIndividualStory(id, ct)));

    // belt and braces: sort the stories by score in descending order before returning
    return Results.Ok(stories.Select(MapToStoryDto).OrderByDescending(s => s.Score));
})
.WithName("GetBestStories");

static StoryDto MapToStoryDto(HackerNewsStoryDto story)
    => new StoryDto
    {
        Title = story.Title,
        Uri = story.Url,
        PostedBy = story.By,
        Time = DateTimeOffset.FromUnixTimeSeconds(story.Time),
        Score = story.Score,
        CommentCount = story.Descendants
    };

app.Run();
