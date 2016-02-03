using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {

	public string SceneToLoad;
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player")
		{
            Application.LoadLevel(SceneToLoad);
		}
	}
}
