using UnityEngine;
using System.Collections;

public class QuestNPC : MonoBehaviour {

	public Transform startLocation;


	void Start () {
		startLocation = this.gameObject.transform;
	}

	public void Reset() {
		this.gameObject.transform.position = startLocation.position;
		return;
	}
}
