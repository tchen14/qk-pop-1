using UnityEngine;
using System.Collections;

public class PoPEventTrigger : MonoBehaviour
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
		if (collision.gameObject.GetComponent<PoPEvent>()) {
			collision.gameObject.GetComponent<PoPEvent>().BeginEvent();
		}
	}

	private void QuestAutoSave(){
		
	}
}
