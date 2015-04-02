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
    public Vector3 p_Vector3;
    public Transform p_Transform;

    public enum WatchType { ChooseACondition, WatchScript, PlayerEntersArea, WaitXSeconds }
    public WatchType watchType;
    
    public enum NumberCompareOption { EqualTo, GreaterThan, LessThan }
    public NumberCompareOption numberCompareOption;

    public enum VectorCompareOption { NearerThan, FurtherThan }
    public VectorCompareOption vectorCompareOption;
}