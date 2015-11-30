#pragma warning disable 414     //Variable assigned and not used: desiredDistance

using UnityEngine;
using System.Collections;

public class QK_Camera : MonoBehaviour {

	public static QK_Camera Instance;

	public Transform TargetLookAt;
	public float distance = 5f;
	public float distanceMin = 3f;
	public float distanceMax = 10f;
	public float x_MouseSensitivity = 5f;
	public float y_MouseSensitivity = 5f;
	public float mouseWheelSensitivity = 5f;
	public float y_MinLimit = -40f;
	public float y_MaxLimit = 80f;


	private float mouseX = 0f;
	private float mouseY = 0f;
	private float startDistance = 0f;
	private float desiredDistance = 0f;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		distance = Mathf.Clamp (distance, distanceMin, distanceMax);
		startDistance = distance;
		Reset ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (TargetLookAt == null)
			return;

		HandlePlayerInput ();

		CalculateDesiredPosition ();

		UpdatePosition ();
	}

	void HandlePlayerInput(){
		var deadZone = 0.1f;

		if (Input.GetMouseButton (1)) {
			//The RMB is down get Axis input
			mouseX += Input.GetAxis("Mouse X") * x_MouseSensitivity;
			mouseY -= Input.GetAxis("Mouse Y") * y_MouseSensitivity;
		}

		//This is where mouseY gets limited

		if (Input.GetAxis ("Mouse ScrollWheel") < -deadZone || Input.GetAxis ("Mouse ScrollWheel") > deadZone) {
			desiredDistance = Mathf.Clamp(distance - Input.GetAxis ("Mouse ScrollWheel") * mouseWheelSensitivity, distanceMin, distanceMax);
		}
	}

	void CalculateDesiredPosition () {

	}

	void UpdatePosition () {

	}

	public void Reset() {
		mouseX = 0;
		mouseY = 10;
		distance = startDistance;
		desiredDistance = distance;
	}

	public static void UseExistingOrCreateNewMainCamera() {
		GameObject tempCamera;
		GameObject targetLookAt;
		QK_Camera myCamera;

		if (Camera.main != null) {
			tempCamera = Camera.main.gameObject;
		}
		else {
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent<Camera>();
			tempCamera.tag = "MainCamera";
		}

		tempCamera.AddComponent <QK_Camera>();
		myCamera = tempCamera.GetComponent ("QK_Camera") as QK_Camera;

		targetLookAt = GameObject.Find ("targetLookAt") as GameObject;

		if (targetLookAt == null) {
			targetLookAt = new GameObject("targetLookAt");
			targetLookAt.transform.position = Vector3.zero;
		}

		myCamera.TargetLookAt = targetLookAt.transform;
	}
}
