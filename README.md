# NugetUnlister

## Usage

- `nuget-unlist list all [YOUR PACKAGE NAME]`
- `nuget-unlist list PrereleaseBefore -p [YOUR PACKAGE NAME] --sv [0.1.3] `
- `nuget-unlist drop PrereleaseBefore -p [YOUR PACKAGE NAME] --sv [0.1.3] -k [APIKEY] -s [Source of nuget repository]`
- `nuget-unlist drop ReleaseBefore -p [YOUR PACKAGE NAME] --sv [0.1.3] -k [APIKEY] -s [Source of nuget repository]`

## CI

| project        | build status           |
| ------------- |-------------|
| master | [![Build status](https://ci.appveyor.com/api/projects/status/dtfuicrw41o9dd2u/branch/master?svg=true)](https://ci.appveyor.com/project/taori/nugetunlister/branch/master)|
| dev      | [![Build status](https://ci.appveyor.com/api/projects/status/dtfuicrw41o9dd2u/branch/dev?svg=true)](https://ci.appveyor.com/project/taori/nugetunlister/branch/dev)     |
| release | [![Build status](https://ci.appveyor.com/api/projects/status/dtfuicrw41o9dd2u/branch/release?svg=true)](https://ci.appveyor.com/project/taori/nugetunlister/branch/release)  |
