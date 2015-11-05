using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

[RequireComponent(typeof(Targetable))]
[EventVisible]
//public class Well : Item
	public class Well : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
//		itemName = "Well";
//		soundThrowCompatible = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void SoundThrow()
	{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		GetComponent<Renderer>().material.color = Color.yellow;
//TESTING

		//Play sound and animation associated with soundThrow for Well

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		GetComponent<Renderer>().material.color = Color.blue;
//END TESTING

		return;
	}
}
