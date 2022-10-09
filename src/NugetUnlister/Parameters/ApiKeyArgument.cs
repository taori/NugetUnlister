using System.CommandLine;

namespace NugetUnlister.Parameters;

public class ApiKeyArgument : Argument<string>
{
	public ApiKeyArgument() : base("apiKey", "key to use for nuget operations")
	{

	}
}
