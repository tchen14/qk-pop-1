using UnityEngine;
using System.Collections.Generic;

//! Base class for the different input types
public abstract class InputType : MonoBehaviour {

	public virtual string InputString() {
		return "";
	}
	public virtual bool GetKey(string keyPressed) {
		return false;
	}
	public virtual int VerticalAxis() {
		return 0;
	}
	public virtual int HorizontalAxis() {
		return 0;
	}
	public virtual bool isCrouched() {
		return false;
	}
	public virtual bool isSprinting() {
		return false;
	}
	public virtual bool isJumping() {
		return false;
	}
	public virtual bool isActionPressed() {
		return false;
	}
	public virtual int ForwardPressed(string keyPressed) {
		return 0;
	}
}
