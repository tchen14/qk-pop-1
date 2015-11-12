using UnityEngine;
using System.Collections;

public class interact_object : MonoBehaviour {

	public GameObject start_state;
	public GameObject end_state;


	public AnimationCurve speed_curve;

	[Range(0.0f, 10.0f)]
	public float mysliderFloat;


	Vector3 end_position;
	Vector3 start_position;

	Vector3 end_rotation;
	Vector3 start_rotation;

	Transform cur_transform;
	Vector3 eulerRotation;
	
	bool start_moving;
	bool end_movement;
	public float currentLerpTime;
	float lerpTime = 1f;

	// Use this for initialization
	void Awake() {
		start_moving = false;
		end_movement = false;
		end_position = new Vector3(end_state.transform.position.x, end_state.transform.position.y, end_state.transform.position.z);
		start_position = new Vector3(start_state.transform.position.x, start_state.transform.position.y, start_state.transform.position.z);

		start_rotation = new Vector3 (start_state.transform.eulerAngles.x, start_state.transform.eulerAngles.y, start_state.transform.eulerAngles.z);
		end_rotation = new Vector3 (end_state.transform.eulerAngles.x, end_state.transform.eulerAngles.y, end_state.transform.eulerAngles.z);

		cur_transform = start_state.transform;
		Destroy(end_state);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("w"))
		   set_in_motion(true);

		if (start_moving && !end_movement)
		{	
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > lerpTime)
				currentLerpTime = lerpTime;
		
			float speed = (currentLerpTime / lerpTime) * speed_curve.Evaluate(currentLerpTime);
		

		
			cur_transform.position = Vector3.Lerp(start_position, end_position, speed);
			eulerRotation = Vector3.Lerp(start_rotation, end_rotation, speed);
			cur_transform.rotation = Quaternion.Euler (eulerRotation);
		
			if (cur_transform.position == end_position)
				end_movement = true;
		}
	}

	public void set_in_motion(bool condition)
	{
		start_moving = condition;
	}
	
}
