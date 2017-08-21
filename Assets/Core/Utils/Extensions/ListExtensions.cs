using System.Collections.Generic;
using System;

public static class ListExtensions {
	public static T PopAt<T>(this List<T> list, int index) {
		var r = list[index];
		list.RemoveAt(index);
		return r;
	}

	public static T PopFirst<T>(this List<T> list) {
		var r = list[0];
		list.RemoveAt(0);
		return r;
	}

	public static T PopFirst<T>(this List<T> list, Predicate<T> predicate) {
		var index = list.FindIndex(predicate);
		var r = list[index];
		list.RemoveAt(index);
		return r;
	}

	public static T PopFirstOrDefault<T>(this List<T> list, Predicate<T> predicate) where T : class {
		var index = list.FindIndex(predicate);
		if (index > -1) {
			var r = list[index];
			list.RemoveAt(index);
			return r;
		}
		return null;
	}
}