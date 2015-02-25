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

    public int comparisonIndex;
    public string[] comparisonOperators = new string[] { "==", ">", "<" };

    public enum IntCompareOption { 

        EqualTo = 1, GreaterThan = 2, LessThan = 3
    }
    public IntCompareOption intCompareOption;
}