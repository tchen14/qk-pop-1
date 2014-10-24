using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoPEventManager : EventManager
{
	const float slowUpdateSpeed = 0.1f;
	const float slowerUpdateSpeed = 1.0f;
	const float periodicUpdateSpeed = 30.0f;
	#region playerDelegates
	
	
	
	#endregion

	#region cameraDelegates
	
	
	
	#endregion


	void Start()
	{
		StartCoroutine ("SlowUpdate");
		StartCoroutine ("SlowerUpdate");
		StartCoroutine ("PeriodicUpdate");
	}

	void FixedUpdate(){

	}

	void Update()
	{
	
	}

	void LateUpdate(){

	}

	IEnumerator SlowUpdate(){
		;
		yield return new WaitForSeconds(slowUpdateSpeed);
	}

	IEnumerator SlowerUpdate(){
		;
		yield return new WaitForSeconds(slowerUpdateSpeed);
	}

	IEnumerator PeriodicUpdate(){
		;
		yield return new WaitForSeconds(periodicUpdateSpeed);
	}
}