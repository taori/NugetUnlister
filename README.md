# NugetUnlister

## Project state

[![.GitHub](https://github.com/taori/NugetUnlister/actions/workflows/dotnet.yml/badge.svg)](https://github.com/taori/NugetUnlister/actions/workflows/dotnet.yml)
[![GitHub issues](https://img.shields.io/github/issues/taori/NugetUnlister)](https://github.com/taori/NugetUnlister/issues)
[![NuGet version (NugetUnlister)](https://img.shields.io/nuget/v/NugetUnlister.svg)](https://www.nuget.org/packages/NugetUnlister/)
[![NuGet version (NugetUnlister)](https://img.shields.io/nuget/vpre/NugetUnlister.svg)](https://www.nuget.org/packages/NugetUnlister/latest/prerelease)

## Usage

- `nuget-unlist list any [YOUR PACKAGE NAME]`
- `nuget-unlist list pre [YOUR PACKAGE NAME] [0.1.3] `
- `nuget-unlist list rel [YOUR PACKAGE NAME] [0.1.3] `
- `nuget-unlist drop any [YOUR PACKAGE NAME] [0.1.3] [APIKEY] [Source of nuget repository]`
- `nuget-unlist drop pre [YOUR PACKAGE NAME] [0.1.3] [APIKEY] [Source of nuget repository]`
- `nuget-unlist drop rel [YOUR PACKAGE NAME] [0.1.3] [APIKEY] [Source of nuget repository]`

## Sample Usage in CI

```ps
& dotnet tool install --global NugetUnlister

$packageVersion = Get-ChildItem -Recurse -Filter '*.nupkg' | select { $_.Name } -ExpandProperty Name -First 1 | Select-String -Pattern "\d[\d\w\.\+-]+(?=.nupkg)" | %{$_.Matches.Value}

& nuget-unlist drop pre [YOUR PACKAGE ID] $packageVersion $(nugetApiKey)
```

## Breaking changes

### Version 1.x.x
- call syntax has been changed for most commands to simplify common execution scenarios as mentioned in #5

### Version 2.0.0
- This is now a .net 5.0 tool


## Release Information

### Version 2.1.0
- Implementation change to use System.CommandLine to provide tab completion for CLI usage.

