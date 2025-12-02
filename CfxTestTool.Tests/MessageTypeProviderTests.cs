using CfxTestTool.Core.Messaging;
using Xunit;

namespace CfxTestTool.Tests;

public class MessageTypeProviderTests
{
    [Fact]
    public void GetAllMessageTypes_ReturnsValues()
    {
        var provider = new ReflectionMessageTypeProvider();
        var types = provider.GetAllMessageTypes();
        Assert.NotEmpty(types);
    }

    [Fact]
    public void GenerateTemplateJson_ReturnsJson()
    {
        var provider = new ReflectionMessageTypeProvider();
        var type = provider.GetAllMessageTypes().First();

        var json = provider.GenerateTemplateJson(type.FullName!);

        Assert.False(string.IsNullOrWhiteSpace(json));
        Assert.Contains("{", json);
    }
}
