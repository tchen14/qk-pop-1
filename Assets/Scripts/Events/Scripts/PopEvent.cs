using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopEvent : MonoBehaviour {

    public string uniqueId;

    public EventCouple couple;

    public bool isActive = true;
    public bool isRegional = false;
    public bool executeOnce = true;

    private PopEvent nextEvent;
    private int index = 0;

    public float totalTimeActive = 0;
    private float timer = 0;
    public float delay = 1;

    public float regionRadius = 1;
    public bool drawRegionTwo = false;
    public Vector3 conditionRegionCenter = new Vector3(0, 0, 0);
    public float conditionRegionRadius = 1;

    void Awake() {
        PopEvent[] popEvents = gameObject.GetComponents<PopEvent>();
        for (int i = 0; i < popEvents.Length - 1; i++) { //  Don't check the last element
            if (this.Equals(popEvents[i])) {
                index = i;
                nextEvent = popEvents[i + 1];
                break;
            }
        }

        foreach (EventCondition condition in couple.conditions){
            if (condition.watchType == "Player Enters Area" || condition.watchType == "Player Leaves Area") {
                SphereCollider newCollider = gameObject.AddComponent<SphereCollider>();
                newCollider.radius = couple.popEvent.conditionRegionRadius;
                newCollider.isTrigger = true;
                break;
            }
        }
    }

    void Update() {
        if (isActive == false) { return; }

        totalTimeActive += Time.deltaTime;

        if (timer < delay) {
            timer += Time.deltaTime;
        }
        if (timer >= delay) {
            timer = 0;
            EventListener.SlowUpdate(couple);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (isActive == false) { return; }
        if (other.gameObject.GetComponent<PoPCharacterController>()) {
            EventListener.SlowUpdate(couple);
        }
    }
    void OnTriggerExit(Collider other) {
        if (isActive == false) { return; }
        if (other.gameObject.GetComponent<PoPCharacterController>()) {
            EventListener.SlowUpdate(couple);
        }
    }

    void OnDrawGizmosSelected() {
        // The other portion of the custom Editor graphics are found in PopEventEditor.cs
        if (isRegional == true) {
        }

        if (drawRegionTwo) {
            Gizmos.color = new Color(0, 1, 0, 0.3F);
            Gizmos.DrawSphere(transform.position, conditionRegionRadius);
        }
    }

    public void ActivateNextEvent() {
        if (nextEvent != null) {
            nextEvent.isActive = true;
        }
    }

    public void Deactivate() {
        isActive = false;
    }
}