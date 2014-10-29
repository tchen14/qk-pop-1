using UnityEngine;
using System.Collections;

public class AI_state : MonoBehaviour {

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        //If the AI has a target already
        if (GetComponent<AI_main>().AI_seesTarget && GetComponent<AI_main>().AI_attacking == false) {
        }
        //if the AI needs to be looking for a target
        else {
            //GetComponent<AI_sight>().CheckForTargets();
        }
    }
}
