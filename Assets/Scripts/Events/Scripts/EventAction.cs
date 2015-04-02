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
    public Vector3 p_Vector3;
    public GameObject p_GameObject;
    public MonoBehaviour p_MonoBehaviour;

    //  Function Arguments
    public object[] args;

    public enum ExecuteType { ChooseAnAction, ExecuteFunction, ActivateNextEvent, DebugMessage }
    public ExecuteType executeType;

    //  Called by EventManagerEditor with reflected parameter
    public object[] SetParameters(System.Type type){
        if (type == typeof(System.Int32))       { return new object[] { p_int }; }
        else if (type == typeof(float))         { return new object[] { p_float }; }
        else if (type == typeof(string))        { return new object[] { p_string }; }
        else if (type == typeof(Vector3))       { return new object[] { p_Vector3 }; }
        else if (type == typeof(GameObject))    { return new object[] { p_GameObject }; }
        else if (type == typeof(MonoBehaviour)) { return new object[] { p_MonoBehaviour }; }
        else { return null; }
    }
}