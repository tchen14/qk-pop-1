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
    public item_type itemType;
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
				Destroy(this);
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
		cutCompatible = false;
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

	public void Push(){
		switch (itemType) {
		case item_type.Crate:
			if(pushCompatible){
				// --insert behavior here--
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

	public void Pull(){
		switch (itemType) {
		case item_type.Crate:
			if(pullCompatible){
				// --insert behavior here--
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

	private void NoEffect(){
		print("This object is not affected by this ability.");
	}
}




