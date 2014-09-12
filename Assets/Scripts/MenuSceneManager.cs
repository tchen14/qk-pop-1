using UnityEngine;
using System.Collections;

public class MenuSceneManager : SceneManager
{
	
	// Use this for initialization
	void Start()
	{
		if (!MasterManager.Instance.init) {
			MasterManager.Instance.init = true;
			MasterManager.Instance.ToggleLevel("Menu", true);
			MasterManager.Instance.LoadLevels();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
