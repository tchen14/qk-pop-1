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

	int numAbilities;
	bool rotating;

	void Start () {
		numAbilities = position.Length;
		for(int i = 0; i < numAbilities; i++){
			position[i] = i;
		}
	}
	
	void Update () {
		if(Input.GetKeyDown (KeyCode.DownArrow)){
			newPosUp();
			//StartCoroutine(rotateCoro());
			rotate();
			//rotating = true;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			newPosDown ();
			//StartCoroutine(rotateCoro());
			rotate();
			//rotating = true;
		}
		while (rotating) {
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
		}
	}
	
	void newPosUp(){
		for(int i = 0; i < position.Length; i++){
			startPosY[position[i]] = getPos(i);
			position[i] = modulo(position[i] + 1, 5);
			targetPosY[position[i]] = getPos(i);
		}
	}

	IEnumerator rotateCoro(){

			for (int i = 0; i < numAbilities; i++) {
				//abilities [i].transform.position = Vector3.Lerp (new Vector3 (abilities [i].transform.position.x, startPosY [i], 0), new Vector3 (abilities [i].transform.position.x, targetPosY [i], 0), lerpTime * Time.deltaTime);
			//abilities[i].transform.Translate(0, 80, 0);
			abilities[i].rectTransform.Translate(0,80,0);
				yield return new WaitForSeconds(0.24f);
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
}
