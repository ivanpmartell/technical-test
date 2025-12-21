namespace BiographicalDetails.Helpers
{
	public static class ThreadSafeTextFileWriter
	{
		private static readonly Lock _lock = new();

		public static void WriteText(string path, string message)
		{
			lock (_lock)
			{
				using StreamWriter textFile = new(path, true);
				textFile.WriteLine(message);
			}
		}
	}
}
