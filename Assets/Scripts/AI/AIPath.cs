using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPath : MonoBehaviour {

	public List<Checkpoint> checkpoints;

	// Use this for initialization
	void Start () {

	}

	void addCheckpoint()
	{
		Checkpoint new_point = Instantiate(point);
		checkpoints.Add(point);
	}
}
