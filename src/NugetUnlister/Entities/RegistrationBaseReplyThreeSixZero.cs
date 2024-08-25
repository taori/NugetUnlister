#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace NugetUnlister.Entities;

public class RegistrationBaseReplyThreeSixZero
{
    public Items[] items { get; set; }

    public class Items
    {
	    public Items1[] items { get; set; }
    }

    public class Items1
    {
	    public string commitId { get; set; }
	    public CatalogEntry catalogEntry { get; set; }
    }

    public class CatalogEntry
    {
	    public bool listed { get; set; }
	    public string version { get; set; }
    }
}

