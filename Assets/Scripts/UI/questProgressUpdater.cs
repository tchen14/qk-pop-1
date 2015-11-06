using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class questProgressUpdater : MonoBehaviour {

	public GameObject progressPercentage;
	public Slider progressSlider;

	Text progressText;

	void Start(){
		if (progressPercentage == null) {
			Debug.LogError("A quest progress slider does not have a UI text object to represent the " +
			               "numerical value for progress.");
		}
		if (progressSlider == null) {
			Debug.LogError("A quest progress slider does not have a UI slider object to reference for the " +
			               "numerical value for progress.");
		}
		progressText = progressPercentage.gameObject.GetComponent<Text> ();
		changeProgressPercentage ();
	}

	public void changeProgressPercentage(){
		progressPercentage.GetComponent<Text>().text = progressSlider.value.ToString () + "%";
	}
}
