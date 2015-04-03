using UnityEngine;
using System.Collections;

//! InputType responsible for UI interaction
public class MenuInputType : InputType {
	
	// TO_DO set variables from json file; for mappable keys
	protected string cancel = "q";
	protected string action0 = "e";

	protected string forward = "w";
	protected string backward = "s";
	protected string left = "a";
	protected string right = "d";
	protected string action = "return";
	protected string sprint = "left shift";
	protected string crouch = "left ctrl";
	protected string jump = "space";

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
