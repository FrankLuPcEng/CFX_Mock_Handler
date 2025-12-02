# CFX_Mock_Handler

A developer-oriented **CFX Protocol Test Tool** for .NET with a WPF UI.

It helps you:
- Discover CFX message types
- Generate JSON templates
- Send and receive CFX messages via RabbitMQ
- Inspect messages in a UI
- Build artifacts with auto versioning

> For development/lab usage, not production.


## Features

- CFX Message Type Discovery
- JSON Template Generation
- Send CFX Messages
- Receive and Inspect Messages
- WPF UI (MVVM)
- GitVersion Auto Versioning
- GitHub Actions CI/CD
  - Build, test, package WPF exe


## Solution Structure

CfxTestTool/
CfxTestTool.Core/
CfxTestTool.Wpf/
CfxTestTool.Tests/
.github/workflows/dotnet.yml
GitVersion.yml
README.md
CfxTestTool.sln

markdown
複製程式碼

## Requirements

- Windows 10 or later (x64)
- .NET 8 SDK
- RabbitMQ broker
- CFX .NET SDK (NuGet)
- Git (for versioning)


## Build & Run

### Build

```bash
dotnet restore
dotnet build -c Release
Run WPF
bash
複製程式碼
dotnet run --project CfxTestTool.Wpf -c Release
CI/CD
This repo includes a GitHub Actions workflow:

Auto compute version with GitVersion

Build solution

Run tests

Publish WPF self-contained executable

Upload artifact

Versioning
Uses GitVersion to generate semantic version numbers based on git history.

You can configure behavior in GitVersion.yml.

Roadmap
CLI frontend

Scenario scripting

Multiple CFX SDK versions

Advanced validation

yaml
複製程式碼
