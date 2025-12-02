using System.Text.Json;
using CfxTestTool.Core.Serialization;

namespace CfxTestTool.Core.Services;

public class FileSettingsService : ISettingsService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = CfxJsonOptions.CreateDefault();

    public FileSettingsService(string? filePath = null)
    {
        _filePath = string.IsNullOrWhiteSpace(filePath)
            ? Path.Combine(AppContext.BaseDirectory, "cfxsettings.json")
            : filePath;
    }

    public async Task<CfxSettings> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new CfxSettings();
        }

        await using var stream = File.OpenRead(_filePath);
        var settings = await JsonSerializer.DeserializeAsync<CfxSettings>(stream, _options);
        return settings ?? new CfxSettings();
    }

    public async Task SaveAsync(CfxSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? AppContext.BaseDirectory);
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, settings, _options);
    }
}
