using UnityEngine;

namespace Warfest {
	public class MouseState : MonoBehaviourSingleton<MouseState> {

		public enum Type {
			Locked,
			Free
		}

		[ReadOnly][SerializeField] Type mode = Type.Locked;

		public static Type Mode {
			get {
				return MouseState.Instance.mode;
			}
			set {
				MouseState.Instance.mode = value;
			}
		}

		void Start() {
			SetCursorLock(true);
		}

		public static bool IsFree() {
			return MouseState.Instance.mode == Type.Free;
		}

		public static bool IsLocked() {
			return !MouseState.IsFree();
		}

		public static void SetLocked(bool locked) {
			var instance = MouseState.Instance;

			if (locked) {
				instance.mode = Type.Locked;
				instance.SetCursorLock(locked);
			} else {
				instance.mode = Type.Free;
				instance.SetCursorLock(locked);
			}
		}

		void SetCursorLock(bool locked) {
			Cursor.lockState = (locked) ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = CursorLockMode.Locked != Cursor.lockState;
		}

	}
}