using AwesomeAssertions;

namespace SantanderTest.Test.Dtos;

public class StoryDtoTests
{
    [Fact]
    public void ShouldDeserialise()
    {
        // from the task description, the JSON is as follows:
        var json = """
        {
            "title": "A uBlock Origin update was rejected from the Chrome Web Store",
            "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
            "postedBy": "ismaildonmez",
            "time": "2019-10-12T13:43:01+00:00",
            "score": 1716,
            "commentCount": 572
        }
        """;

        var story = System.Text.Json.JsonSerializer.Deserialize<SantanderTest.Dtos.StoryDto>(json, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        story.Should().NotBeNull();
        story.Should().BeEquivalentTo(new SantanderTest.Dtos.StoryDto
        {
            Title = "A uBlock Origin update was rejected from the Chrome Web Store",
            Uri = "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
            PostedBy = "ismaildonmez",
            Time = new DateTimeOffset(new DateOnly(2019, 10, 12), new TimeOnly(13, 43, 01), TimeSpan.Zero),
            Score = 1716,
            CommentCount = 572
        });
    }

    [Fact]
    public void ShouldSerialise()
    {
        var model = new SantanderTest.Dtos.StoryDto
        {
            Title = "A uBlock Origin update was rejected from the Chrome Web Store",
            Uri = "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
            PostedBy = "ismaildonmez",
            Time = new DateTimeOffset(new DateOnly(2019, 10, 12), new TimeOnly(13, 43, 01), TimeSpan.Zero),
            Score = 1716,
            CommentCount = 572
        };

        var json = """
        {"title":"A uBlock Origin update was rejected from the Chrome Web Store","uri":"https://github.com/uBlockOrigin/uBlock-issues/issues/745","postedBy":"ismaildonmez","time":"2019-10-12T13:43:01+00:00","score":1716,"commentCount":572}
        """;

        var story = System.Text.Json.JsonSerializer.Serialize(model, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        story.Should().NotBeNull();
        story.Should().BeEquivalentTo(json);
    }
}
