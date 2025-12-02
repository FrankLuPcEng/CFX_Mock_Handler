using System.Text.Json;
using CFX;
using CfxTestTool.Core.Serialization;

namespace CfxTestTool.Core.Messaging;

public class ReflectionMessageTypeProvider : IMessageTypeProvider
{
    private readonly JsonSerializerOptions _jsonOptions = CfxJsonOptions.CreateDefault();
    private IReadOnlyCollection<Type>? _cached;

    public IReadOnlyCollection<Type> GetAllMessageTypes()
    {
        if (_cached != null)
        {
            return _cached;
        }

        var baseType = typeof(CFXMessage);
        _cached = baseType.Assembly
            .GetTypes()
            .Where(t => baseType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .OrderBy(t => t.FullName)
            .ToList();

        return _cached;
    }

    public string GenerateTemplateJson(string typeName)
    {
        var type = GetAllMessageTypes()
            .FirstOrDefault(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal)
                                 || string.Equals(t.Name, typeName, StringComparison.Ordinal));

        if (type == null)
        {
            throw new ArgumentException($"Message type '{typeName}' not found", nameof(typeName));
        }

        var instance = Activator.CreateInstance(type)
                      ?? throw new InvalidOperationException($"Cannot create instance of {type.FullName}");

        return JsonSerializer.Serialize(instance, type, _jsonOptions);
    }
}
