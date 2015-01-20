using UnityEngine;
using UnityEditor;
using System.Collections;

/*
 * Class for triggering camera events
 * Attach this script to invisible trigger object that player will collide with
 * Camera Position and Focus Vector3 handles appear 
 * Player GameObject must be tagged as "Player"
 */
public class PoPCameraEvent : MonoBehaviour {

	[HideInInspector]
	public Vector3 cameraStartPosition = Vector3.zero;	//!<Start Position of camera on first frame of event
	public Vector3 eventCameraPosition = Vector3.zero;	//!<Position Camera will move to in event
	public Vector3 eventCameraFocus = Vector3.zero;		//!<Position Camera will focus on
	public float eventTimer = 0f;						//!<Time in seconds camera will be in event
	[Range(0.00001f, 0.99f)] 
	public float cameraSpeed = 0.01f;					//!<Time camera takes to reach target position *1.0 is instant movement, 0.0 camera does not move
	
	private new PoPCamera camera;

	void Start()
	{
		camera = Camera.main.GetComponent<PoPCamera>();
	}

	IEnumerator OnTriggerEnter(Collider go)
	{
		if (go.gameObject.tag == "Player") 
		{
			camera.eventTrigger = this;
			camera.inEvent = true;
			yield return new WaitForSeconds(eventTimer);
			camera.Reset ();
			Destroy(this.gameObject);
		}
	}
	
	public void Event()
	{
		cameraStartPosition = camera.transform.position;
		camera.cameraLatency = cameraSpeed;
	}
}
