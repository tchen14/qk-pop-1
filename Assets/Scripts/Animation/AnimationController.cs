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
	int turnRight;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();	
	}
	
	void FixedUpdate () 
	{
		//If the player moves vertically, assign the integer value from the input to the movement parameter from the animation controller
		if(InputManager.input.MoveVerticalAxis() != 0) {
			movement = (int)InputManager.input.MoveVerticalAxis();
			turnRight = 0;
		//If the player moves horizontally, assign the integer value from the input to the turnRight parameter from the animation controller
		} else if(InputManager.input.MoveHorizontalAxis() != 0) {
			turnRight = (int)InputManager.input.MoveHorizontalAxis();
			movement = (int)InputManager.input.MoveHorizontalAxis(); ;
		} else {
		//If the player don't move vertically or horizontally, the parameter will be set to zero so no 'movement' animation will occur
			movement = 0;
			turnRight = 0;
		}
		//When it is supposed to run, 0.5 is a temporary value until finds something better
		if((InputManager.input.MoveVerticalAxis() > 0.5f) || (InputManager.input.MoveVerticalAxis() < -0.5f) || (InputManager.input.MoveHorizontalAxis() > 0.5f) || (InputManager.input.MoveHorizontalAxis() < -0.5f)) {
			running = true;
		} else {
			running = false;
		}
		//Set the values of the parameters from the animation controller
		jumping = Input.GetButton("Jump");
		crouching = InputManager.input.isCrouched();
		sprinting = InputManager.input.isSprinting();
		animator.SetInteger("Movement", movement);
        animator.SetBool("Jump", jumping);
        animator.SetBool("Crouch", crouching);
        animator.SetBool("isSprinting", sprinting);
        animator.SetBool("isRunning", running);
		animator.SetInteger("TurnRight", turnRight);
	}
}
