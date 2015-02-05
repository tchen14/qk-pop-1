using UnityEngine;
using System.Collections;

public class TestScriptLeft : MonoBehaviour, IEventScript {

    public EventTable eventTable() { return new EventTable("Left"); }

    private float timer = 0;
    private float delay = 1;

    public int counter = 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            counter++;
            EventListener.Report(this, "Press One X Times", (int)counter);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            EventListener.Report(this, "Press Two");
        }
        
        if (timer < delay) {
            timer += Time.deltaTime;
            if (timer >= delay) {
                timer = 0;
                EventListener.Report(this, "Every One Second");
            }
        }
    }

    [MethodEvent]
    public void FunctionOne() {
        print("FunctionOne was called");
    }

    [MethodEvent]
    public void FunctionTwo() {
        print("FunctionTwo was called");
    }
}