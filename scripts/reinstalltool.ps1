$toolName = "NugetUnlister"
$projectDirectory = "$PSScriptRoot/../src/NugetUnlister"
$installCache = "$PSScriptRoot/../artifacts/tool"

Remove-Item $installCache -Recurse -Force

dotnet tool uninstall $toolName --global && dotnet build $projectDirectory -c Release && dotnet pack $projectDirectory -o "$installCache" && dotnet tool install --global --add-source $installCache $toolName --prerelease