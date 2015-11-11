using UnityEngine;
using System.Collections;

public enum item_type {
	Crate,
	Rope,
	Well,
	Enemy
};

[RequireComponent (typeof (Collider))]
[RequireComponent(typeof (Targetable))]
[System.Serializable]
//public abstract class Item : MonoBehaviour
public class Item : MonoBehaviour
{
    public item_type itemType;
	public int itemIndex;
    [EventVisible("Pushed X Times")]
    public int pushCounter;
    [EventVisible("Pulled X Times")]
    public int pullCounter;
    [EventVisible("Cut X Times")]
    public int cutCounter;
    [EventVisible("Sound Thrown X Times")]
    public int soundThrowCounter;
    [EventVisible("Stunned X Times")]
    public int stunCounter;
    [EventVisible("Affected by QuinC")]
    public bool quincAffected = false;
	public bool pushCompatible = false;
	public bool pullCompatible = false;
	public bool cutCompatible = false;
	public bool soundThrowCompatible = false;
	public float soundThrowRadius = 25f;
	public bool stunCompatible = false; //!> Might not need this for items

	public bool heatCompatible = false;
	public bool coldCompatible = false;
	public bool blastCompatible = false;



	//Crate variables
	public Vector3 startPosition;		//starting location of the crate
	public float pushPullRadius = 25f;	//maximum movement radius before crate snaps back to startPosition
	public float moveDist;				//distance moved by the crate
	public bool hasMoved;				//flag to track if crate has been moved by Quinc 
	public bool isSnapping;				//flag tracking if crate is currently snapping back
	Color originalColor;				//store the original color of the object while executing quinc functions
	float colorMax;						//maximum length of time the gameObject can remain a color other than the orginal color
	public float colorTime;				//the last time that the gameObject color was changed


	//MoveSlowly variables
	public float smoothing = 1f;			//lerp smoothing rate
	bool pushPullLerp;						//flag to delay crate snapback
	bool killCo = false;					//flag to kill the coroutine


	//Rope variables


	//Well variables

	//Enemy variables
	public const float stunPeriod = 10.0f;
	public float curStunTimer;
	public bool stunState;
	public bool soundThrowAffected;


    //!Player gathers X number of this item, (kill all enemies in area, use other item script to auto drop item into play inventory)
    protected virtual void GatherObjective (int count)
	{
		return;
	}

    //!Player arrives at the item, option with timed, use negative number for stall quest
    protected virtual void ArriveObjective (Vector3 buffer, float timer = 0.0f)
	{
		return;
	}

    //!This item is brought to target location
    protected virtual void EscortObjective (Vector3 location, Vector3 buffer)
	{
		return;
	}
	

	void Start()
	{

		colorTime = Time.time;
		colorMax = 10f;

		//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		originalColor = GetComponent<Renderer>().material.color;

	}

	void Update()
	{


//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		//check if the color has been changed and how long
		if((GetComponent<Renderer>().material.color != originalColor) && ((Time.time - colorTime) > colorMax))
		{

			//change the gameObject back to its original color
			GetComponent<Renderer>().material.color = originalColor;

		}//END if((GetComponent<Renderer>().material.color != originalColor) && ((Time.time - colorTime) > colorMax))
//END TESTING

		//check for movement and if object can be pushed or pulled
		if(hasMoved && !isSnapping && (pushCompatible || pullCompatible))
		{

			//get the total distance from object's start position
			moveDist = Vector3.Distance(startPosition, transform.position);

			//if the distance from the starting position is greater than the maximum allowed distance
			if(moveDist > pushPullRadius)
			{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
				print("calling SnapBack()");
//END TESTING

				//stop the coroutine that is moving the object
	//			Quinc.Instance.stopCo("MoveSlowly");
				//move the object back to the starting position
				StartCoroutine(SnapBack());

			}//END if(moveDist > pushPullRadius)

		}//END if(hasMoved && !isSnapping && (pushCompatible || pullCompatible))
		
		//if an enemy  is stunned
		if(stunCompatible && stunState)
		{

			//update the total time stunned
			curStunTimer += Time.deltaTime;
			//check for stun to have worn off
			if(curStunTimer > stunPeriod)
			{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
				GetComponent<Renderer>().material.color = originalColor;
//END TESTING
				stunState = false;
			}//END if(curStunTimer > stunPeriod)

		}//END if(stunCompatible && stunState)

	}//END void Update()

//END UPDATE
//---------------------------------------------------------------------------
//START CRATE FUNCTIONS

