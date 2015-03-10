using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

public class Quinc : MonoBehaviour
{
	#region push variables
	private float pushRate = 0.5f;
	public float pushDistance = 5.0f;
	public float pushModifier = 50.0f;
	private float nextPush = 0.0f;
	private int pushRange = 20;
	#endregion

	#region pull variables
	private float pullRate = 0.5f;
	public float pullDistance = 5.0f;
	public float pullModifier = 50.0f;
	private float nextPull = 0.0f;
	private int pullRange = 20;
	#endregion

	#region cut variables
	private int cutRange = 20;
	#endregion

	#region soundThrow variables
	private int soundThrowRange = 20;
	#endregion
	
	#region stun variables
	private int stunRange = 20;
	#endregion

	public float smoothing = 1f;


	public GameObject pushPullTarget;
	public GameObject cutTarget;
	public GameObject soundThrowTarget;
	public GameObject stunTarget;

	void Start ()
	{
	}
	
	void FixedUpdate ()
	{
		if (Input.GetKey(KeyCode.Alpha1) && Time.time > nextPush)
		{
			string pushStatus = "";
			nextPush = Time.time + pushRate;
			if(Push(pushStatus))
			{
				print ("Push status: " + pushStatus);
			}
			else
			{
				print ("Push Error: " + pushStatus);
			}
		}
		else if (Input.GetKey(KeyCode.Alpha2) && Time.time > nextPull)
		{
			string pullStatus = "";
			nextPull = Time.time + pullRate;
			if(Pull(pullStatus))
			{
				print ("Pull Status: " + pullStatus);
			}
			else
			{
				print ("Pull Error: " + pullStatus);
			}
		}
		else if (Input.GetKey(KeyCode.Alpha3))
		{
			string cutStatus = "";
			if(Cut(cutStatus))
			{
				print ("Cut Status: " + cutStatus);
			}
			else
			{
				print ("Cut Error: " + cutStatus);
			}
		}
		else if(Input.GetKey(KeyCode.Alpha4))
		{
			string soundStatus = "";
			if(SoundThrow(soundStatus))
			{
				print ("Sound Status: " + soundStatus);
			}
			else
			{
				print ("Sound Error: " + soundStatus);
			}
		}
		else if(Input.GetKey(KeyCode.Alpha5))
		{
			string stunStatus = "";
			if(Stun(stunStatus))
			{
				print ("Stun Status: " + stunStatus);
			}
			else
			{
				print ("Stun Error: " + stunStatus);
			}
		}
	}

	//! Function to be called when cutting rope
	bool Cut (string status)
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
		
		if(!cutTarget.GetComponent<Item>().cutCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(cutTarget.transform.position, transform.position) > cutRange)
		{
			status = "Not In Range";
			return false;
		}

		cutTarget.GetComponent<Rope>().Cut();

		// Untarget Cuttable Object

		return true;
		
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
		print("Distance between Player and Target: " + Vector3.Distance(pushPullTarget.transform.position, transform.position));

		if(!pushPullTarget.GetComponent<Item>().pushCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(pushPullTarget.transform.position, transform.position) > pushRange)
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

		/*Vector3 pushDirection = transform.position - Target.transform.position;
		pushDirection = -pushDirection;
		//pushDirection.Normalize();
		Vector3 targetPosition = pushDirection.normalized * pushDistance;
		print ("Push Direction: " + pushDirection);
		print ("Push Direction Normalized: " + pushDirection.normalized);
		print ("Target Object: " + Target.transform.position);
		print ("Player Position: " + transform.position);
		print ("Target Location: " + targetPosition);

		GameObject tempGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		tempGo.transform.position = targetPosition;
		tempGo.renderer.material.color = Color.red;
		
		//StartCoroutine(MoveSlowly(Target.gameObject, targetPosition));
		StartCoroutine(MoveSlowly(Target.gameObject, targetPosition, pushDirection.normalized));

		//Target.transform.Translate(pushDirection.normalized * pushDistance * Time.deltaTime);
		
		status = "Success";
		return true;*/

		Vector3 pushDirection = pushPullTarget.transform.position - transform.position;
		pushDirection.Normalize();
		pushPullTarget.transform.rigidbody.AddForce(pushDirection * pushDistance * pushModifier);
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

		if(!pushPullTarget.GetComponent<Item>().pullCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(pushPullTarget.transform.position, transform.position) > pullRange)
		{
			status = "Not In Range";
			return false;
		}
		
		/*
		//Force
		Vector3 pullDirection = transform.position - pushPullTarget.transform.position;
		pullDirection.Normalize();
		pushPullTarget.transform.rigidbody.AddForce(pullDirection * pullDistance * pullModifier, ForceMode.VelocityChange);
		status = "Success";
		*/


		/*
		//MoveTowards
		Vector3 pullDirection = transform.position - pushPullTarget.transform.position;
		pullDirection.Normalize();
		Vector3 targetPos = (pullDirection * pullDistance) + pushPullTarget.transform.position;
		pushPullTarget.transform.position = Vector3.MoveTowards(pushPullTarget.transform.position, targetPos, pullDistance);
		status = "Success";
		*/

		 // StartCoroutine
		Vector3 pullDirection = transform.position - pushPullTarget.transform.position;
		pullDirection.Normalize();
		Vector3 targetPosition = (pullDirection * pullDistance) + pushPullTarget.transform.position;
		StartCoroutine(MoveSlowly(pushPullTarget.gameObject, targetPosition, pullDirection));
		return true;
		
	}

	bool SoundThrow (string status)
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

		if(!soundThrowTarget.GetComponent<Item>().soundThrowCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(soundThrowTarget.transform.position, transform.position) > soundThrowRange)
		{
			status = "Not In Range";
			return false;
		}

		// Possibly allow player to select sound, or each object will have it's own sound attached to it
		// Call SoundThrow function in Well script, which will play sound and animation
		soundThrowTarget.GetComponent<Well>().SoundThrow();

		// Untarget GameObject?

		return true;
	}

	bool Stun (string status)
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

		if(!stunTarget.GetComponent<Item>().pullCompatible)
		{
			status = "Not Compatible";
			return false;
		}
		if(Vector3.Distance(pushPullTarget.transform.position, transform.position) > pullRange)
		{
			status = "Not In Range";
			return false;
		}

		stunTarget.GetComponent<Enemy>().Stun();

		//Untarget Enemy?

		return true;
	}

	//IEnumerator MoveSlowly(GameObject targetObject, Vector3 targetPosition)
	IEnumerator MoveSlowly(GameObject targetObject, Vector3 targetPosition, Vector3 direction)
	{
		print ("Target Position In CoRoutine: " + targetPosition);

		/*while(Vector3.Distance(targetObject.transform.position, targetPosition) > 2.0f)
		{
			//targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPosition, smoothing * Time.deltaTime);
			//pushPullTarget.transform.Translate(direction * pushDistance * Time.deltaTime);
			targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, pushDistance);
			yield return null;
		}*/

		targetObject.transform.position = Vector3.MoveTowards(targetObject.transform.position, targetPosition, pushDistance);
		yield return null;

		print ("Target Reached");
	}

}
