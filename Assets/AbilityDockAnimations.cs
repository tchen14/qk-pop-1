using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityDockAnimations : MonoBehaviour {

	public int[] position;
	public Image[] abilities;
	public float timeTakenDuringLerp = 0.3f;

	float xPosition;
	float timeStartedLerping;
	Image[] abilityDocImages;
	int numAbilities;
	int selectedAbility;
	bool rotating;
	bool canGetInput;
	Vector3[] startPos = new Vector3[5];
	Vector3[] targetPos = new Vector3[5];


	void Start () {
		numAbilities = position.Length;
		canGetInput = true;
		abilityDocImages = this.gameObject.GetComponentsInChildren<Image>();
		foreach(Image icon in abilityDocImages){
			icon.enabled = false;
		}
		for(int i = 0; i < numAbilities; i++){
			position[i] = i;
		}
		selectedAbility = position [2];
		xPosition = abilities [0].transform.position.x;
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			foreach(Image icon in abilityDocImages){
				icon.enabled = true;
			}
		}
		if (Input.GetKeyUp (KeyCode.Tab)) {
			foreach(Image icon in abilityDocImages){
				icon.enabled = false;
			}
			selectedAbility = position [2];
		}
		if (Input.GetKey(KeyCode.Tab) && canGetInput) {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				newPos (true);
				startLerping();
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				newPos (false);
				startLerping();
			}
		}
	}

	void FixedUpdate(){
		if (rotating) {

			float timeSinceStarted = Time.time - timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			for (int i = 0; i < numAbilities; i++) {
				abilities [i].transform.position = Vector3.Lerp(abilities[i].transform.position, targetPos[i], percentageComplete);
			}

			if(percentageComplete >= .35f){
				canGetInput = true;
			}

			if(percentageComplete >= 1f){
				rotating = false;
			}
		}
	}

	void startLerping(){
		rotating = true;
		canGetInput = false;
		timeStartedLerping = Time.time;
	}

	void newPos(bool isUp){

		if (!isUp) {
			for (int i = 0; i < position.Length; i++) {
				abilities [i].transform.SetSiblingIndex (position [i] + 2);
			}
		}
		for (int i = 0; i < position.Length; i++) {
			startPos[i] = new Vector3(xPosition, getPos (position[i]),0);
			if(isUp){
				position[i] = modulo(position[i] + 1, 5);
			}
			else{
				position[i] = modulo(position[i] - 1, 5);
			}
			targetPos[i] = new Vector3(xPosition, getPos (position[i]), 0);
		}
		if (isUp) {
			for (int i = 0; i < position.Length; i++) {
				abilities [i].transform.SetSiblingIndex (position [i] + 2);
			}
		}
	}

	/* Helper function that returns the modulo. This correctly calculates negative numbers as the % oeprator does not by itsself
	 */
	int modulo(int a, int b){
		return (a%b + b)%b;
	}

	/* Helper function that returns the Y value for the next position. 
	 * 
	 * This only works with 5 abilities!
	 * 
	 * If you want it to work with more or less abilities, a function that dynamically generates the next y position will be needed.
	 */
	float getPos(int i){
		switch(i){
		case 0:
			return 55;
		case 1:
			return 135;
		case 2:
			return 215;
		case 3:
			return 295;
		case 4:
			return 375;
		default:
			return 0;
		}
	}

	/* This function returns an integer that corresponds to the ability that is selected
	 * 0 is Cut
	 * 1 is Sound Throw
	 * 2 is Taze
	 * 3 is Push
	 * 4 is Pull
	 */
	public int getSelectedAbility(){
		return selectedAbility;
	}
}
