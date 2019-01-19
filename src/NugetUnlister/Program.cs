using System;
using CommandDotNet;

namespace NugetUnlister
{
	class Program
	{
		static int Main(string[] args)
		{
			var runner = new AppRunner<ConsoleShell>();
			try
			{
				return runner.Run(args);
			}
			catch (ExitCodeException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine(e.InnerException?.ToString());
				return e.ExitCode;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine(e.InnerException?.ToString());
				return int.MinValue;
			}
		}
	}
}
