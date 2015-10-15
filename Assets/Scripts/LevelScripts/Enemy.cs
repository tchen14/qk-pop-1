using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

[EventVisible]
public class Enemy : Item
{
	public const float stunPeriod = 10.0f;
	public float curStunTimer = 0.0f;
	public bool stunState = false;
	Color regColor;

	void Start ()
	{
		itemName = "Enemy";
		stunCompatible = true;
		regColor = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(stunState)
		{
			curStunTimer += Time.deltaTime;
			if(curStunTimer > stunPeriod)
			{
				stunState = false;
			}
		}
	}
	
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
		GetComponent<Renderer>().material.color = regColor;
//END TESTING

		return;
	}
}
