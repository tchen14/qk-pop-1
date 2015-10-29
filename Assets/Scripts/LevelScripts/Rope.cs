using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

[RequireComponent(typeof(Targetable))]
[EventVisible]
//public class Rope : Item
public class Rope : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
//		itemName = "Rope";
//		cutCompatible = true;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//! Disables collider, enables gravity, marks Rope untargettable
	public void Cut()
	{

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		GetComponent<Renderer>().material.color = Color.yellow;
//END TESTING

	//	transform.GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = true;
		// Function to make this object untargettable

//TESTING - FOR LEVEL DESIGN REMOVE FOR FINAL BUILD
		GetComponent<Renderer>().material.color = Color.blue;
//END TESTING

	}
}
