using UnityEngine;
using System.Collections;
using Debug = FFP.Debug;

/*! 
 * Class added to targetable objects
 * On Creation adds itself to a master list stored on the camera.
 * Accessed by CheckTargets script which checks visibility
 * Updates public flag determining targetability
 */

public class Targetable : MonoBehaviour {

	[ReadOnly] public bool isTargetable = false;
	public float range = 0f;
	[ReadOnly] public float time = 0f;

	void Start()
	{
		PoPCamera.instance.allTargetables.Add (gameObject);

		if(gameObject.GetComponent<Crate>()) {
			range = 50f;
			return;
		} else if(gameObject.GetComponent<Rope>()) {
			range = 40f;
			return;
		} else if(gameObject.GetComponent<Well>()) {
			range = 30f;
			return;
		} else if(gameObject.GetComponent<Enemy>()) {
			range = 20f;
			return;
		} else
			range = PoPCamera.instance.targetingRange;
	}
}
