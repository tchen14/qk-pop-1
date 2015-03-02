using UnityEngine;
using System.Collections;

public class TestScriptLeft : MonoBehaviour {


    [EventField]
    public float timer = 0;
    [EventField]
    public float delay = 1;

    [EventField]
    public int counter = 5;
    [EventField]
    public int fieldA = 5;
    [EventField]
    public int fieldB = 5;

    void Start(){
        object[] obj = new object[] { counter };
        this.GetType().GetMethod("Test").Invoke(this, obj);
    }

    public void Test(int value){
        print(value);
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