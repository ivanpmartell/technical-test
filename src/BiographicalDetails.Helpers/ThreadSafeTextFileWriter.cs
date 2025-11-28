namespace BiographicalDetails.Helpers
{
	public static class ThreadSafeTextFileWriter
	{
		private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

		public static void WriteText(string path, string message)
		{
			_readWriteLock.EnterWriteLock();
			try
			{
				using (StreamWriter textFile = File.AppendText(path))
				{
					textFile.WriteLine(message);
				}
			}
			finally
			{
				_readWriteLock.ExitWriteLock();
			}
		}
	}
}
