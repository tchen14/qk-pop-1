using UnityEngine;
using System.Collections;

[EventVisible]
public class TestScriptLeft : MonoBehaviour {

    [EventVisible]
    public float timer = 0;
    [EventVisible]
    public float delay = 1;

    [EventVisible]
    public int counter = 5;
    [EventVisible]
    public int fieldA = 5;
    [EventVisible]
    public int fieldB = 5;
    [EventVisible]
    public bool trueFalse = false;

    [EventVisible]
    public Vector3 vectorA= Vector3.zero;

    void Start(){
        object[] obj = new object[] { counter };
        this.GetType().GetMethod("Test").Invoke(this, obj);
    }

    public void Test(int value){
        //print(value);
    }

    void OnTriggerEnter() {
        EventListener.Report(this, "Enter");
    }

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
        EventListener.Report(this, "Press One");
    }

    [EventVisible]
    public void FunctionOne() {
        print("FunctionOne was called");
    }

    [EventVisible]
    public void FunctionTwo() {
        print("FunctionTwo was called");
    }
}