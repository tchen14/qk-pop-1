using UnityEngine;
using System.Collections;

public class Goal {
	bool completed = false;

	public enum GoalTypes {
		Interact,
		Collect
	}

	void Evaluate() {

	}

	public bool IsCompleted() {
		return completed;
	}
}
