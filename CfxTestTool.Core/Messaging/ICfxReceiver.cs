namespace CfxTestTool.Core.Messaging;

public interface ICfxReceiver
{
    event EventHandler<CfxReceivedMessageEventArgs>? MessageReceived;
    Task StartAsync();
    Task StopAsync();
}
