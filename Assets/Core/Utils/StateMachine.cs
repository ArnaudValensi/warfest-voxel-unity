using System;
using System.Collections.Generic;

public class StateMachine<TLabel> {

	readonly Dictionary<TLabel, State> states;
	State currentState;

	public StateMachine() {
		states = new Dictionary<TLabel, State>();
	}

	public TLabel CurrentState {
		get {
			return currentState.label;
		}
		set {
			ChangeState(value);
		}
	}

	void ChangeState(TLabel newState) {
		if (currentState != null && currentState.onStop != null) {
			currentState.onStop();
		}

		currentState = states[newState];

		if (currentState.onStart != null) {
			currentState.onStart();
		}
	}

	public void Update() {
		if (currentState != null && currentState.onUpdate != null) {
			currentState.onUpdate();
		}
	}

	public override string ToString() {
		return CurrentState.ToString();
	}

	public void AddState(TLabel state, Action onStart, Action onUpdate, Action onStop) {
		states[state] = new State(state, onStart, onUpdate, onStop);
	}

	public void AddState(TLabel state, Action onStart, Action onUpdate) {
		states[state] = new State(state, onStart, onUpdate, null);
	}

	public void AddState(TLabel state, Action onStart) {
		states[state] = new State(state, onStart, null, null);
	}

	public void AddState(TLabel state) {
		states[state] = new State(state, null, null, null);
	}

	class State {
		public TLabel label;
		public Action onStart;
		public Action onUpdate;
		public Action onStop;

		public State(TLabel label, Action onStart, Action onUpdate, Action onStop) {
			this.label = label;
			this.onStart = onStart;
			this.onUpdate = onUpdate;
			this.onStop = onStop;
		}
	}

}
