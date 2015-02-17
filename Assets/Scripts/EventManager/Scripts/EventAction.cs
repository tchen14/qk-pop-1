using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class EventAction {

    public MonoBehaviour actionScript;
    public string actionName;
    public System.Type actionType;
    public int actionIndex;
    public int actionInt;
    public Vector3 actionVector3;

}