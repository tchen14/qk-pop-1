using UnityEngine;
using System.Collections;

public class PoPSceneManager : SceneManager {

    void Start() {
        //PoPEventManager instance = new PoPEventManager();
    }

    //! testing code until we get Scaleform UI fully working \todo: implement scaleform UI and delete this function
    void OnGUI () {
        // Make a background box
        GUI.Box(new Rect(10,10,100,90), "Testing Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(20,40,80,20), "Load Menu")) {
        	MasterManager.Instance.QuickLoadLevel ("Menu");
        }
    }
}
