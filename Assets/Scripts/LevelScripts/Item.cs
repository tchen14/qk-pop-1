using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
[RequireComponent(typeof (Targetable))]
[System.Serializable]
//public abstract class Item : MonoBehaviour
public class Item : MonoBehaviour
{

    public string itemName = "";
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

    public enum ItemType
    {
      //! Need list of item types
      Consumable,
      Quest,
      Sellable
    }

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

//END ORIGINAL ITEM CODE
//---------------------------------------------------------------------------
//START OF INTEGRATION WITH INDIVIDUAL ITEMS

	void Start()
	{

		colorTime = Time.time;
		colorMax = 10f;

		if(gameObject.name == "Crate")
		{

			itemName = "Crate";
			pushCompatible = true;
			pullCompatible = true;
			startPosition = transform.position;
			moveDist = 0f;
			hasMoved = false;
			isSnapping = false;

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			originalColor = GetComponent<Renderer>().material.color;
//END TESTING

		}
		else if(gameObject.name == "Rope")
		{

			itemName = "Rope";
			cutCompatible = true;

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			originalColor = GetComponent<Renderer>().material.color;
//END TESTING

		}
		else if(gameObject.name == "Well")
		{

			itemName = "Well";
			soundThrowCompatible = true;

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			originalColor = GetComponent<Renderer>().material.color;
//END TESTING

		}
		else if(gameObject.name == "Enemy")
		{

			itemName = "Enemy";
			stunCompatible = true;
			curStunTimer = 0.0f;
			stunState = false;
			soundThrowAffected = true;

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			originalColor = GetComponent<Renderer>().material.color;
//END TESTING

		}
		else
		{

			print("Item gameObject unknown object");

		}

	}//END void Start()

//END SETUP
//---------------------------------------------------------------------------
//START UPDATE

	void Update()
	{


//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		//check if the color has been changed and how long
		if((GetComponent<Renderer>().material.color != originalColor) && ((Time.time - colorTime) > colorMax))
		{

			//change the gameObject back to its original color
			GetComponent<Renderer>().material.color = originalColor;

		}
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

			}
		}
		//if crate has moved, check the distance from startPosition
		/*		if(hasMoved == true)
				{

					moveDist = Vector3.Distance(startPosition, transform.position);
					//if crate is out of pushPullRadius from startPostion, and isn't currently moving return to startPosition
					if((moveDist > pushPullRadius) && (Quinc.Instance.pushPullLerp == false))
					{

						isSnapping = true;
		//				Quinc.Instance.stopCo("MoveSlowly");
		//				StartCoroutine(SnapBack());
						hasMoved = false;
					}

					//reset hasMoved
		//			hasMoved = false;

				}
		*/
		
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
			}

		}

	}//END void Update()

//END UPDATE
//---------------------------------------------------------------------------
//START CRATE FUNCTIONS

	public void PlayPushSound()
	{
		//Play push sound associated with crates
		return;
	}



	public void pushPull(Vector3 location, Vector3 direction)
	{

		print("pushPull called");

		if((moveDist < pushPullRadius) && (isSnapping == false))
		{
			print("calling moveSlowly");
			StartCoroutine(MoveSlowly(location, direction));
			print("MoveSlowly called");
		}

	}


	//-------------------------------------------------------------------------------------------------------

	public IEnumerator MoveSlowly(Vector3 targetPosition, Vector3 direction)
	{
		print("Target Position In CoRoutine: " + transform.position);


		int testCount = 0;

		//set flag to delay crate snapback
		pushPullLerp = true;


		while(Vector3.Distance(transform.position, targetPosition) > 2.0f && (killCo == false))// && (targetObject.GetComponent<Crate>().SnapBack() == null))
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

		//reset flag to allow crate snapback
		pushPullLerp = false;
		print("Target Reached");
		killCo = false;

		//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		GetComponent<Renderer>().material.color = Color.blue;
		colorTime = Time.time;

		//END TESTING

		yield return null;
	}//END IEnumerator MoveSlowly(GameObject targetObject, Vector3 targetPosition, Vector3 direction)

	//-------------------------------------------------------------------------------------------------------



	public IEnumerator SnapBack()
	{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		print("SnapBack() called");
//END TESTING

		if(Quinc.Instance.coMoveSlowly != null)
		{

			Quinc.Instance.stopCo("MoveSlowly");

		}

		while(!transform.position.Equals(startPosition))
		{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.yellow;
			colorTime = Time.time;
//END TESTING


			//while(Vector3.Distance(transform.position, startPosition) > 0.01f)
			while(moveDist > 0.1f)
			{

				print("SnapBack Lerping");
				transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime);
				moveDist = Vector3.Distance(transform.position, startPosition);
				//yield return null;

			}

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.blue;
			colorTime = Time.time;
//END TESTING

//			hasMoved = false;
//			isSnapping = false;
			yield return null;

		}

		hasMoved = false;
		isSnapping = false;
		yield return null;

	}//end public IEnumerator SnapBack()


//left over from first semester
	[EventVisible("temp")]
	public int temp = 0;

	[EventVisible("test")]
	public void TestCrateFunction()
	{
		Debug.Log("TestCrateFunction");
	}
//END left over from first semester

//END CRATE
//---------------------------------------------------------------------------
//START ROPE FUNCTIONS

	//! Disables collider, enables gravity, marks Rope untargettable
	public void Cut()
	{
		if(cutCompatible == true)
		{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.yellow;
			colorTime = Time.time;
//END TESTING

			//	transform.GetComponent<Collider>().enabled = false;
			GetComponent<Rigidbody>().useGravity = true;
			// Function to make this object untargettable

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.blue;
			colorTime = Time.time;
//END TESTING

		}
		else
		{

			print("Item not cut compatible");

		}
	}//END public void Cut()

//END ROPE
//---------------------------------------------------------------------------
//START WELL FUNCTIONS

	public void SoundThrow()
	{

		if(soundThrowCompatible == true)
		{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.yellow;
			colorTime = Time.time;
//END TESTING

			//Play sound and animation associated with soundThrow for Well

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
			GetComponent<Renderer>().material.color = Color.blue;
			colorTime = Time.time;
//END TESTING

		}
		else
		{

			print("Item not sound throw compatible");

		}

	}//END public void SoundThrow()

//END WELL
//---------------------------------------------------------------------------
//START ENEMY FUNCTIONS

	//! Plays animation and sound for enemy being stunned
	public void Stun()
	{
		stunState = true;
		// Play Animation and Sound for enemy being stunned
		// Laying Enemy sideways for now
		Vector3 tempAngles = transform.eulerAngles;
		tempAngles.x = 90.0f;
		transform.eulerAngles = tempAngles;

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		GetComponent<Renderer>().material.color = Color.blue;
		colorTime = Time.time;
//END TESTING

		return;
	}


}//END public abstract class Item : MonoBehaviour