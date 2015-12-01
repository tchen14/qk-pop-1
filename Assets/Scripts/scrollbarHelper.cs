using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scrollbarHelper : MonoBehaviour {

	void Start(){
		gameObject.GetComponent<Scrollbar>().size = 0;
		gameObject.GetComponent<Scrollbar>().value = 0.99f;
	}
}
