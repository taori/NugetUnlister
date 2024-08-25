using System.Diagnostics;
using System.Text.Json.Serialization;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NugetUnlister.Entities;

internal class NugetServiceIndex
{
	[JsonPropertyName("version")]
	public string Version { get; set; }
	[JsonPropertyName("resources")]
	public ResourcesModel[] Resources { get; set; }
	[JsonPropertyName("@context")]
	public ContextModel Context { get; set; }

	[DebuggerDisplay("{Type}")]
	public class ResourcesModel
	{
		[JsonPropertyName("@id")]
		public string Id { get; set; }
		[JsonPropertyName("@type")]
		public string Type { get; set; }
		[JsonPropertyName("comment")]
		public string Comment { get; set; }
		[JsonPropertyName("clientVersion")]
		public string ClientVersion { get; set; }
	}

	public class ContextModel
	{
		[JsonPropertyName("@vocab")]
		public string Vocab { get; set; }
		[JsonPropertyName("comment")]
		public string Comment { get; set; }
	}
}
