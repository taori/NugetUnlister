using System.Runtime.CompilerServices;
using DiffEngine;
using Shouldly.Configuration;
using DiffTool = DiffEngine.DiffTool;

namespace NugetUnlister.IntegrationTests.Configuration;

public class VerifyTestConfiguration
{
	[ModuleInitializer]
	public static void Initialize()
	{
		DiffTools.UseOrder(DiffTool.TortoiseGitMerge, DiffTool.VisualStudioCode, DiffTool.VisualStudio);

		VerifierSettings.DerivePathInfo(
			(sourceFile, projectDirectory, type, method) => new(
				directory: Path.Combine(projectDirectory, "Snapshots"),
				typeName: type.Name,
				methodName: method.Name));
	}
}