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

---

## 📄 2. GitVersion.yml

> 放在：`GitVersion.yml`

```yaml
mode: ContinuousDeployment
branches:
  main:
    regex: main
    mode: ContinuousDeployment
ignore:
  sha: []
這是最精簡、最穩定的設定
會自動產生版本例如 1.0.0+build.5

📄 3. GitHub Actions workflow
放在：.github/workflows/dotnet.yml

yaml
複製程式碼
name: .NET Build

on:
  push:
    branches: [ "main", "master" ]
  pull_request:
    branches: [ "main", "master" ]

jobs:
  build:
    runs-on: windows-latest

    steps:
    - name: Checkout
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x

    - name: Setup GitVersion
      uses: gittools/actions/gitversion/setup@v0.11.1
      with:
        versionSpec: '5.x'

    - name: Run GitVersion
      id: gitversion
      uses: gittools/actions/gitversion/execute@v0.11.1

    - name: Print version
      run: echo "SemVer: ${{ steps.gitversion.outputs.semVer }}"

    - name: Restore
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release /p:Version=${{ steps.gitversion.outputs.semVer }}

    - name: Test
      run: dotnet test --configuration Release --no-build --verbosity normal

    - name: Publish WPF
      run: dotnet publish CfxTestTool.Wpf/CfxTestTool.Wpf.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:Version=${{ steps.gitversion.outputs.semVer }} -o publish

    - name: Zip output
      run: powershell Compress-Archive -Path publish\* -DestinationPath publish.zip

    - name: Upload artifact
      uses: actions/upload-artifact@v4
      with:
        name: CfxTestTool-${{ steps.gitversion.outputs.semVer }}
        path: publish.zip

