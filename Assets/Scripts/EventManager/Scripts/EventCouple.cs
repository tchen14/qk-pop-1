using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventCouple : MonoBehaviour {

    public MonoBehaviour conditionScript;
    public string conditionField;
    public System.Type conditionType;
    public int conditionIndex;
    public int conditionInt;
    public float conditionFloat;

    public MonoBehaviour actionScript;
    public string actionName;
    public System.Type actionType;
    public int actionIndex;
    public int actionInt;
    public Vector3 actionVector3;

    void Start() {
        EventListener.AddCouple(this);
    }

    private float timer = 0;
    public float delay = 1;

    void Update() {
        if (timer < delay) {
            timer += Time.deltaTime;
        }
        if (timer >= delay) {
            timer = 0;
            EventListener.SlowUpdate(this);
        }
    }
}