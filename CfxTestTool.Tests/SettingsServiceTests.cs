using CfxTestTool.Core;
using CfxTestTool.Core.Services;
using Xunit;

namespace CfxTestTool.Tests;

public class SettingsServiceTests
{
    [Fact]
    public async Task SaveAndLoad_RoundTripsValues()
    {
        var path = Path.Combine(Path.GetTempPath(), $"cfxsettings_{Guid.NewGuid():N}.json");
        var service = new FileSettingsService(path);
        var original = new CfxSettings
        {
            AmqpUri = "amqp://user:pass@server:5672/vhost",
            Exchange = "exchangeA",
            RoutingKey = "routeA",
            Queue = "queueA"
        };

        await service.SaveAsync(original);
        var loaded = await service.LoadAsync();

        Assert.Equal(original.AmqpUri, loaded.AmqpUri);
        Assert.Equal(original.Exchange, loaded.Exchange);
        Assert.Equal(original.RoutingKey, loaded.RoutingKey);
        Assert.Equal(original.Queue, loaded.Queue);
    }
}
