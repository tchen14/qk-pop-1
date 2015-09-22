using UnityEngine;
using System.Collections;

public class enemyStatusController : MonoBehaviour {

	GameObject mainCamera;
	
	void Start(){
		mainCamera = GameObject.Find ("_Main Camera");
	}
	
	void Update () {
		//!Updates the rotation of the indicator bars so they always face the camera.
		if (mainCamera != null) {
			this.transform.rotation = mainCamera.transform.rotation;
		}
	}
}
