using UnityEngine;
using System.Collections;

public class MainMenuInputManager : InputManager {
	
	// TO_DO set variables from json file; for mappable keys
	protected string cancel = "q";
	protected string action0 = "e";

	//! Unity Start function
	void Start () 
	{
	}
	
	//! Unity Update function
	void Update ()
	{
		if(Input.GetKeyDown(cancel))
		{
			
		}
		if(Input.GetKeyDown(action0))
		{

		}
		if(Input.GetKey(forward))
		{
		}
		if(Input.GetKey(backward))
		{
		}
		if(Input.GetKey(left))
		{
		}
		if(Input.GetKey(right))
		{
		}
		if(Input.GetKey(action))
		{
		}
		if(Input.GetKey(sprint))
		{
		}
		if(Input.GetKey(crouch))
		{
		}
		if(Input.GetKey(jump))
		{
		}
	}
}
