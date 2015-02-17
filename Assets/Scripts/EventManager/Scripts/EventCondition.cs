using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class EventCondition {

    public MonoBehaviour conditionScript;
    public string conditionField;
    public System.Type conditionType;
    public int conditionIndex;
    public int conditionInt;
    public float conditionFloat;

}