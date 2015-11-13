using UnityEngine;
using System.Collections;

public enum item_type {
	Crate,
	Rope,
	Well,
	Enemy
};

[RequireComponent (typeof (Collider))]
[RequireComponent(typeof (Targetable))]
[System.Serializable]
//public abstract class Item : MonoBehaviour
public class Item : MonoBehaviour
{
	public enum push_type {
		Free,
		TwoAxis,
		FourAxis,
		HeightAxis,
		Anim
	};

    public item_type itemType;
	public push_type current_push_type;
	private Vector3 push_forward_direction = Vector3.forward;
	public int itemIndex;
    [EventVisible("Pushed X Times")]
    public int pushCounter;
    [EventVisible("Pulled X Times")]
    public int pullCounter;
    [EventVisible("Cut X Times")]
    public int cutCounter;
    [EventVisible("Sound Thrown X Times")]
    public int soundThrowCounter;
    [EventVisible("Stunned X Times")]
    public int stunCounter;
    [EventVisible("Affected by QuinC")]
    public bool quincAffected = false;
	public bool pushCompatible = false;
	public bool pullCompatible = false;
	public bool cutCompatible = false;
	public bool soundThrowCompatible = false;
	public bool stunCompatible = false;
	public bool heatCompatible = false;
	public bool coldCompatible = false;
	public bool blastCompatible = false;
	public float soundThrowRadius = 25f;
	
	//Crate variables
	public Vector3 startPosition;		//starting location of the crate
	public float pushPullRadius = 25f;	//maximum movement radius before crate snaps back to startPosition
	public float moveDist;				//distance moved by the crate
	public bool hasMoved;				//flag to track if crate has been moved by Quinc 
	public bool isSnapping;				//flag tracking if crate is currently snapping back
	Color originalColor;				//store the original color of the object while executing quinc functions
	float colorMax;						//maximum length of time the gameObject can remain a color other than the orginal color
	public float colorTime;				//the last time that the gameObject color was changed
	

	//Enemy variables
	public const float stunPeriod = 10.0f;
	public float curStunTimer;
	public bool stunState;
	public bool soundThrowAffected;


    //!Player gathers X number of this item, (kill all enemies in area, use other item script to auto drop item into play inventory)
    protected virtual void GatherObjective (int count)
	{
		return;
	}

    //!Player arrives at the item, option with timed, use negative number for stall quest
    protected virtual void ArriveObjective (Vector3 buffer, float timer = 0.0f)
	{
		return;
	}

    //!This item is brought to target location
    protected virtual void EscortObjective (Vector3 location, Vector3 buffer)
	{
		return;
	}

