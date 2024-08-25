using System;
using Amusoft.Toolkit.Threading;

namespace NugetUnlister.Scopes;

public class ConsoleColorScope : AmbientScope<ConsoleColorScope>
{
	private ConsoleColor _before;
	
	public ConsoleColorScope(ConsoleColor color)
	{
		_before = Console.ForegroundColor;
		Console.ForegroundColor = color;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
			Console.ForegroundColor = _before;
		
		base.Dispose(disposing);
	}
}