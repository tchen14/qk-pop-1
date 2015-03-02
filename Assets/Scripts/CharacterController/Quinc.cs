using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

public class Quinc : MonoBehaviour
{
	private float pushRate = 0.5f;
	private int pushDistance = 100;
	private float nextPush = 0.0f;
	private int pushRange = 20;

	private float pullRate = 0.5f;
	private int pullDistance = 100;
	private float nextPull = 0.0f;
	private int pullRange = 20;


	public GameObject Target;
	public GameObject Heaven;

	void Start ()
	{
		Heaven = GameObject.FindGameObjectWithTag("Heaven");
	}
	
	void FixedUpdate ()
	{
		if (Input.GetKey(KeyCode.E) && Time.time > nextPush)
		{
			string pushStatus = "";
			nextPush = Time.time + pushRate;
			if(Push(pushStatus))
			{
				print ("Push");
			}
			else
			{
				print (pushStatus);
			}
		}
		else if (Input.GetKey(KeyCode.R) && Time.time > nextPull)
		{
			string pullStatus = "";
			nextPull = Time.time + pullRate;
			if(Pull(pullStatus))
			{
				print ("Pull");
			}
			else
			{
				print (pullStatus);
			}
		}
	}

	//! Function to be called when cutting rope
	void Cut ()
	{
		/*
		Check if Target is Selected
		Get Target GameObject
		Check if Object is Cut Compatible
		Else return "Object not Compatible"
		Check if Object is within Cut Range
		Else return "Object not in Range"
		Call Object.cut() function
		Play animation for player using Cut
		Untarget GameObject?
		*/
	}

	//! Function to be called when pushing a box or other heavy object, pushing at intervals (think Ocarina of time)
	bool Push (string status)
	{
		/*
		Check if Target is Selected
		Get Target GameObject
		Check if Object is Push Compatible
		Else return "Object not Compatible"
		Check if Object is with in Push Range
		Else return "Object not in Range"
		Push the object either with force, translation, or grid-based movement
		Play animation for player using Push
		*/
		print(Vector3.Distance(Target.transform.position, transform.position));

		if(!Target.GetComponent<Item>().pushCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(Target.transform.position, transform.position) > pushRange)
		{
			status = "Not In Range";
			return false;
		}

		/* 
			1. Get direction to push
			2. Get position by taking direction multiplied by distance
			3. Use Raycast from Heaven to find Y value for targetPosition that is valid or is above terrain
			4. Call Coroutine to Lerp to target position
		*/

		Vector3 pushDirection = Target.transform.position - transform.position;
		Vector3 targetPosition = pushDirection * pushDistance;
		Vector3 heavenDirection = targetPosition.normalized;
		RaycastHit hitInfo;

		if(Physics.Raycast(Heaven.transform.position, heavenDirection, out hitInfo))
		{
			targetPosition = hitInfo.point;
		}

		MoveSlowly(Target.gameObject, targetPosition);

		//Target.transform.Translate(pushDirection.normalized * pushDistance * Time.deltaTime);
		
		status = "Success";
		return true;
	}

	//! Function to be called when pulling an object, pulling at intervals (think Ocarina of time)
	bool Pull (string status)
	{
		/*
		Check if Target is Selected
		Get Target GameObject
		Check if Object is PullCompatible
		Else return "Object not Compatible"
		Check if Object is with in Pull Range
		Else return "Object not in Range"
		Pull the object either with force, translation, or grid-based movement
		Play animation for player using Pull
		*/

		if(!Target.GetComponent<Item>().pullCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(Target.transform.position, transform.position) > pullRange)
		{
			status = "Not In Range";
			return false;
		}
		
		Vector3 pullDirection = transform.position - Target.transform.position;
		pullDirection.Normalize();
		Target.transform.rigidbody.AddForce(pullDirection * pullDistance);
		status = "Success";
		return true;
	}

	void SoundThrow ()
	{
		/*
		Check if Target is Selected
		Get Target GameObject
		Check if Object is SoundThrow compatible
		Else return "Object not Compatible"
		Check if Object is within SoundThrow Range
		Else return "Object not in Range"
		Maybe pick type of sound to be thrown?
		Play sound and animation
		Untarget GameObject?
		*/
	}

	void TazeStun ()
	{
		/*
		Check if Target is Selected
		Get Target GameObject
		Check if Object is Stunnable object
		Else return "Object not Compatible"
		Check if Object is within Stun Range
		Else return "Object not in Range"
		Call Object.stun() funciton
		Play animation for player using stun
		Untarget GameObject?
		*/

	}

	IEnumerator MoveSlowly(GameObject targetObject, Vector3 targetPosition)
	{
		float timeSinceStarted = 0.0f;
		while (true)
		{
			timeSinceStarted += Time.deltaTime;
			targetObject.transform.position = Vector3.Lerp (targetObject.transform.position, targetPosition, timeSinceStarted);

			if (targetObject.transform.position == targetPosition)
			{
				yield break;
			}

			yield return null;
		}
	}

}
