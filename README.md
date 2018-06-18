# TextRpgCreator
An engine for running your self-created text adventures!

## Requirements
- .NET Standard 2.0 runtime, tested with .NET Core 2.0

- NuGet packages (should be installed automatically by dotnet): 
  - Eto.Forms
  - Eto.Platforms.Your-Platform-Here
  - Microsoft.Extensions.Configuration
  - NetEscapades.Configuration.Yaml
  - Serilog
  - Serilog.Sinks.Console
  - YamlDotNet

Platform-specific:
- Linux:
  - .NET Core 2.0
  - gtk-sharp2
  - gtk3
  - mono framework
- Windows: dunno, cant test... Should just work.
- Mac:
  - Add Eto.Platforms.Mac64 nuget package
  - Build the project
  - now it should just work, but I cannot test it

## Project structure
A game/project is basically a folder containing specific files.

- project-info.yaml
- start-characters.yaml
- items
  - weapons.yaml
  - armor.yaml
...
