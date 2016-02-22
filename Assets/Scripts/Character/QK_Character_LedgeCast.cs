using UnityEngine;
using System.Collections;

public class QK_Character_LedgeCast : MonoBehaviour {
	public Vector3 col_point;
	private bool ledge_found = false;
	private GameObject player;
	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<Interactable>()) {	
			if (col.gameObject.GetComponent<Interactable> ().Type == Interactable.ObjectType.Ledge) {
				col_point = col.collider.ClosestPointOnBounds (this.transform.position);
				player = GameObject.FindGameObjectWithTag ("Player");
				//apply hanging calculation to col_point
				col_point.y -= 1f;
				player.transform.position = col_point;
				player.GetComponent<QK_Character_Movement>()._stateModifier = QK_Character_Movement.CharacterState.Hang;
				player.GetComponent<QK_Character_Movement>().ledge = col.gameObject;
			}
		}
	}

	public bool Check(){
		return ledge_found;
	}
	public void Erase(){
		Destroy (this.gameObject);
	}

	public Vector3 HangCoord(){
		return col_point;
	}
}
