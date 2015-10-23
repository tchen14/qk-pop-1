using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityDockAnimations : MonoBehaviour {
	
	public Image[] abilities;
	public float timeTakenDuringLerp = 0.35f;
	public Image selectionBeam;
	public Image highligtedIcon;

	int[] position;
	float xPosition;
	float timeStartedLerping;
	Image[] abilityDocImages;
	int numAbilities;
	int selectedAbility;
	bool rotating;
	bool opening;
	bool closing;
	bool canGetInput;
	Vector3[] targetPos = new Vector3[5];


	void Start () {
		canGetInput = true;
		opening = false;
		rotating = false;
		closing = false;
		selectionBeam.rectTransform.sizeDelta = new Vector2 (0, 0);
		selectionBeam.transform.position = new Vector3 (selectionBeam.transform.position.x, 55, 0);
		abilityDocImages = this.gameObject.GetComponentsInChildren<Image>();
		position = new int[abilities.Length];
		numAbilities = position.Length;
		for(int i = 0; i < numAbilities; i++){
			position[i] = i;
		}
		selectedAbility = 2;
		xPosition = abilities [0].transform.position.x;
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			//Open ability dock
			currentPosition();
			opening = true;
			closing = false;
			startLerping();
		}
		else if (Input.GetKey(KeyCode.Tab) && canGetInput) {
			//Ability dock is open
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				selectedAbility = modulo(selectedAbility + 1, 5);
				newPos (true);
				rotating = true;
				startLerping();
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				selectedAbility = modulo(selectedAbility - 1, 5);
				newPos (false);
				rotating = true;
				startLerping();
			}
		}
		else if (Input.GetKeyUp (KeyCode.Tab)) {
			//Close ability dock
			abilities[selectedAbility].transform.SetAsLastSibling();
			closedPosition();
			closing = true;
			opening = false;
			startLerping();
			
		}
	}

	void FixedUpdate(){
		if (opening || rotating || closing) {
			float timeSinceStarted = Time.time - timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
			if(opening){
				highligtedIcon.transform.position = Vector3.Lerp(highligtedIcon.transform.position, new Vector3(highligtedIcon.transform.position.x, 215, 0), percentageComplete);
				selectionBeam.rectTransform.sizeDelta = Vector2.Lerp(selectionBeam.rectTransform.sizeDelta, new Vector2(100, 400), percentageComplete);
				selectionBeam.transform.position = Vector3.Lerp (selectionBeam.transform.position, new Vector3(selectionBeam.transform.position.x, 215, 0), percentageComplete);
			}
			if(closing){
				highligtedIcon.transform.position = Vector3.Lerp(highligtedIcon.transform.position, new Vector3(highligtedIcon.transform.position.x, 55, 0), percentageComplete);
				selectionBeam.rectTransform.sizeDelta = Vector2.Lerp(selectionBeam.rectTransform.sizeDelta, new Vector2(0, 0), percentageComplete);			
				selectionBeam.transform.position = Vector3.Lerp (selectionBeam.transform.position, new Vector3(selectionBeam.transform.position.x, 55, 0), percentageComplete);
			}
			for (int i = 0; i < numAbilities; i++) {
				abilities [i].transform.position = Vector3.Lerp(abilities[i].transform.position, targetPos[i], percentageComplete);
			}

			/* This allows for a slight delay between button presses.
			 * Before this fix, if the user were to spam one of the directional keys, all the abilities would clump together.
			 * This did not look good at all.
			 */
			if(percentageComplete >= .35f){
				canGetInput = true;
			}
			if(percentageComplete >= 1f){
				rotating = false;
				opening = false;
				closing = false;
			}
		}
	}

	void startLerping(){
		canGetInput = false;
		timeStartedLerping = Time.time;
	}

	/*Helper function that gets the target position for where the ability icon will travel next.
	 * It takes in a boolean that should be true when the down arrow is pressed and false when the up arrow is.
	 * The "position" array helps figure out where the ability should travel next.
	 * The if statements before and after the for loop change the order of the abilities so they do not cross over each other when moving.
	 * When an ability needs to travel from the top to the bottom or vice-versa it is set to the back so it travels behind all other icons.
	 */
	void newPos(bool isDown){
		if (isDown) {
			for (int i = 0; i < position.Length; i++) {
				abilities [i].transform.SetSiblingIndex (position [i] + 2);
			}
		}
		for (int i = 0; i < position.Length; i++) {
			if(isDown){
				position[i] = modulo(position[i] - 1, 5);
			}
			else{
				position[i] = modulo(position[i] + 1, 5);
			}
			targetPos[i] = new Vector3(xPosition, getPos (position[i]), 0);
		}
		if (!isDown) {
			for (int i = 0; i < position.Length; i++) {
				abilities [i].transform.SetSiblingIndex (position [i] + 2);
			}
		}
	}

	void currentPosition(){
		for (int i = 0; i < position.Length; i++) {
			targetPos [i] = new Vector3 (xPosition, getPos (position [i]), 0);
		}
	}

	void closedPosition(){
		for (int i = 0; i < position.Length; i++) {
			targetPos [i] = new Vector3 (xPosition, getPos (0), 0);
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
