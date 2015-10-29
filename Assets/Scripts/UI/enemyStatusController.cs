using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class enemyStatusController : MonoBehaviour {

	GameObject mainCamera;
	Slider suspicionSlider;
	Slider agressionSlider;
	Slider[] sliders = new Slider[2];

	void Start(){
		mainCamera = GameObject.Find ("_Main Camera");
		sliders = gameObject.GetComponentsInChildren<Slider> ();
		suspicionSlider = sliders [0];
		agressionSlider = sliders [1];
	}
	
	void Update () {
		//!Updates the rotation of the indicator bars so they always face the camera.
		if (mainCamera != null) {
			this.transform.rotation = mainCamera.transform.rotation;
		}
	}

	/* Returns the float value for Suspicion that is represented by the respective slider.
	 */
	public float getSuspicion(){
		return suspicionSlider.value;
	}

	/* Returns the float value for Agression that is represented by the respective slider.
	 */
	public float getAgression(){
		return agressionSlider.value;
	}

	/* Sets the value that represents suspicion on the slider. Setting this value automatically updates the slider to show the correct value.
	 * If the value passed to the function is outside the bounds of the slider, an error is thrown to to debug console.
	 */
	public void setSuspicion(float value){
		if (value > suspicionSlider.maxValue || value < suspicionSlider.minValue) {
			Debug.LogError ("Error: Cannot set value of Suspicion Slider. Value given is outside the bounds of the Suspicion Slider!");
		} 
		else {
			suspicionSlider.value = value;
		}
	}

	/* Sets the value that represents agression on the slider. Setting this value automatically updates the slider to show the correct value.
	 * If the value passed to the function is outside the bounds of the slider, an error is thrown to to debug console.
	 */
	public void setAgression(float value){
		if (value > agressionSlider.maxValue || value < agressionSlider.minValue) {
			Debug.LogError ("Error: Cannot set value of Agression Slider. Value given is outside the bounds of the Agression Slider!");
		} 
		else {
			agressionSlider.value = value;
		}
	}
}
