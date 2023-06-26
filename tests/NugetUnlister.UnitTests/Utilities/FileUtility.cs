using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NugetUnlister.UnitTests.Utilities;

public static class FileUtility
{
	private static string GetManifestFilePath(string path)
	{
		return $"NugetUnlister.UnitTests.{path}";
	}

	public static StreamReader GetEmbeddedFileStream(string path)
	{
		var stream = typeof(FileUtility).Assembly.GetManifestResourceStream(GetManifestFilePath(path));
		return new StreamReader(stream, Encoding.UTF8, true, 1024, true);
	}

	public static async Task<T> GetEmbeddedJsonAsync<T>(string path)
	{
		using (var reader = GetEmbeddedFileStream(path))
		{
			return JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync());
		}
	}
}