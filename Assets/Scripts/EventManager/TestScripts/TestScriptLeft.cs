using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class TestScriptLeft : MonoBehaviour {

    [EventField]
    public float timer = 0;
    private float delay = 1;

    [EventField]
    public int counter = 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            PressOne();

            counter++;
        }
        
        if (timer < delay) {
            timer += Time.deltaTime;
            if (timer >= delay) {
                timer = 0;
            }
        }
    }

    public void PressOne() {
        EventListener.Report(this);
    }

    [EventMethod]
    public void FunctionOne() {
        print("FunctionOne was called");
    }

    [EventMethod]
    public void FunctionTwo() {
        print("FunctionTwo was called");
    }
}