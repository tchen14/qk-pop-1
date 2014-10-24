using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour
{
	public float checkUpdateSpeed = 1.0f;
	
	void Start()
	{
		StartCoroutine("SlowUpdate");
	}
	
	IEnumerator SlowUpdate()
	{
	
		yield return new WaitForSeconds(checkUpdateSpeed);
		StartCoroutine("SlowUpdate");
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Checkpoint>()) {
			//run short term checkpoint saving code here
		}
	}

	private void QuickAutoSave(){

	}

	private void QuestAutoSave(){
		//moving to PoPEventTrigger.cs
	}

	//testing code only
	#if UNITY_EDITOR
	private void ManualSave(){
		
	}
	#endif
}
