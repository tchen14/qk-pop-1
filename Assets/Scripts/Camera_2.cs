using UnityEngine;
using System.Collections;

/*
 * Parent Class for Third Person Cameras
 * Includes necessary public and protected variables,
 * As aell as functions for Smooth transition from
 * One point to the another
 */

public abstract class Camera_2 : MonoBehaviour {
    public Transform targetLookAt;
    public float distance = 5f;				   			// Distance from player to the camera (not constant)
    public float distanceMin = 3f;			   			// Minimum Distance
    public float distanceMax = 10f;			   			// Maximum Distance
    public float distanceResumeSmooth = 0.05f; 			// Smooth value for when camera snaps back to preoccluded position
    public float xMouseSensitivity = 5f;	   			// X-Axis mouse sensitivity
    public float yMouseSensitivity = 5f;	   			// Y-Axis mouse sensitivity
    public float mouseWheelSensitivity = 5f;   			// Mouse wheel sensitivity
    public float cameraLatency = 0.2f;		   			// Drag of camera after user moves mouse
    public float yMinLimit = -30f;			   			// Minimum Y value the camera can move
    public float yMaxLimit = 80f;			   			// Maximum Y value the camera can move
    public float occlusionDistanceMove = 0.1f; 			// Distance the camera moves every update if occluded
    public int maxOcclusionChecks = 10;

    protected float mouseX = 0f;			   			// Stores x-axis mouse input for position calculation
    protected float mouseY = 0f;			   			// Stores y-axis mouse input for position calculation
    protected Vector3 position = Vector3.zero; 			// Current position of camera at any given time
    protected Vector3 desiredPosition = Vector3.zero;	// Stores desired camera position during calculation
    protected float desiredDistance = 0f;				// Stores desired camera distance during calculation
    protected float distanceSmooth = 0.05f;				// Smoothing factor for camera move
    protected float preOccludedDistance = 0f;			// Value of distance before camera was moved b/c of occlusion

    //Takes mouse input, finds new camera distance and calculates position
    protected void CalculateDesiredPosition() {
        ResetOccludedDistance();
        // interpolate from current distance to desired
        distance = Mathf.Lerp (distance, desiredDistance, distanceSmooth);
        // Pass mouse input into function to calculate new position
        desiredPosition = CalculatePosition (mouseY, mouseX, distance);
    }

    // Calculate new camera position
    // relative to targets position
    protected Vector3 CalculatePosition (float rotationX, float rotationY, float distance) {
        Vector3 Direction = new Vector3 (0, 0, -distance);
        Quaternion Rotation = Quaternion.Euler (rotationX, rotationY, 0);
        return targetLookAt.position + Rotation * Direction;
    }

    // Resets camera position to preoccluded position
    public abstract void ResetOccludedDistance();
}