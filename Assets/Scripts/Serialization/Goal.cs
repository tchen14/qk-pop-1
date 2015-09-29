using UnityEngine;
using System.Collections;

public class Goal {

	bool completed = false;
	
	public void Complete() {
		completed = true;
		return;
	}

	public bool IsCompleted() {
		return completed;
	}
}
