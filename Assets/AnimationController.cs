using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
	Animator animator;
	float travelState;
    bool jumping;
    bool crouching;
    bool sprinting;
    int movement;
    bool running;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
        //a and d buttons
        movement = InputManager.input.MoveVerticalAxis();
        //w and s buttons
        Debug.Log(movement);
        if ((Input.GetAxis("Vertical") > 0.5f) || (Input.GetAxis("Vertical") < -0.5f))
        {
            running = true;
        }
        else
        {
            running = false;
        }
        jumping = InputManager.input.isJumping();
        crouching = InputManager.input.isCrouched();
        sprinting = InputManager.input.isSprinting();
    }

	void FixedUpdate () 
	{
        animator.SetInteger("Movement", movement);
        animator.SetFloat("Blend", travelState);
        animator.SetBool("Jump", jumping);
        animator.SetBool("Crouch", crouching);
        animator.SetBool("isSprinting", sprinting);
        animator.SetBool("isRunning", running);
	}
}
