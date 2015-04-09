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
	public virtual int VerticalValue(string keyPressed) {
		return 0;
	}
	public virtual int HorizontalValue(string keyPressed) {
		return 0;
	}
	public virtual bool CancelPressed() {
		return false;
	}
	public virtual bool ActionPressed() {
		return false;
	}
	public virtual bool SprintPressed() {
		return false;
	}
	public virtual bool JumpPressed() {
		return false;
	}
	public virtual bool CrouchPressed() {
		return false;
	}
	public virtual bool isTargetPressed() {
		return false;
	}
	public virtual bool isCameraReset() {
		return false;
	}
	public virtual bool isQAbility1() {
		return false;
	}
	public virtual bool isQAbility2() {
		return false;
	}
}
