using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using CfxTestTool.Core;
using CfxTestTool.Core.Messaging;
using CfxTestTool.Core.Serialization;
using CfxTestTool.Core.Services;

namespace CfxTestTool.Wpf.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly JsonSerializerOptions _jsonOptions = CfxJsonOptions.CreateDefault();
    private readonly ISettingsService _settingsService;
    private readonly IMessageTypeProvider _messageTypeProvider;
    private readonly ICfxSender _sender;
    private readonly ICfxReceiver _receiver;

    private CfxSettings _settings;
    private string? _selectedMessageType;
    private string _selectedMessageJson = string.Empty;

    public MainWindowViewModel()
    {
        _settingsService = new FileSettingsService();
        _messageTypeProvider = new ReflectionMessageTypeProvider();
        _settings = new CfxSettings();
        _sender = new CfxSender(_settings);
        _receiver = new CfxReceiver(_settings);
        _receiver.MessageReceived += ReceiverOnMessageReceived;

        MessageTypes = new ObservableCollection<string>();
        Logs = new ObservableCollection<LogEntryViewModel>();

        LoadMessageTypes();

        LoadSettingsCommand = new AsyncRelayCommand(LoadSettingsAsync);
        SaveSettingsCommand = new AsyncRelayCommand(SaveSettingsAsync);
        SendCommand = new AsyncRelayCommand(SendAsync, () => !string.IsNullOrWhiteSpace(SelectedMessageJson));
        StartReceiveCommand = new AsyncRelayCommand(StartReceiveAsync);
        StopReceiveCommand = new AsyncRelayCommand(StopReceiveAsync);
    }

    public ObservableCollection<string> MessageTypes { get; }

    public ObservableCollection<LogEntryViewModel> Logs { get; }

    public ICommand LoadSettingsCommand { get; }

    public ICommand SaveSettingsCommand { get; }

    public ICommand SendCommand { get; }

    public ICommand StartReceiveCommand { get; }

    public ICommand StopReceiveCommand { get; }

    public CfxSettings Settings
    {
        get => _settings;
        set
        {
            _settings = value;
            RaisePropertyChanged();
        }
    }

    public string? SelectedMessageType
    {
        get => _selectedMessageType;
        set
        {
            if (_selectedMessageType == value)
            {
                return;
            }

            _selectedMessageType = value;
            RaisePropertyChanged();
            UpdateTemplateForSelection();
        }
    }

    public string SelectedMessageJson
    {
        get => _selectedMessageJson;
        set
        {
            _selectedMessageJson = value;
            RaisePropertyChanged();
        }
    }

    private void LoadMessageTypes()
    {
        MessageTypes.Clear();
        foreach (var type in _messageTypeProvider.GetAllMessageTypes())
        {
            MessageTypes.Add(type.FullName ?? type.Name);
        }

        SelectedMessageType = MessageTypes.FirstOrDefault();
    }

    private async Task LoadSettingsAsync()
    {
        Settings = await _settingsService.LoadAsync().ConfigureAwait(false);
        AddLog("Settings loaded");
    }

    private async Task SaveSettingsAsync()
    {
        await _settingsService.SaveAsync(Settings).ConfigureAwait(false);
        AddLog("Settings saved");
    }

    private void UpdateTemplateForSelection()
    {
        if (string.IsNullOrWhiteSpace(SelectedMessageType))
        {
            SelectedMessageJson = string.Empty;
            return;
        }

        try
        {
            SelectedMessageJson = _messageTypeProvider.GenerateTemplateJson(SelectedMessageType);
        }
        catch (Exception ex)
        {
            SelectedMessageJson = $"// Failed to generate template: {ex.Message}";
        }
    }

    private async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedMessageType))
        {
            return;
        }

        var type = _messageTypeProvider.GetAllMessageTypes()
            .First(t => string.Equals(t.FullName, SelectedMessageType, StringComparison.Ordinal));

        var message = JsonSerializer.Deserialize(SelectedMessageJson, type, _jsonOptions);
        if (message == null)
        {
            AddLog("Cannot send message: JSON is invalid");
            return;
        }

        await _sender.SendAsync(message).ConfigureAwait(false);
        AddLog($"Sent {SelectedMessageType}");
    }

    private async Task StartReceiveAsync()
    {
        await _receiver.StartAsync().ConfigureAwait(false);
        AddLog("Receiver started");
    }

    private async Task StopReceiveAsync()
    {
        await _receiver.StopAsync().ConfigureAwait(false);
        AddLog("Receiver stopped");
    }

    private void ReceiverOnMessageReceived(object? sender, CfxReceivedMessageEventArgs e)
    {
        if (Application.Current?.Dispatcher != null)
        {
            Application.Current.Dispatcher.Invoke(() => AddLog($"Received on {e.RoutingKey}: {e.PayloadJson}"));
        }
        else
        {
            AddLog($"Received on {e.RoutingKey}: {e.PayloadJson}");
        }
    }

    private void AddLog(string message)
    {
        Logs.Add(new LogEntryViewModel(DateTime.Now, message));
    }
}
