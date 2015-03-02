using UnityEngine;
using System.Collections;

public class TestScriptRight : MonoBehaviour {

    public GameObject CubePrefab;

    public int listenInt() { return 0; }

    [EventMethod]
    public void NullFunction() {
        print(gameObject.name + " NullFunction was called");
    }

    [EventMethod]
    public void IntFunction(int value) {
        print(gameObject.name + " IntFunction was passed " + value);
    }

    [EventMethod]
    public void FloatFunction(float value) {
        print(gameObject.name + " FloatFunction was passed " + value);
    }

    [EventMethod]
    public void VectorFunction(Vector3 value) {
        //Instantiate(CubePrefab, value, Quaternion.identity);
        print(gameObject.name + " VectorFunction was passed " + value);
    }

    [EventMethod]
    public void GameObjectFunction(GameObject value) {
        print(gameObject.name + " GameObjectFunction was passed " + value.name);
    }
}