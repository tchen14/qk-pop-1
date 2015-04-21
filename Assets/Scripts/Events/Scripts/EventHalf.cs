using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventHalf {

    //  Things which are set in the editor
    public string e_categoryString = "System";  //!< The top level choice - A custom category
    public int e_categoryIndex;   //!<    Assigned via popup
    public string e_classString = "System";  //!< Second level choice - A class or script
    public int e_classIndex;   //!<    Assigned via popup
    public string e_fieldString   = "Choose An Action";   //!< Third level choice - a function, field, or custom test
    public int e_fieldIndex;    //!< Assigned via popup
    public System.Type e_fieldType;

    //  Target options. Typically dragged into the editor
    public MonoBehaviour e_MonoBehaviour;
    public GameObject e_GameObject;

    private const int i = 5;    // Length of parameter arrays
    //  Potential Parameters to pass or watch
    public int[] p_int = new int[i];
    public bool[] p_bool = new bool[i];
    public float[] p_float = new float[i];
    public string[] p_string = new string[i];
    public Vector3[] p_Vector3 = new Vector3[i];
    public Transform[] p_Transform = new Transform[i];
    public GameObject[] p_GameObject = new GameObject[i];
    public MonoBehaviour[] p_MonoBehaviour = new MonoBehaviour[i];

    public string[] numberCompareString = new string[] { "Equal To", "Greater Than", "Less Than" };
    public string[] vectorCompareString = new string[] { "Nearer Than", "Further Than" };
    public string[] boolCompareString = new string[] { "Is False", "If True" };
    public string[] stringCompareString = new string[] { "Equal To", "Different Than" };
    public string compareString;
    public int compareIndex;

    public Color color = new Color(0, 0.58f, 0.69f, 0.45f);
    public int editorHeight = 20;

    //  Function Arguments
    public object[] args;

    //  Called by EventManagerEditor with reflected parameter
    public object[] SetParameters(System.Type[] type) {
        if (type.Length == 0){ 
            return null; 
        }

        List<object> objects = new List<object>();
        for (int i = 0; i < type.Length; i++) {
            if (type[i] == typeof(System.Int32)) { objects.Add(p_int[i]); }
            else if (type[i] == typeof(float)) { objects.Add(p_float[i]); }
            else if (type[i] == typeof(string)) { objects.Add(p_string[i]); }
            else if (type[i] == typeof(Vector3)) { objects.Add(p_Vector3[i]); }
            else if (type[i] == typeof(GameObject)) { objects.Add(p_GameObject[i]); }
            else if (type[i] == typeof(MonoBehaviour)) { objects.Add(p_MonoBehaviour[i]); }
        }

        return objects.ToArray();
    }

    public void SetParameters() {
        System.Type[] paramType = new System.Type[] { typeof(void) };

        if (e_categoryString == "Object Script" && e_MonoBehaviour != null) {
            System.Reflection.ParameterInfo[] par = e_MonoBehaviour.GetType().GetMethod(e_fieldString).GetParameters();
            if (par.Length > 0) {
                paramType = new System.Type[par.Length];
            }

            for (int i = 0; i < par.Length; i++) {
                paramType[i] = par[i].ParameterType;
            }
        }
        else if (e_categoryString == "Static Script") {
            if (EventLibrary.staticClasses[e_classString].GetMethod(e_fieldString) != null) {
                System.Reflection.ParameterInfo[] par = EventLibrary.staticClasses[e_classString].GetMethod(e_fieldString).GetParameters();
                if (par.Length > 0) {
                    paramType = new System.Type[par.Length];
                }

                for (int i = 0; i < par.Length; i++) {
                    paramType[i] = par[i].ParameterType;
                }
            }
        }

        args = SetParameters(paramType);
    }
}