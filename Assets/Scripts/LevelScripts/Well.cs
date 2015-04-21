using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

[RequireComponent(typeof(Targetable))]
public class Well : Item
{
	// Use this for initialization
	void Start ()
	{
		itemName = "Well";
		soundThrowCompatible = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void SoundThrow()
	{
		//Play sound and animation associated with soundThrow for Well
		return;
	}
}
