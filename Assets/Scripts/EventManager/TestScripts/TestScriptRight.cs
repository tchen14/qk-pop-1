using UnityEngine;
using System.Collections;

public class TestScriptRight : MonoBehaviour {

    public int listenInt() { return 0; }

    [EventMethod]
    public void NullFunction() {
        print(gameObject.name + " NullFunction was called");
    }

    [EventMethod]
    public void IntFunction(int value) {
        print(gameObject.name + " RightTestFunctionTwo was passed " + value);
    }

    [EventMethod]
    public void VectorFunction(Vector3 value) {
        print(gameObject.name + " RightTestFunctionTwo was passed " + value);
    }
}