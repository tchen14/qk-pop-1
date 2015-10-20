using UnityEngine;
using System.Collections;

[EventVisibleAttribute]
public class QuestNPC : MonoBehaviour {

	public Transform startLocation;
	public int questID;


	void Start () {
		startLocation = this.gameObject.transform;
	}

	[EventVisibleAttribute]
	public void ResetLocation() {
		this.gameObject.transform.position = startLocation.position;
		return;
	}
}
