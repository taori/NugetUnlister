using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Amusoft.Toolkit.Threading;

namespace NugetUnlister.Scopes;

internal class SecretsScope : AmbientScope<SecretsScope>
{
	private readonly HashSet<string> Secrets = new();

	internal SecretsScope WithSecret(string secret)
	{
		Secrets.Add(secret);
		return this;
	}

	internal static string ScrubMessage(string message) => Current?.GetScrubbed(message) ?? message;
	
	internal string GetScrubbed(string message)
	{
		var secrets = Current?.Secrets ?? new HashSet<string>();
		if (secrets.Count == 0)
			return message;
		
		var pattern = string.Join("|", secrets.Select(Regex.Escape));
		return new Regex(pattern).Replace(message, "***");
	}
}
