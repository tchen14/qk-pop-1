using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

[RequireComponent(typeof(Targetable))]
[EventVisible]
public class Rope : Item
{
	// Use this for initialization
	void Start ()
	{
		itemName = "Rope";
		cutCompatible = true;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//! Disables collider, enables gravity, marks Rope untargettable
	public void Cut()
	{
		transform.GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = true;
		// Function to make this object untargettable
	}
}
