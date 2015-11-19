using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

/*!This is a script that uses two different objects to represent start and end.
 * The script takes the start and lerps it to the exact location and orientation
 * of the end object. The duration variable and speed curve affect what the 
 * animation looks like. duration is the time it takes for the animation to complete
 * the speed curve is to accelerate  or decelerate.
 */

public class ItemAnimator : MonoBehaviour {

	public GameObject start_state;
	public GameObject end_state;

	public AnimationCurve speed_curve;
	public float duration = 1.0f;


	Vector3 end_position;
	Vector3 start_position;

	Vector3 end_rotation;
	Vector3 start_rotation;

	Transform cur_transform;
	Vector3 eulerRotation;
	
	bool start_moving;
	bool end_movement;
	bool error_flagged = false;

	float currentLerpTime;
	float lerpTime = 1f;

	void Awake() {
		start_moving = false;
		end_movement = false;


		updateKeyframes ();//!<see the function for notes on what this does

		//the two public gameobjects are important to the script, so they are needed
		if (!start_state) {
			Debug.Error ("level","There is no start_state for " + name + ", need one in public variable");
			error_flagged = true;
		} 
		else if (!end_state) {
			Debug.Error("level","There is no end_state for " + name + ", need one in public variable");
			error_flagged = true;
		}
		else 
		{	
			end_position = new Vector3(end_state.transform.position.x, end_state.transform.position.y, end_state.transform.position.z);
			start_position = new Vector3(start_state.transform.position.x, start_state.transform.position.y, start_state.transform.position.z);

			start_rotation = new Vector3 (start_state.transform.eulerAngles.x, start_state.transform.eulerAngles.y, start_state.transform.eulerAngles.z);
			end_rotation = new Vector3 (end_state.transform.eulerAngles.x, end_state.transform.eulerAngles.y, end_state.transform.eulerAngles.z);
		}

		cur_transform = start_state.transform;
		Destroy(end_state);
	
		if (duration <= 0)
			duration = 0.01f;
	}

	void Update () 
	{
		if (Input.GetKeyDown ("w"))
			set_in_motion ();

		if (!error_flagged && start_moving && !end_movement)
		{	
			move_object();
		}
		
	}

	//!this is the function to call when you want to animate the object at that instance
	public void set_in_motion()
	{
		start_moving = true;
	}

	//function that animates the object until its position is equal to its end position
	void move_object()
	{
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime*duration)
			currentLerpTime = lerpTime*duration;
		
		float lerp_speed = (currentLerpTime / lerpTime) * speed_curve.Evaluate(currentLerpTime);
		
		
		eulerRotation = Vector3.Lerp(start_rotation, end_rotation, lerp_speed/duration);
		cur_transform.rotation = Quaternion.Euler (eulerRotation);
		cur_transform.position = Vector3.Lerp(start_position, end_position, lerp_speed/duration);
		
		
		if (cur_transform.position == end_position)
			end_movement = true;
	}

	//depending on the duration the user inputs, the key ratio needs to be updated, so this does just that
	void updateKeyframes()
	{
		Keyframe[] ks;
		ks = new Keyframe[speed_curve.length];
		keya1 = new float[speed_curve.length];
		keya2 = new float[speed_curve.length];

		for(int i = 0; i < ks.Length; i++)
		{
			Keyframe tmp = speed_curve[i];
			ks[i] = new Keyframe(tmp.time * duration, tmp.value, tmp.inTangent/duration, tmp.outTangent/duration);
		}
		speed_curve = new AnimationCurve(ks);
	}
}
