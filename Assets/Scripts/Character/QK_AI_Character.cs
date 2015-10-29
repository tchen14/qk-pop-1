using UnityEngine;
using System.Collections;

public class QK_AI_Character : MonoBehaviour {

	NavMeshAgent agent;
	QK_AI_Movement CharMove;

	public Transform target;
	float targetTolerance = 1;

	Vector3 targetPos;

	// Use this for initialization
	void Start () {
		agent = GetComponentInChildren<NavMeshAgent> ();
		CharMove = GetComponent<QK_AI_Movement> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
			if ((target.position - targetPos).magnitude > targetTolerance) {
				targetPos = target.position;
				agent.SetDestination (targetPos);
			}

			agent.transform.position = transform.position;

			CharMove.Move (agent.desiredVelocity);
		} 
		else {
			CharMove.Move(Vector3.zero);
		}
	}
}
