using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class EventAction {

    public MonoBehaviour actionScript;
    public string actionName;
    public int actionEditorIndex;

    //  Potential Parameters
    public int p_int;
    public float p_float;
    public string p_string;
    public string p_string2;
    public Vector3 p_Vector3;
    public GameObject p_GameObject;
    public MonoBehaviour p_MonoBehaviour;

    //  Function Arguments
    public object[] args;

    public string executeCategory = "System";
    public string executeType = "Choose An Action";

    public string executeString;
    public int executeIndex;

    public string executeCategoryString;
    public int executeCategoryIndex;

    public bool executeStaticFunction = false;


    //  Called by EventManagerEditor with reflected parameter
    public object[] SetParameters(System.Type[] type){
        if (type.Length == 1) {
            if (type[0] == typeof(System.Int32)) { return new object[] { p_int }; }
            else if (type[0] == typeof(float)) { return new object[] { p_float }; }
            else if (type[0] == typeof(string)) { return new object[] { p_string }; }
            else if (type[0] == typeof(Vector3)) { return new object[] { p_Vector3 }; }
            else if (type[0] == typeof(GameObject)) { return new object[] { p_GameObject }; }
            else if (type[0] == typeof(MonoBehaviour)) { return new object[] { p_MonoBehaviour }; }
        }
        else if (type.Length == 2) {
            if (type[0] == typeof(string) && type[1] == typeof(System.Int32)) { return new object[] { p_string, p_int }; }
            if (type[0] == typeof(string) && type[1] == typeof(float)) { return new object[] { p_string, p_float }; }
        }
        return null;
    }
}