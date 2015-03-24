using UnityEngine;
using System.Collections;

[System.Serializable]
public class EventCondition {

    public MonoBehaviour conditionScript;
    public string conditionField;
    public System.Type conditionType;
    public int conditionIndex;
    public int conditionInt;
    public float conditionFloat;
    public Vector3 conditionVector3;

    public enum WatchType { WatchScript }
    public WatchType watchType;
    
    public enum NumberCompareOption { EqualTo, GreaterThan, LessThan }
    public NumberCompareOption numberCompareOption;

    public enum VectorCompareOption { NearerThan, FurtherThan }
    public VectorCompareOption vectorCompareOption;
}