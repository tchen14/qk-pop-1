using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    public List<EventCouple> couples;

    private float timer = 0;
    public float delay = 1;

    void Update() {

        if (timer < delay) {
            timer += Time.deltaTime;
        }
        if (timer >= delay) {
            timer = 0;
            foreach (var c in couples) {
                EventListener.SlowUpdate(c);
            }
        }
    }
}
