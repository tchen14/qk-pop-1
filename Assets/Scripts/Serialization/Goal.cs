using UnityEngine;
using System.Collections;

public class Goal {

	string goalName;
	int goalProgress;
	int goalProgressNeeded;
	bool completed = false;

	public Goal(string name) {
		goalName = name;
	}

	public Goal(string name, int progressNeeded) {
		goalName = name;
		goalProgress = 0;
		goalProgressNeeded = progressNeeded;
	}

	public void Complete() {
		completed = true;
		return;
	}

	public bool IsCompleted() {
		return completed;
	}

	public string GetName() {
		return goalName;
	}

	public void Progress() {
		if (goalProgress == null) {
			Debug.Log("No progress available on goal!");
			return;
		}

		goalProgress++;

		if (goalProgress >= goalProgressNeeded) {
			completed = true;
		}

		return;
	}
}
