using UnityEngine;
using System.Collections;

public class PoPSceneManager : SceneManager {

    //! testing code until we get Scaleform UI fully working \todo: implement scaleform UI and delete this function
    void OnGUI () {
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(4,4,32,16), "Menu")) {
        	MasterManager.Instance.QuickLoadLevel ("Menu");
        }
    }
    
    void OnEnable(){
    	Cursor.visible = false;
    }
    
	void OnDisable(){
		Cursor.visible = true;
	}
}
