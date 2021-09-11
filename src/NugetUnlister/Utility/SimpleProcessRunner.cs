using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NugetUnlister.Utility
{
	public class SimpleProcessRunner : IDisposable
	{
		private readonly Process _process;
		private readonly StringBuilder _errorStringBuilder;
		private readonly StringBuilder _outputStringBuilder;

		public SimpleProcessRunner(string fileName, string arguments)
		{
			_process = ObservableProcessFactory.Create(fileName, arguments);
			_outputStringBuilder = new StringBuilder();
			_errorStringBuilder = new StringBuilder();
		}

		public string OutputContent => _outputStringBuilder.ToString();

		public string ErrorContent => _errorStringBuilder.ToString();

		public int ExitCode => _process.ExitCode;

		public async Task ExecuteAsync(TimeSpan timeout)
		{
			using (var cts = new CancellationTokenSource(timeout))
			{
				_process.Start();

				await _process.WaitForExitAsync(cts.Token);
				_outputStringBuilder.Append(await _process.StandardOutput.ReadToEndAsync());
				_errorStringBuilder.Append(await _process.StandardError.ReadToEndAsync());
			}
		}

		public void Dispose()
		{
			_process?.Dispose();
		}
	}
}
