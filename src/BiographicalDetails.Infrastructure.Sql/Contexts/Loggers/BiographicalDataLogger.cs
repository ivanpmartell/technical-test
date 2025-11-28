using BiographicalDetails.Helpers;
using static System.Environment;

namespace BiographicalDetails.Infrastructure.Sql.Contexts.Loggers;

public static class BiographicalDataLogger
{
	public static void WriteLine(string message)
	{
		string folder = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "sql-logs");
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);

		string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		string path = Path.Combine(folder, $"BiographicalDataLog-{dateTimeStamp}.txt");

		ThreadSafeTextFileWriter.WriteText(path, message);
	}
}