	public void pushPull(Vector3 location, Vector3 direction)
	{

		print("pushPull called");

		if((moveDist < pushPullRadius) && (isSnapping == false))
		{
			print("calling moveSlowly");
			StartCoroutine(MoveSlowly(location, direction));
			print("MoveSlowly called");
		}//END if((moveDist < pushPullRadius) && (isSnapping == false))

	}//END public void pushPull(Vector3 location, Vector3 direction)


	//-------------------------------------------------------------------------------------------------------

	public IEnumerator MoveSlowly(Vector3 targetPosition, Vector3 direction)
	{
		print("Target Position In CoRoutine: " + transform.position);


		int testCount = 0;

		//set flag to delay crate snapback
		pushPullLerp = true;


//		while(Vector3.Distance(transform.position, targetPosition) > 2.0f && (killCo == false))// && (targetObject.GetComponent<Crate>().SnapBack() == null))
		while(Vector3.Distance(transform.position, targetPosition) > 2.0f && (isSnapping == false))
		{

			//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.yellow;
			colorTime = Time.time;
			//END TESTING

			print("MoveSlowly Lerping " + ++testCount);

			transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
			hasMoved = true;
			yield return null;
		}

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		//only change color to blue if loop ended normally
		if(!isSnapping)
		{
			GetComponent<Renderer>().material.color = Color.blue;
			colorTime = Time.time;
			print("Target Reached");
		}
//END TESTING

		//reset flag to allow crate snapback
		pushPullLerp = false;
		killCo = false;

	}//END IEnumerator MoveSlowly(GameObject targetObject, Vector3 targetPosition, Vector3 direction)

	//-------------------------------------------------------------------------------------------------------

	public IEnumerator SnapBack()
	{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		print("SnapBack() called");
//END TESTING

		isSnapping = true;


//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.yellow;
			colorTime = Time.time;
//END TESTING
		while(pushPullLerp)
		{
			//pause here while move slowly stops working
			yield return null;
		}

			//while(Vector3.Distance(transform.position, startPosition) > 0.01f)
		while(moveDist > 0.1f)
		{

			print("SnapBack Lerping");
			transform.position = Vector3.Lerp(transform.position, startPosition, smoothing * Time.deltaTime);
			moveDist = Vector3.Distance(transform.position, startPosition);
			yield return null;

		}

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.blue;
			colorTime = Time.time;
//END TESTING

		hasMoved = false;
		isSnapping = false;
	}

	//! Plays animation and sound for enemy being stunned
	public void Stun(float time)
	{
		switch (itemType) {
		case item_type.Crate:
			break;

		case item_type.Rope:
			break;

		case item_type.Well:
			break;

		case item_type.Enemy:
			StartCoroutine (_Stun (time));
			break;
		}
	}

	private void start_stun(){
		stunState = true;
		// Play Animation and Sound for enemy being stunned
		// Laying Enemy sideways for now
		Vector3 tempAngles = transform.eulerAngles;
		tempAngles.x = 90.0f;
		transform.eulerAngles = tempAngles;
	}

	private void end_stun(){
		stunState = false;
		Vector3 tempAngles = transform.eulerAngles;
		tempAngles.x = 0.0f;
		transform.eulerAngles = tempAngles;
		StopCoroutine ("_Stun");
	}

	public void Cut(){
		print ("Item was cut!");
		cutCompatible = false;
	}

	public void SoundThrow(){
		print ("Item is emitting sounds!");
	}

	public void Push(){
		print ("Item was pushed!");
	}

	public void Pull(){
		print ("Item was pulled!");
	}

	public void Heat(){
		print ("Item is being heat up!");
	}

	public void Cool(){
		print ("Item is frozen!");
	}

	public void Blast(){
		print ("Item is blasted!");
	}

	IEnumerator _Stun(float time){
		start_stun ();
		yield return new WaitForSeconds(time);
		end_stun ();
	}

	private void NoEffect(){

	}
}




