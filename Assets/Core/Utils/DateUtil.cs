using System;
using System.Globalization;

public static class DateUtil {
	const string dateFormat = "dd/MM/yy hh:mm tt";

	public static DateTime StringToDateTime(string date) {
		return DateTime.ParseExact(date, dateFormat, CultureInfo.InvariantCulture);
	}

	public static int CompareStringDate(string date1, string date2) {
		DateTime dateTime1 = DateUtil.StringToDateTime(date1);
		DateTime dateTime2 = DateUtil.StringToDateTime(date2);

		return dateTime1.CompareTo(dateTime2);
	}

	public static string GetCurrentStringDate() {
		return DateTime.Now.ToString(dateFormat);
	}

}
