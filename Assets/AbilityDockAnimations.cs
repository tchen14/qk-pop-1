using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine.UI;

public class AbilityDockAnimations : MonoBehaviour {

	public int[] position;
	public Image[] abilities;
	public float lerpTime;

	public float[] targetPosY;
	public float[] startPosY;

	public float waitTimeCoro;

	Image[] abilityDocImages;
	int numAbilities;
	int selectedAbility;
	bool rotating;
	Vector3[] startPos = new Vector3[5];
	Vector3[] targetPos = new Vector3[5];


	void Start () {
		numAbilities = position.Length;
		abilityDocImages = this.gameObject.GetComponentsInChildren<Image>();
		foreach(Image icon in abilityDocImages){
			icon.enabled = false;
		}
		for(int i = 0; i < numAbilities; i++){
			position[i] = i;
		}
		selectedAbility = position [2];
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
		if (Input.GetKey(KeyCode.Tab)) {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				newPosUp ();
				//rotateDown ();
				StartCoroutine(rotateCoro());
				//rotate();
				//rotating = true;
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				newPosDown ();
				//rotateUp ();
				StartCoroutine(rotateCoro());
				//rotate();
				//rotating = true;
			}
		}
		if (rotating) {
			for (int i = 0; i < numAbilities; i++) {
				abilities [i].transform.position = Vector3.Lerp (new Vector3 (abilities [i].transform.position.x, startPosY [i], 0), new Vector3 (abilities [i].transform.position.x, targetPosY [i], 0), lerpTime * Time.deltaTime);
			}
		}
	}
	
	void newPosDown(){
		for(int i = 0; i < position.Length; i++){
			startPosY[position[i]] = getPos(i);
			position[i] = modulo(position[i] - 1, 5);
			targetPosY[position[i]] = getPos(i);
			startPos[i] = new Vector3(abilities [i].transform.position.x, startPosY[position[i]], 0);
			targetPos[i] = new Vector3(abilities [i].transform.position.x, targetPosY[position[i]], 0);
		}
	}
	
	void newPosUp(){
		for(int i = 0; i < position.Length; i++){
			startPosY[position[i]] = getPos(i);
			position[i] = modulo(position[i] + 1, 5);
			targetPosY[position[i]] = getPos(i);
			startPos[i] = new Vector3(abilities [i].transform.position.x, startPosY[position[i]], 0);
			targetPos[i] = new Vector3(abilities [i].transform.position.x, targetPosY[position[i]], 0);
		}
	}

	IEnumerator rotateCoro(){

			for (int i = 0; i < numAbilities; i++) {
				
				//abilities [i].transform.position = Vector3.Lerp (new Vector3 (abilities [i].transform.position.x, startPosY [i], 0), new Vector3 (abilities [i].transform.position.x, targetPosY [i], 0), lerpTime * Time.deltaTime);
				abilities [i].transform.position = Vector3.Lerp(startPos[i], targetPos[i], lerpTime);
				//abilities[i].transform.Translate(0, 80, 0);
				yield return null;
				//yield return new WaitForSeconds(waitTimeCoro);
		}
		//yield return null;
		//yield return new WaitForSeconds(waitTimeCoro);
	}

	void rotateUp(){
		for (int i = 0; i < numAbilities; i++) {
			if(position[i] == 4){
				abilities[i].rectTransform.Translate(0,320,0);
			}
			else{
				abilities[i].rectTransform.Translate(0,-80,0);
			}
		}
	}

	void rotateDown(){
		for (int i = 0; i < numAbilities; i++) {
			if (position [i] == 0) {
				abilities [i].rectTransform.Translate (0, -320, 0);
			} else {
				abilities [i].rectTransform.Translate (0, 80, 0);
			}
		}
	}

	void rotate(){
		for (int i = 0; i < numAbilities; i++) {
			abilities [i].transform.position = new Vector3 (abilities [i].transform.position.x, targetPosY [i], 0);
		}
	}

	int modulo(int a, int b){
		return (a%b + b)%b;
	}

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
