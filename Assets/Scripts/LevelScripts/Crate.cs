using UnityEngine;
using System.Collections;
//using Debug=FFP.Debug;

[RequireComponent(typeof(Targetable))]
[EventVisible]
public class Crate : Item
{

	public Vector3 startPosition;		//starting location of the crate
	public float pushPullRadius = 25f;	//maximum movement radius before crate snaps back to startPosition
	public float moveDist;				//distance moved by the crate
	public bool hasMoved;				//flag to track if crate has been moved by Quinc 
	public bool isSnapping;

	// Use this for initialization
	void Start ()
	{

		itemName = "Crate";
		pushCompatible = true;
		pullCompatible = true;
		startPosition = transform.position;
		moveDist = 0f;
		hasMoved = false;
		isSnapping = false;

	}
	
	// Update is called once per frame
	void Update ()
	{
		
		//if crate has moved, check the distance from startPosition
		if(hasMoved == true)
		{

			moveDist = Vector3.Distance(startPosition, transform.position);
			//if crate is out of pushPullRadius from startPostion, return to startPosition
			if(moveDist > pushPullRadius)
			{

				isSnapping = true;
				StartCoroutine(SnapBack());

			}

			//reset hasMoved
			hasMoved = false;

		}
	
	}

	public void PlayPushSound()
	{
		//Play push sound associated with crates
		return;
	}

	IEnumerator SnapBack()
	{

		
		//StopCoroutine(Quinc.Instance.coMoveSlowly);
		while(!transform.position.Equals(startPosition))
		{

			print("SnapBack Lerping");
			transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime);
			//yield return null;
		}

		isSnapping = false;
		yield return null;
	}

	[EventVisible("temp")]
	public int temp = 0;
	
	[EventVisible("test")]
	public void TestCrateFunction(){
		Debug.Log("TestCrateFunction");
	}
}
