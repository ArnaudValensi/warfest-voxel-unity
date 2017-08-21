using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Async : MonoBehaviour {

	static Async instance;
	public static Async Instance { get { return instance; } }

	void Awake() {
		instance = this;
	}

	//----------------------------------------------------------------------------
	// Invoke Action
	//----------------------------------------------------------------------------

	public static IEnumerator Invoke(float time, Action action) {
		IEnumerator routine = InvokeImpl(time, action);

		instance.StartCoroutine(routine);
		return routine;
	}

	static IEnumerator InvokeImpl(float time, Action action) {
		yield return new WaitForSeconds(time);
		action();
	}

	public static IEnumerator InvokeNextFrame(Action action) {
		IEnumerator routine = InvokeNextFrameImpl(action);

		instance.StartCoroutine(routine);
		return routine;
	}

	static IEnumerator InvokeNextFrameImpl(Action action) {
		yield return null;
		action();
	}

	public static IEnumerator InvokeEveryUpdate(Action action) {
		IEnumerator routine = InvokeEveryUpdateImpl1(action);

		instance.StartCoroutine(routine);
		return routine;
	}

	static IEnumerator InvokeEveryUpdateImpl1(Action action) {
		while (true) {
			yield return null;
			action();
		}
	}

	public static IEnumerator InvokeEveryUpdate(Func<bool> action) {
		IEnumerator routine = InvokeEveryUpdateImpl2(action);

		instance.StartCoroutine(routine);
		return routine;
	}

	static IEnumerator InvokeEveryUpdateImpl2(Func<bool> action) {
		while (true) {
			yield return null;
			if (!action()) {
				yield break;
			}
		}
	}

	public static IEnumerator InvokeEveryFixedUpdate(Action action) {
		IEnumerator routine = InvokeEveryFixedUpdateImpl1(action);

		instance.StartCoroutine(routine);
		return routine;
	}

	static IEnumerator InvokeEveryFixedUpdateImpl1(Action action) {
		while (true) {
			yield return new WaitForFixedUpdate();
			action();
		}
	}

	public static void StopInvoke(IEnumerator routine) {
		instance.StopCoroutine(routine);
	}

	//----------------------------------------------------------------------------
	// Invoke Action
	//----------------------------------------------------------------------------

	/// <summary>
	/// This class instanciate an object which execute a function over a list until the callback
	/// of this function pass true.
	/// The final callback is called when the first function pass true in its callback or when all 
	/// the function callbacks are called.
	/// </summary>
	public class ForEachUntilAny<T> {
		readonly List<T> items;
		readonly Action<T, Action<bool>> fn;
		readonly Action<bool> cb;

		bool result = false;
		int nbFnCalled = 0;

		public ForEachUntilAny(List<T> items, Action<T, Action<bool>> fn, Action<bool> cb) {
			this.items = items;
			this.fn = fn;
			this.cb = cb;

			Async.InvokeEveryUpdate(OnUpdate);
		}

		bool OnUpdate() {
			if (result) {
				cb(true);
				return false; // Stop coroutine
			}

			if (items.Count > 0) {
				nbFnCalled++;
				fn(items.PopFirst(), isDone => {
					result = isDone;
					nbFnCalled--;
				});
			} else if (nbFnCalled == 0) {
				cb(false);
				return false; // Stop coroutine
			}

			return true; // Continue coroutine
		}
	}

}
