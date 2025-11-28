using BiographicalDetails.Helpers;
using static System.Environment;

namespace BiographicalDetails.TestInfrastructure.Sql.Loggers;

public static class BiographicalDataTestLogger
{
	public static void WriteLine(string message)
	{
		string folder = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "sql-logs-test");
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);

		string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		string path = Path.Combine(folder, $"BiographicalDataLog-{dateTimeStamp}.txt");

		ThreadSafeTextFileWriter.WriteText(path, message);
	}
}
