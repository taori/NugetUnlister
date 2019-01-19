using System;

namespace NugetUnlister
{
	public class ExitCodeException : Exception
	{
		/// <inheritdoc />
		public ExitCodeException(int exitCode, string message, Exception innerException) : base(message, innerException)
		{
			ExitCode = exitCode;
		}

		/// <inheritdoc />
		public ExitCodeException(int exitCode, string message) : base(message)
		{
			ExitCode = exitCode;
		}

		public int ExitCode { get; set; }
	}
}