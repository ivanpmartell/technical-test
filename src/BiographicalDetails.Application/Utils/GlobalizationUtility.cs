using System.Globalization;

namespace BiographicalDetails.Application.Utils;

public static class GlobalizationUtility
{
	public static int ValidCharToInt(char c)
	{
		//Ensure only ASCII numbers
		if (c < 255)
			return CharUnicodeInfo.GetDecimalDigitValue(c);
		else
			return -1;
	}
}
