namespace CfxTestTool.Core.Messaging;

public class CfxReceivedMessageEventArgs : EventArgs
{
    public CfxReceivedMessageEventArgs(string payloadJson, string routingKey)
    {
        PayloadJson = payloadJson;
        RoutingKey = routingKey;
        ReceivedAtUtc = DateTime.UtcNow;
    }

    public string PayloadJson { get; }
    public string RoutingKey { get; }
    public DateTime ReceivedAtUtc { get; }
}
