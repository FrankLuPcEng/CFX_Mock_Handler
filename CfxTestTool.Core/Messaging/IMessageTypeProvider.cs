namespace CfxTestTool.Core.Messaging;

public interface IMessageTypeProvider
{
    IReadOnlyCollection<Type> GetAllMessageTypes();
    string GenerateTemplateJson(string typeName);
}
