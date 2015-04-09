using UnityEngine;
using System.Collections;

public class MenuSceneManager : SceneManager {
	
	public override void LoadScene(){
		MasterSceneManager.Instance.QuickLoadLevel("PoP");
	}
}
