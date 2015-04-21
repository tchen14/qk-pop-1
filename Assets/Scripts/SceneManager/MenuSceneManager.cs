using UnityEngine;
using System.Collections;

public class MenuSceneManager : SceneManager {
	
	public override void LoadNextExpectedScene(){
		MasterSceneManager.Instance.QuickLoadLevel("PoP");
	}
}
