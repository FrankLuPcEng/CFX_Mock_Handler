using CfxTestTool.Core.Serialization;

namespace CfxTestTool.Core.Services;

public interface ISettingsService
{
    Task<CfxSettings> LoadAsync();
    Task SaveAsync(CfxSettings settings);
}
