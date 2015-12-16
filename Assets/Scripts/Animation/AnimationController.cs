using UnityEngine;
using System.Collections;
/// <summary>
/// Class created to handle Gavyn animations
/// </summary>
public class AnimationController : MonoBehaviour {
	Animator animator;
    bool jumping;
    bool crouching;
    bool sprinting;
    int movement;
    bool running;
	bool ladder;
	bool sidle;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();	
	}
	
	void FixedUpdate () 
	{
		//If the player moves vertically, assign the integer value from the input to the movement parameter from the animation controller
		if(Input.GetAxis("Vertical") != 0) {
			movement = (int)Input.GetAxis("Vertical");
		//If the player moves horizontally, assign the integer value from the input to the turnRight parameter from the animation controller
		} else if(Input.GetAxis("Horizontal") != 0) {
			movement = (int)Input.GetAxis("Horizontal");
		} else {
		//If the player don't move vertically or horizontally, the parameter will be set to zero so no 'movement' animation will occur
			movement = 0;
		}
		//I am using the speed value from QK_Character_Movement script
		if((QK_Character_Movement.Instance.curSpeed >= QK_Character_Movement.Instance.runSpeed)&& (QK_Character_Movement.Instance._moveState!= QK_Character_Movement.CharacterState.Sprint)) {
			running = true;
		} else {
			running = false;
		}
		//Checks if the character is climbing a ladder or not
		if(QK_Character_Movement.Instance._moveState == QK_Character_Movement.CharacterState.Ladder)
			ladder = true;
		else
			ladder = false;
		//Checks if the character is sidling
		if(QK_Character_Movement.Instance._moveState == QK_Character_Movement.CharacterState.Sidle)
            sidle = true;
		else
			sidle = false;
		//Set the values of the parameters from the animation controller
		jumping = Input.GetButton("Jump");
        crouching = false;
        sprinting = false;
		animator.SetInteger("Movement", movement);
        animator.SetBool("Jump", jumping);
        animator.SetBool("Crouch", crouching);
        animator.SetBool("isSprinting", sprinting);
        animator.SetBool("isRunning", running);
		//animator.SetBool("Ladder", ladder);
		animator.SetBool("Sidle", sidle);
	}
}
