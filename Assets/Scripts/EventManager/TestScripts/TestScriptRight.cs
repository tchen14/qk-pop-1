using UnityEngine;
using System.Collections;

public class TestScriptRight : MonoBehaviour, IEventScript {

    public EventTable eventTable() { return new EventTable("Right"); }

    public void NullFunction() {
        print(gameObject.name + " NullFunction was called");
    }

    public void IntFunction(int value) {
        print(gameObject.name + " RightTestFunctionTwo was passed " + value);
    }

    public void VectorFunction(Vector3 value) {
        print(gameObject.name + " RightTestFunctionTwo was passed " + value);
    }
}