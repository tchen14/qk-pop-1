using UnityEngine;
using System.Collections;

public class Goal {
	bool completed = false;

	protected virtual void Evaluate() {

	}

	public void Complete() {
		completed = true;
		return;
	}

	public bool IsCompleted() {
		return completed;
	}
}
