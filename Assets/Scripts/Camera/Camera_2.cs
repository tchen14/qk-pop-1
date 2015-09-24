using UnityEngine;
using System.Collections;

/*
 * Parent Class for Third Person Cameras
 * Includes necessary public and protected variables,
 * As well as functions for Smooth transition from 
 * One point to the another
 */ 

public abstract class Camera_2 : MonoBehaviour
{
    public enum CameraState { Normal, CamEvent, Pause, TargetLock, TargetReset } //!< Enumerator to cycle through camera state.
    protected static CameraState _curState;                 //<! Holds current state of the camera. This variable for internal use only, public methods made available in PoPCamera.

	public Transform target;
	[ReadOnly] public float distance = 5f;					//!< Current distance from player to the camera.
	[ReadOnly] public float distanceMin = 3f;			   	//!< Minimum distance from player to camera.
	[ReadOnly] public float distanceMax = 10f;				//!< Maximum distance from player to camera.
	[ReadOnly] public float distanceResumeSmooth = 0.05f; 	//!< Smooth value for when camera snaps back to preoccluded position.
	[ReadOnly] public float xMouseSensitivity = 5f;	   		//!< X-Axis mouse sensitivity
	[ReadOnly] public float yMouseSensitivity = 5f;	   		//!< Y-Axis mouse sensitivity
	[ReadOnly] public float mouseWheelSensitivity = 15f;   	//!< Mouse Wheel Sensitivity
	[ReadOnly] public float cameraLatency = 0.2f;			//!< Drag of camera after user moves mouse
	[ReadOnly] public float rotateSmooth = 0.2f;
	[ReadOnly] public float yMinLimit = -30f;			   	//!< Minimum Y value the camera can move
	[ReadOnly] public float yMaxLimit = 80f;				//!< Maximum Y value the camera can move
	[ReadOnly] public float occlusionDistanceMove = 0.1f; 	//!< Distance the camera moves every update if occluded

	[HideInInspector] public Vector3 targetLookAt;			//!< Position of the object/location the camera is currently looking at
	[HideInInspector] public Vector3 desiredPosition = Vector3.zero;	//!< Stores desired camera position during calculation.

	protected Transform player;							//!< Stores player transform for camera reset
	protected float mouseX = 0f;			   			//!< Stores x-axis mouse input for position calculation
	protected float mouseY = 0f;						//!< Stores y-axis mouse input for position calculation
	protected Vector3 position = Vector3.zero; 			//!< Current position of camera at any given time
	protected float desiredDistance = 0f;				//!< Stores desired camera distance during calculation
	protected float distanceSmooth = 0.03f;				//!< Smoothing factor for camera move
	protected float preOccludedDistance = 0f;			//!< Distance before camera was moved b/c of occlusion


	// Takes mouse input, finds new camera distance and calculates position.
	protected virtual void CalculateDesiredPosition()
	{
		ResetOccludedDistance ();

		// interpolate from current distance to desired
		distance = Mathf.Lerp(distance, desiredDistance, distanceSmooth);

		// Pass mouse input into function to calculate new position
		desiredPosition = CalculatePosition(mouseY, mouseX, distance);
	}

	// Calculate new camera position relative to targets position.
	protected Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 Direction = new Vector3(0, 0, -distance);
		Quaternion Rotation = Quaternion.Euler(rotationX, rotationY, 0);
		return target.position + Rotation * Direction;
	}

	// Updates camera position after new position has been calculated
	protected virtual void UpdatePosition()
	{
		position = Vector3.Lerp (position, desiredPosition, cameraLatency);
		transform.position = position;

		if(_curState != CameraState.Normal)
		{
			Quaternion rotation = Quaternion.LookRotation(targetLookAt - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSmooth);
		}
		else
			transform.LookAt(targetLookAt); 
	}
	
	// Resets camera position to preoccluded position
	protected abstract void ResetOccludedDistance ();	
}