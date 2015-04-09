using UnityEngine;
using System.Collections;

[System.Serializable]
public class EventCondition {

    public MonoBehaviour conditionScript;
    public string conditionField;
    public System.Type conditionType;
    public int conditionIndex;

    //  Potential Parameters
    public int p_int;
    public float p_float;
    public string p_string;
    public Vector3 p_Vector3;
    public Transform p_Transform;

    public string watchType = "Choose A Condition";

    public string watchString;
    public int watchIndex;
    
    public enum NumberCompareOption { EqualTo, GreaterThan, LessThan }
    public NumberCompareOption numberCompareOption;

    public enum VectorCompareOption { NearerThan, FurtherThan }
    public VectorCompareOption vectorCompareOption;
}