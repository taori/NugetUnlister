using System;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Threading.Tasks;
using NLog;
using NugetUnlister.Commands;
using NugetUnlister.Commands.Hierarchy;

namespace NugetUnlister
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
#if DEBUG
			if (Debugger.IsAttached)
			{
				string input;
				do
				{
					Console.WriteLine("Waiting for user input.");
					input = Console.ReadLine();
					if (string.IsNullOrEmpty(input))
						return 0;

					Console.Clear();
					var userArgs = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
					var code = await RunApplication(userArgs);
					Console.WriteLine(code);
				} while (input != "exit");

				return 0;
			}
			else
			{
				return await RunApplication(args);
			}
#else
				return await RunApplication(args);
#endif
		}

		private static async Task<int> RunApplication(string[] args)
		{
			var commandLineBuilder = new CommandLineBuilder(new ApplicationRootCommand());
			try
			{
				return await commandLineBuilder
					.UseDefaults()
					.Build()
					.InvokeAsync(args);
			}
			catch (ExitCodeException e)
			{
				Console.WriteLine(e.Message);
				return e.ExitCode;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine(e.InnerException?.ToString());
				return int.MinValue;
			}
			finally
			{
				LogManager.Flush(TimeSpan.FromSeconds(10));
			}
		}
	}
}
