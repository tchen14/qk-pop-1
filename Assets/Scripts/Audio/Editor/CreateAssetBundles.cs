using UnityEngine;
using System.Collections;
using UnityEditor;
//Script taken from Unity http://docs.unity3d.com/Manual/BuildingAssetBundles5x.html
//modified by Ace


//Script must be modified to add deployment to consoles
//!Creates AssetBundles
public class CreateAssetBundles {

	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles() {
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);	//currently only puts bundles in the audio folder
		//BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.XboxOne);			//Build for xbox one, untested
		//BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.PS4);				//Build for PS4, untested
	}
}
