# CfxTestTool

A developer-focused WPF utility for exploring and exercising IPC CFX messages. It discovers CFX message types from the official SDK, generates JSON templates, and sends/receives messages over RabbitMQ.

> This tool is for lab/development use only.

## Features
- Discover CFX message types via reflection
- Generate JSON templates for selected message types
- Edit/send CFX messages to RabbitMQ
- Start/stop receiving messages from RabbitMQ
- WPF UI with JSON editor, settings panel, and log view
- GitVersion-based semantic versioning
- GitHub Actions CI/CD that builds, tests, and publishes a single-file win-x64 executable

## Solution Structure
- `CfxTestTool.Core` – settings, message discovery/template generation, RabbitMQ sender/receiver (no UI deps)
- `CfxTestTool.Wpf` – WPF MVVM frontend
- `CfxTestTool.Tests` – xUnit tests for the core library
- `.github/workflows/dotnet.yml` – CI/CD pipeline
- `GitVersion.yml` – GitVersion configuration

## Getting Started
### Prerequisites
- .NET 8 SDK
- RabbitMQ broker reachable from the app
- Windows for running the WPF app

### Restore & Build
```bash
dotnet restore
dotnet build -c Release
```

### Run the WPF App
```bash
dotnet run --project CfxTestTool.Wpf -c Release
```

### Tests
```bash
dotnet test
```

## CI/CD
The GitHub Actions workflow:
1. Checks out the repository
2. Installs .NET 8
3. Runs GitVersion to compute semantic version
4. Restores, builds, and tests the solution (Release)
5. Publishes the WPF app as a self-contained single-file `win-x64` executable
6. Zips the publish directory and uploads it as an artifact

## Versioning
GitVersion Continuous Deployment mode drives semantic versions per branch configuration in `GitVersion.yml`.

## Settings
`CfxSettings` captures AMQP connection details:
- `AmqpUri`
- `Exchange`
- `RoutingKey`
- `Queue`

The default `FileSettingsService` persists settings to `cfxsettings.json` next to the executable.

## Notes
- CFX messages are discovered from the official CFX .NET SDK via reflection.
- Templates are serialized using `System.Text.Json` with indented JSON and string enums.
- RabbitMQ is accessed through `RabbitMQ.Client`.
