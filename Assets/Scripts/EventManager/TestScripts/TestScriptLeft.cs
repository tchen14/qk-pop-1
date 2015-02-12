using UnityEngine;
using System.Collections;

public class TestScriptLeft : MonoBehaviour {

    [EventField]
    public float timer = 0;
    private float delay = 1;

    [EventField]
    public int counter = 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            counter++;
        }
        
        if (timer < delay) {
            timer += Time.deltaTime;
            if (timer >= delay) {
                timer = 0;
            }
        }
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