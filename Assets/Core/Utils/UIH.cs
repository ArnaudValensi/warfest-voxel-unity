using UnityEngine;
using UnityEditor;

public static class UIH {

	#region Layout

	// Toggle Button Style.
	static GUIStyle ToggleButtonStyleNormal = null;
	static GUIStyle ToggleButtonStyleToggled = null;

	static UIH() {
		// Define styles to make buttons visually toggable.
		ToggleButtonStyleNormal = "Button";
		ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
		ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
	}

	public static void LayoutV(System.Action action, bool box = false, int colorID = 0) {
		if (box) {
			GUIStyle style = new GUIStyle(GUI.skin.box);
			style.padding = new RectOffset(6, 6, 2, 2);
			Color old = GUI.color;
			GUI.color = GetLayoutColor(colorID);
			GUILayout.BeginVertical(style);
			GUI.color = old;
		} else {
			GUILayout.BeginVertical();
		}
		Color _old = GUI.color;
		GUI.color = Color.Lerp(GetLayoutColor(colorID), Color.white, 0.94f);
		action();
		GUI.color = _old;
		GUILayout.EndVertical();
	}

	public static void LayoutH(System.Action action, bool box = false, int colorID = 0) {
		if (box) {
			GUIStyle style = new GUIStyle(GUI.skin.box);
			Color old = GUI.color;
			GUI.color = GetLayoutColor(colorID);
			GUILayout.BeginHorizontal(style);
			GUI.color = old;
		} else {
			GUILayout.BeginHorizontal();
		}
		action();
		GUILayout.EndHorizontal();
	}

	public static void LayoutF(System.Action action, string label, ref bool open, bool box = false, int colorID = 0) {
		bool _open = open;
		LayoutV(() => {
			_open = GUILayout.Toggle(
				_open,
				label,
				GUI.skin.GetStyle("foldout"),
				GUILayout.ExpandWidth(true),
				GUILayout.Height(18)
			);
			if (_open) {
				action();
			}
		}, box, colorID);
		open = _open;
	}

	public static Color GetLayoutColor(int id) {
		switch (id) {
		default:
		case 0:
			return Color.white;
		case 1:
			return Color.black;
		case 2:
			return new Color(1, 0.24f, 0.14f, 1);
		case 3:
			return new Color(1, 1, 0.14f, 1);
		case 4:
			return new Color(0.14f, 1, 0.14f, 1);
		case 5:
			return new Color(0.14f, 1, 1, 1);
		case 6:
			return new Color(0.14f, 0.14f, 1, 1);
		}
	}

	public static Rect GUIRect(float width, float height, bool exWidth = false, bool exHeight = false) {
		return GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(exWidth), GUILayout.ExpandHeight(exHeight));
	}

	public static void Space(float space = 4f) {
		GUILayout.Space(space);
	}

	public static void Header(string text) {
		EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
	}

	public static bool TextureToggleButton(Texture texture, bool isSelected) {
		return GUILayout.Button(texture, isSelected ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
	}

	public static bool TextureToggleButton(string text, bool isSelected) {
		return GUILayout.Button(text, isSelected ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
	}

	#endregion


	#region Editor

	public static void Dialog (string title, string msg, string ok, string cancel = "") {
		EditorApplication.Beep();
		if (string.IsNullOrEmpty(cancel)) {
			EditorUtility.DisplayDialog(title, msg, ok);
		} else {
			EditorUtility.DisplayDialog(title, msg, ok, cancel);
		}
	}

	public static void TimeSpentDialog(System.Action action, string logPrefix = "Time", bool logMessage = true) {
		string successMsg;

		System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
		watch.Start();

		action();

		watch.Stop();
		string secondStr = watch.Elapsed.TotalSeconds.ToString("0.00");

		successMsg = string.Format("Done in {0}s", secondStr);

		if (logMessage) {
			Debug.Log("[" + logPrefix + "] " + successMsg);
		}

		UIH.Dialog("Success", successMsg, "OK");
	}

	public static void ProgressBar (string title, string msg, float value) {
		value = Mathf.Clamp01(value);
		EditorUtility.DisplayProgressBar(title, msg, value);
	}


	public static void ClearProgressBar () {
		EditorUtility.ClearProgressBar();
	}

	#endregion

}
