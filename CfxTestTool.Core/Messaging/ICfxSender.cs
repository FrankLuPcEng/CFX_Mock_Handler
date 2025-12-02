namespace CfxTestTool.Core.Messaging;

public interface ICfxSender
{
    Task SendAsync(object message);
}
