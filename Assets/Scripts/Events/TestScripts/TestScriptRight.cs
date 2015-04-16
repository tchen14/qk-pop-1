using UnityEngine;
using System.Collections;

public class TestScriptRight : MonoBehaviour {

    public GameObject CubePrefab;

    public int listenInt() { return 0; }

    [EventVisible]
    public void NullFunction() {
        print(gameObject.name + " NullFunction was called");
    }

    [EventVisible]
    public void IntFunction(int value) {
        print(gameObject.name + " IntFunction was passed " + value);
    }

    [EventVisible]
    public void FloatFunction(float value) {
        print(gameObject.name + " FloatFunction was passed " + value);
    }

    [EventVisible]
    public void VectorFunction(Vector3 value) {
        //Instantiate(CubePrefab, value, Quaternion.identity);
        print(gameObject.name + " VectorFunction was passed " + value);
    }

    [EventVisible]
    public void GameObjectFunction(GameObject value) {
        print(gameObject.name + " GameObjectFunction was passed " + value.name);
    }
}

[EventVisible("Test Static Class")]
public static class TestScriptRightStatic {
    [EventVisible("Print \\\"Debug\\\"")]
    static public void GameObjectFunction() {
        MonoBehaviour.print("Debug");
    }
}