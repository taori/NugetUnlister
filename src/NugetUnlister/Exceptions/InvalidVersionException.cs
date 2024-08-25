namespace NugetUnlister;

public class InvalidVersionException(string version) : ExitCodeException(-101, $"{version} is not a valid version that can be parsed by semver")
{
}
