using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component {
	static T _instance;

	public static T Instance {
		get {
			if (_instance == null) {
				var objs = FindObjectsOfType(typeof(T)) as T[];
				if (objs.Length > 0)
					_instance = objs[0];
				if (objs.Length > 1) {
					Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
				}
				if (_instance == null) {
					GameObject obj = new GameObject();
					_instance = obj.AddComponent<T>();
				}
			}
			return _instance;
		}
	}
}

public class MonoBehaviourSingletonHided<T> : MonoBehaviour where T : Component {
	static T _instance;

	public static T Instance {
		get {
			if (_instance == null) {
				var objs = FindObjectsOfType(typeof(T)) as T[];
				if (objs.Length > 0)
					_instance = objs[0];
				if (objs.Length > 1) {
					Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
				}
				if (_instance == null) {
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<T>();
				}
			}
			return _instance;
		}
	}
}

public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour where T : Component {
	public static T Instance { get; private set; }

	public virtual void Awake() {
		if (Instance == null) {
			Instance = this as T;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}
	}
}
