namespace NugetUnlister.IntegrationTests.Toolkit;

public class ConsoleCaptureSession : IDisposable
{
	private readonly TextWriter _defaultWriter;
	private readonly StringWriter _stringWriter = new();

	public string Content => _stringWriter.ToString();

	public ConsoleCaptureSession()
	{
		_defaultWriter = Console.Out;
		Console.SetOut(_stringWriter);
	}

	public void Dispose()
	{
		Console.SetOut(_defaultWriter);
	}
}