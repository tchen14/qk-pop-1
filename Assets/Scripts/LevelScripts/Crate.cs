using UnityEngine;
using System.Collections;
//using Debug=FFP.Debug;

[RequireComponent(typeof(Targetable))]
[EventVisible]
public class Crate : Item
{
	// Use this for initialization
	void Start ()
	{
		itemName = "Crate";
		pushCompatible = true;
		pullCompatible = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void PlayPushSound()
	{
		//Play push sound associated with crates
		return;
	}
	
	[EventVisible("test")]
	public void TestCrateFunction(){
		Debug.Log("TestCrateFunction");
	}
}
