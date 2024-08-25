using System;
using System.IO;
using System.Reflection;

namespace NugetUnlister.UnitTests.Utilities;

internal class EmbeddedResourceReader
{
	private readonly Assembly _assembly;

	public EmbeddedResourceReader(Assembly assembly)
	{
		_assembly = assembly;
	}

	public string GetContent(string accessPath)
	{
		try
		{
			using var reader = GetStream(accessPath);
			if (reader is null)
				throw new Exception($"{accessPath} not found");
			
			using var streamReader = new StreamReader(reader);
			return streamReader.ReadToEnd();
		}
		catch (Exception e)
		{
			throw new Exception(string.Format("available names: {0}", string.Join(",", _assembly.GetManifestResourceNames())), e);
		}
	}

	private Stream GetStream(string accessPath)
	{
		var fullPath = _assembly.GetName().Name + "." + accessPath;
		return _assembly.GetManifestResourceStream(fullPath) ?? throw new Exception(string.Format("Failed to load stream - available names: {0}", string.Join(",", _assembly.GetManifestResourceNames())));
	}
}