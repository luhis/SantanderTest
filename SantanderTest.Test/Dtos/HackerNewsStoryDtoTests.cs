using AwesomeAssertions;

namespace SantanderTest.Test.Dtos;

public class HackerNewsStoryDtoTests
{
    [Fact]
    public void ShouldDeserialise()
    {
        // from https://hacker-news.firebaseio.com/v0/item/21233041.json
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
        """;

        var story = System.Text.Json.JsonSerializer.Deserialize<SantanderTest.Dtos.HackerNewsStoryDto>(json, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        story.Should().NotBeNull();
        story.Should().BeEquivalentTo(new SantanderTest.Dtos.HackerNewsStoryDto
        {
            Title = "A uBlock Origin update was rejected from the Chrome Web Store",
            Url = "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
            By = "ismaildonmez",
            Time = 1570887781,
            Score = 1757,
            Descendants = 588
        });
    }

    [Fact]
    public void ShouldDeserialiseOddExample()
    {
        // from https://hacker-news.firebaseio.com/v0/item/21233041.json
        var json = """
        {
          "by": "vishnukool",
          "descendants": 119,
          "id": 50059096,
          "kids": [50060422, 50085671, 50060249, 50060717, 50071898, 50068295, 50069352, 50065483, 50063940, 50067623, 50063391, 50060240, 50061261, 50060144, 50070586, 50064051, 50076725, 50067580, 50073959, 50066903, 50064192, 50066866, 50060514, 50068270, 50062588, 50070894, 50068675, 50065037, 50065802, 50065149, 50060033, 50059903, 50067961, 50064023, 50064886, 50065960, 50083486, 50066993, 50063837, 50070631, 50061240, 50059919, 50060163, 50089075, 50070298, 50068956, 50067360, 50065139],
          "score": 160,
          "text": "Hey HN, I’m Vishnu from Agent37 (\u003Ca href=\"https:&#x2F;&#x2F;agent37.com\"\u003Ehttps:&#x2F;&#x2F;agent37.com\u003C/a\u003E). We build sandboxes for running AI agents at under 10% of the cost of most sandbox providers. Our customers spend an average of $0.37&#x2F;month per sandbox.\u003Cp\u003EYou can view a video walkthrough of the product here: \u003Ca href=\"https:&#x2F;&#x2F;youtu.be&#x2F;DK5ISR2tbNo\" rel=\"nofollow\"\u003Ehttps:&#x2F;&#x2F;youtu.be&#x2F;DK5ISR2tbNo\u003C/a\u003E\u003Cp\u003EWe started as one of those OpenClaw wrapper companies back in February (yup.. judge me). In time we added more and more harnesses.\u003Cp\u003EOne of the goals I had at the time was to offer a free plan to users, so as to gain more market share. But it was too cost prohibitive to do it right away. Especially since we were bootstrapped and servers were running on my savings.\u003Cp\u003EToday we’re actually able to offer the free plan thanks to the optimizations we’ve made since. Over 90% of the new signups we have are just free plan users and they host their personal Hermes&#x2F;Openclaw&#x2F;Claude Code etc on our cloud. Our infrastructure cost for the free sandboxes averages around $0.05 to $0.10 per user per month.\u003Cp\u003EHere are some techniques that have worked for lowering cost:\u003Cp\u003E* When you look at agent harnesses, they have certain patterns in the way they consume CPU and RAM. They need CPU in bursts when they’re initializing or performing certain tasks, but beyond that, they’re often not peaking. They need more RAM for things like browser and computer-use tasks. Once the task is done, usage often comes back down. This is a key insight in lowering cost, so by efficiently managing the CPU&#x2F;RAM utilization we could lower the cost.\u003Cp\u003E* Secondly, you have to skip the hyperscalers when renting the Hosts. I worked at Amazon before and they have 80-90% margins on VMs. So it was an obvious choice once I looked at the prices to go one layer down to vendors like Hetzner and OVH. This alone was probably one of the biggest cost levers in reducing the cost.\u003Cp\u003E* Thirdly, you also want to pick a technology stack that lets you efficiently cache and share resources across the host. We use gVisor for better utilization and fast boot times. It lets us use available host memory across sandboxes without reserving a full VM’s memory for each one. There are tradeoffs: our gVisor sandboxes don’t support workloads like Docker-in-Docker or Android emulators.\u003Cp\u003E* And finally the size and shape of the hosts you buy also makes a big difference. RAM is typically the bottleneck when it comes to cost and not the CPU. This is specifically true for agent harness workloads, so when buying servers, we optimize for packing more RAM into a given host.\u003Cp\u003EWe also host full VM sandboxes for workloads that need them, including running Docker at a slightly higher price, but they are still pretty low cost given the rest of the techniques.\u003Cp\u003EOverall, my learning has been that just by focusing on a niche of sandbox usecases, and optimizing for it you can give a much better cost&#x2F;performance tradeoff.\u003Cp\u003EI was previously a Senior Software Engineer at Amazon and an Engineering Manager at Quantcast. I built Amazon&#x27;s internal AI chatbot, Diya, and their internal coding harness, Ziya.\u003Cp\u003EIf you’re someone who’s looking to run agents on sandbox and are exploring a sandbox vendor, come check us out. We’re particularly looking for developers building messaging agents to try their workloads and challenge the limits and economics of our platform.\u003Cp\u003EThanks!\u003Cp\u003E\u003Ca href=\"https:&#x2F;&#x2F;agent37.com\"\u003Ehttps:&#x2F;&#x2F;agent37.com\u003C/a\u003E",
          "time": 1791823594,
          "title": "Launch HN: Agent37 (YC F26) – Low-cost agent sandboxes for running agents",
          "type": "story"
        }
        """;

        var story = System.Text.Json.JsonSerializer.Deserialize<SantanderTest.Dtos.HackerNewsStoryDto>(json, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        story.Should().NotBeNull();
        story.Should().BeEquivalentTo(new SantanderTest.Dtos.HackerNewsStoryDto
        {
            Title = "Launch HN: Agent37 (YC F26) – Low-cost agent sandboxes for running agents",
            Url = "",
            By = "vishnukool",
            Time = 1791823594,
            Score = 160,
            Descendants = 119
        });
    }
}
