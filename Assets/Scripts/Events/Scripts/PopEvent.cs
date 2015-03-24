using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopEvent : MonoBehaviour {

    public EventCouple couple;

    public bool checkConditions = true;
    public bool regional = false;
    public bool runOnce = true;

    public enum AndOrCompare { All, OneOrMore, OnlyOne }
    public AndOrCompare andOrCompare = AndOrCompare.All;

    private float timer = 0;
    public float delay = 1;

    void Update() {
        if (checkConditions == false) { return; }

        if (timer < delay) {
            timer += Time.deltaTime;
        }
        if (timer >= delay) {
            timer = 0;
            EventListener.SlowUpdate(couple);
        }
    }
}