	public void Stun(float time)
	{
		switch (itemType) {
		case item_type.Crate:
			if(stunCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;

		case item_type.Rope:
			if(stunCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;

		case item_type.Well:
			if(stunCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;

		case item_type.Enemy:
			StartCoroutine (_Stun (time));
			break;
		}
	}

	public void Cut(){
		switch (itemType) {
		case item_type.Crate:
			if(cutCompatible){
				print("Crate was destroyed by cut.");
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(cutCompatible){
				print("Rope was cut.");
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(cutCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(cutCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			break;
		}
		//cutCompatible = false;
	}

	public void SoundThrow(){
		switch (itemType) {
		case item_type.Crate:
			if(soundThrowCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(soundThrowCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(soundThrowCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(soundThrowCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
		}
	}

	public void Push(Vector3 player_position, float push_force){
		switch (itemType) {
		case item_type.Crate:	
			if(pushCompatible){
				crate_pushPull(player_position, push_force, current_push_type, false);
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(pushCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(pushCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(pushCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
		}
	}

	public void Pull(Vector3 player_position, float push_force){
		switch (itemType) {
		case item_type.Crate:
			if(pullCompatible){
				crate_pushPull(player_position, push_force, current_push_type, true);
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(pullCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(pullCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(pullCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
		}
	}

	public void Heat(){
		switch (itemType) {
		case item_type.Crate:
			if(heatCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(heatCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(heatCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(heatCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
		}
	}

	public void Cool(){
		switch (itemType) {
		case item_type.Crate:
			if(coldCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(coldCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(coldCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(coldCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
		}
	}

	public void Blast(){
		switch (itemType) {
		case item_type.Crate:
			if(blastCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Rope:
			if(blastCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Well:
			if(blastCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
			
		case item_type.Enemy:
			if(blastCompatible){
				// --insert behavior here--
			}
			else {
				NoEffect();
			}
			break;
		}
	}

	private void start_stun(){
		stunState = true;
		// Play Animation and Sound for enemy being stunned
		// Laying Enemy sideways for now
		Vector3 tempAngles = transform.eulerAngles;
		tempAngles.x = 90.0f;
		transform.eulerAngles = tempAngles;
	}
	
	private void end_stun(){
		stunState = false;
		Vector3 tempAngles = transform.eulerAngles;
		tempAngles.x = 0.0f;
		transform.eulerAngles = tempAngles;
		StopCoroutine ("_Stun");
	}

	IEnumerator _Stun(float time){
		start_stun ();
		yield return new WaitForSeconds(time);
		end_stun ();
	}

	private void crate_pushPull(Vector3 player_pos, float magnitude, push_type type, bool pull){
		Vector3 heading = new Vector3(0.0f, 0.0f, 0.0f);
		float angle = 0.0f;
		Vector3 pos = Vector3.zero;

		switch (type) {
		case push_type.Free:
			heading = gameObject.transform.position - player_pos;
			break;
		case push_type.TwoAxis:
			heading = gameObject.transform.position - player_pos;
			angle = Vector3.Angle(heading, Vector3.forward);
			if(angle > 0.0f && angle <= 90.0f) heading = push_forward_direction;	
			else if(angle > 90.0f && angle <= 180.0f) heading = -push_forward_direction;
			else heading = Vector3.zero;
			break;
		case push_type.FourAxis:
			heading = gameObject.transform.position - player_pos;
			pos = gameObject.transform.position;
			angle = Vector3.Angle(heading, Vector3.forward);
			if(angle > 0.0f && angle <= 45.0f) heading = Vector3.forward;
			else if(angle > 135.0f && angle <= 180.0f) heading = Vector3.back;
			else if(angle > 45.0f && angle <= 135.0f && pos.x > player_pos.x) heading = Vector3.right;
			else if(angle > 45.0f && angle <= 135.0f && pos.x < player_pos.x) heading = Vector3.left;
			break;
		case push_type.HeightAxis:
			heading = gameObject.transform.position - player_pos;
			pos = gameObject.transform.position;
			angle = Vector3.Angle(heading, Vector3.forward);
			if(angle > 0.0f && angle <= 22.5f) heading = Vector3.forward;
			else if(angle > 22.5f && angle <= 45.0f && pos.x > player_pos.x) heading = new Vector3(-Mathf.Sin(22.5f), 0.0f, -Mathf.Cos(22.5f));
			else if(angle > 22.5f && angle <= 45.0f && pos.x < player_pos.x) heading = new Vector3(Mathf.Sin(22.5f), 0.0f, -Mathf.Cos(22.5f));
			else if(angle > 45.0f && angle <= 135.0f && pos.x > player_pos.x) heading = Vector3.right;
			else if(angle > 45.0f && angle <= 135.0f && pos.x < player_pos.x) heading = Vector3.left;
			else if(angle > 135.0f && angle <= 157.5f && pos.x > player_pos.x) heading = new Vector3(-Mathf.Sin(22.5f), 0.0f, Mathf.Cos(22.5f));
			else if(angle > 135.0f && angle <= 157.5f && pos.x < player_pos.x) heading = new Vector3(Mathf.Sin(22.5f), 0.0f, Mathf.Cos(22.5f));
			else if(angle > 135.0f && angle <= 180.0f) heading = Vector3.back;
			else heading = Vector3.zero;
			break;
		}

		heading.y = 0.0f;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		if (pull) direction *= -1;

		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddForce(direction * (magnitude * 100));
	}

	private void NoEffect(){
		print("This object is not affected by this ability.");
	}
}




