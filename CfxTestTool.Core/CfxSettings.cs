namespace CfxTestTool.Core;

public class CfxSettings
{
    public string AmqpUri { get; set; } = "amqp://guest:guest@localhost:5672/";
    public string Exchange { get; set; } = "cfx";
    public string RoutingKey { get; set; } = "cfx.message";
    public string Queue { get; set; } = "cfx.queue";
}
