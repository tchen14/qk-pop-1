using UnityEngine;
using System.Collections;

public class AbilityDockAnimations : MonoBehaviour {

	public int[] position;
	public int numAbilities;
	
	void Start () {
		for(int i = 0; i < numAbilities; i++){
			position[i] = i;
		}
	}
	
	void Update () {
		if(Input.GetKeyDown (KeyCode.DownArrow)){
			RotateDown ();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			RotateUp();
		}
	}
	
	void RotateDown(){
		for(int i = 0; i < position.Length; i++){
			position[i] = modulo(position[i] - 1, 5);
			
		}
	}
	
	void RotateUp(){
		for(int i = 0; i < position.Length; i++){
			position[i] = modulo(position[i] + 1, 5);
			
		}
	}
	
	int modulo(int a, int b){
		return (a%b + b)%b;
	}
}
