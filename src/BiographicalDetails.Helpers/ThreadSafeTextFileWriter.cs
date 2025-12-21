namespace BiographicalDetails.Helpers;

public static class ThreadSafeTextFileWriter
{
	private static readonly Lock _lock = new();

	public static void WriteText(string path, string message)
	{
		lock (_lock)
		{
			int attempt = 0;
			int delay = 5;
			int maxRetries = 5;

			try
			{
				using StreamWriter textFile = new(path, true);
				textFile.WriteLine(message);
			}
			catch (IOException ex)
			{
				attempt++;
				if (attempt > maxRetries)
				{
					Console.WriteLine($"ERROR: Writing to {path}.\n{ex.Message}");
					throw;
				}
				Thread.Sleep(delay);
				delay *= 2;
			}
		}
	}
}
