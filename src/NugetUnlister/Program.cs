using System;
using System.Diagnostics;
using CommandDotNet;
using CommandDotNet.HelpGeneration;
using CommandDotNet.MicrosoftCommandLineUtils;
using CommandDotNet.Models;
using NLog;

namespace NugetUnlister
{
	class Program
	{
		static int Main(string[] args)
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
					var code = RunApplication(userArgs);
					Console.WriteLine(code);
				} while (input != "exit");

				return 0;
			}
			else
			{
				return RunApplication(args);
			}
#else
				return RunApplication(args);
#endif
		}

		private static int RunApplication(string[] args)
		{
			var runner = new AppRunner<ConsoleShell>(CreateAppSettings());
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
			finally
			{
				LogManager.Flush(TimeSpan.FromSeconds(10));
			}
		}

		private static AppSettings CreateAppSettings()
		{
			var appSettings = new AppSettings();
			appSettings.MethodArgumentMode = ArgumentMode.Parameter;
			appSettings.Case = Case.CamelCase;
			appSettings.Help = new AppHelpSettings()
			{
				TextStyle = HelpTextStyle.Detailed,
				UsageAppNameStyle = UsageAppNameStyle.GlobalTool
			};

			return appSettings;
		}
	}
}
