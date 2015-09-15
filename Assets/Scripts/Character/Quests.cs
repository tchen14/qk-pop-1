using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Quest {

	string name;
	string description;
	int objective;
	int progress = 0;
	int currTimer = 0;
	int timer;

	List <Quest> currentQuests;

	public Quest(string n, string d, int o) {
		name = n;
		description = d;
		objective = o;
	}

	public Quest (string n, string d, int o, int t) {
		name = n;
		description = d;
		objective = o;
		timer = t;
	}

	void AddQuest() {

	}
}
