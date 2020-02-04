# NugetUnlister


## NuGet

### CommandLine

[![NuGet Badge](https://buildstats.info/nuget/NugetUnlister)](https://www.nuget.org/packages/NugetUnlister/)
[![NuGet Badge](https://buildstats.info/nuget/NugetUnlister?includePreReleases=true)](https://www.nuget.org/packages/NugetUnlister/)

## CI

| project        | build status           |
| ------------- |-------------|
| master | [![Build Status](https://dev.azure.com/taori/NugetUnlister/_apis/build/status/master?branchName=master)](https://dev.azure.com/taori/NugetUnlister/_build/latest?definitionId=14&branchName=master)|

## Usage

- `nuget-unlist list all [YOUR PACKAGE NAME]`
- `nuget-unlist list PrereleaseBefore [YOUR PACKAGE NAME] [0.1.3] `
- `nuget-unlist drop PrereleaseBefore [YOUR PACKAGE NAME] [0.1.3] [APIKEY] [Source of nuget repository]`
- `nuget-unlist drop ReleaseBefore [YOUR PACKAGE NAME] [0.1.3] [APIKEY] [Source of nuget repository]`

## Sample Usage in CI

```ps
& dotnet tool install --global NugetUnlister

$packageVersion = Get-ChildItem -Recurse -Filter '*.nupkg' | select { $_.Name } -ExpandProperty Name -First 1 | Select-String -Pattern "\d[\d\w\.\+-]+(?=.nupkg)" | %{$_.Matches.Value}

& nuget-unlist drop prereleasebefore [YOUR PACKAGE ID] $packageVersion $(nugetApiKey)
```

## Breaking changes

### Version 1.x.x
- call syntax has been changed for most commands to simplify common execution scenarios as mentioned in #5
