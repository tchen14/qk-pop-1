using UnityEngine;
using System.Collections.Generic;

public class PanicTargets : MonoBehaviour {

	public GameObject PanicController;
	public List<Vector3> panicPoints = new List<Vector3>();
	//! Unity Start function
	void Start () {
		PanicController = this.gameObject;
		//panicPoints = PanicController.GetComponentsInChildren<Transform>();
			for(int i = 0; i < PanicController.transform.childCount;i++)
			{
				panicPoints.Add(PanicController.transform.GetChild(i).position);
				Destroy(PanicController.transform.GetChild(i).gameObject);
			}
	}
	
	//! Unity Update function
	public Vector3 GetPanickPoint () {
		return panicPoints[(int)(Mathf.Floor(Random.Range(0,(panicPoints.Count)-1)))];
	}
}
