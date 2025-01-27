using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace NugetUnlister.Entities;

public class CatalogRootThreeSixZero
{
	[JsonPropertyName("@id")]
	public string Id { get; set; }
	
	[JsonPropertyName("items")]
    public CatalogPage[] catalogPages { get; set; }

    public class CatalogPage
    {
	    [JsonPropertyName("@id")]
	    public string Id { get; set; }
	    
	    [JsonPropertyName("items")]
	    public Package[]? packages { get; set; }
    }

    public class Package
    {
	    public string commitId { get; set; }
	    [JsonPropertyName("catalogEntry")]
	    public PackageDetails details { get; set; }
    }

    public class PackageDetails
    {
	    public bool listed { get; set; }
	    public string version { get; set; }
    }
}

