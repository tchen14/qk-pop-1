using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

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
		transform.collider.enabled = false;
		rigidbody.useGravity = true;
		// Function to make this object untargettable
	}
}
