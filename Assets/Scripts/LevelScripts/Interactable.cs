using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public enum ObjectType { Ladder, Door, Sidle }
	public ObjectType Type { get; set; }

	// Ladder Public Variables
	public Vector3 ladderStart = Vector3.zero;
	public Vector3 ladderEnd = Vector3.zero;

	void OnDrawGizmosSelected()
	{
		switch(Type)
		{
			case ObjectType.Ladder:
			Gizmos.color = Color.red;
				Gizmos.DrawLine(ladderStart, ladderEnd);
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(ladderStart, 0.3f);
				Gizmos.DrawWireSphere(ladderEnd, 0.3f);
				Gizmos.color = new Color(0,1,1,0.5f);
				Gizmos.DrawSphere(ladderStart, 0.3f);
				Gizmos.DrawSphere(ladderEnd, 0.3f);
			break;
		}
	}
}
