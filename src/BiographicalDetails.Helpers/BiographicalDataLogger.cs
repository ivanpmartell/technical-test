using static System.Environment;

namespace BiographicalDetails.Helpers;

public class BiographicalDataLogger
{
	public string FolderPath = GetFolderPath(SpecialFolder.DesktopDirectory);
	public string FolderName = "logs";

	public void WriteLine(string message)
	{
		var completeDirectoryPath = Path.Combine(FolderPath, FolderName);
		if (!Directory.Exists(completeDirectoryPath))
			Directory.CreateDirectory(completeDirectoryPath);

		var dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		var fullPath = Path.Combine(completeDirectoryPath, $"BiographicalDataLog-{dateTimeStamp}.txt");

		ThreadSafeTextFileWriter.WriteText(fullPath, message);
	}
}
