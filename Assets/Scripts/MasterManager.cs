using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class MasterManager{
	//sealed attribute prevents other classes from inheriting this class

	#region singletonEnforcement 
	private static readonly MasterManager instance = new MasterManager();
	//readonly keyword modifier prevents variable from being writen except on declaration
	
	public static MasterManager Instance{
		get {
			return instance;
		}
	}
	#endregion

	public static Dictionary<string,bool> scenes = new Dictionary<string,bool>();
	private MasterManager(){
		MasterManager.scenes.Add ("menu", true);
		MasterManager.scenes.Add ("game", false);
	}
}
