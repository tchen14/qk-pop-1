using UnityEngine;
using System.Collections;

public class enemyBehaviorStatusController : MonoBehaviour {

	// Use this for initialization
	GameObject camera;

	void start(){
		camera = GameObject.Find("_Main Camera");
	}

	// Update is called once per frame
	void Update () {
		this.transform.rotation = camera.transform.rotation;
	}
}
