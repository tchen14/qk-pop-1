using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

public class InputSerialization {

	//Save keyboard input to PlayerPrefs
	public static void SaveInput(Dictionary<string, string> inputs) {
		foreach(string s in inputs.Keys)
			PlayerPrefs.SetString(s, inputs[s]);
	}

	//Load keyboard input from PlayerPrefs
	public static Dictionary<string, string> LoadInput() {
		Dictionary<string, string> inputs = new Dictionary<string, string>();
		inputs.Add ("forward", PlayerPrefs.GetString("forward"));
		inputs.Add ("backward", PlayerPrefs.GetString("backward"));
		inputs.Add ("left", PlayerPrefs.GetString("left"));
		inputs.Add ("right", PlayerPrefs.GetString("right"));
		inputs.Add ("action", PlayerPrefs.GetString("action"));
		inputs.Add ("sprint", PlayerPrefs.GetString("sprint"));
		inputs.Add ("crouch", PlayerPrefs.GetString("crouch"));
		inputs.Add ("cover", PlayerPrefs.GetString("cover"));
		inputs.Add ("climb", PlayerPrefs.GetString("climb"));
		inputs.Add ("jump", PlayerPrefs.GetString("jump"));
		inputs.Add ("target", PlayerPrefs.GetString("target"));
		inputs.Add ("cameraReset", PlayerPrefs.GetString("cameraReset"));
		inputs.Add ("abilityEquip", PlayerPrefs.GetString("abilityEquip"));
		inputs.Add ("notifications", PlayerPrefs.GetString("notifications"));
		inputs.Add ("compass", PlayerPrefs.GetString("compass"));
		inputs.Add ("journal", PlayerPrefs.GetString("journal"));
		inputs.Add ("qAbility1", PlayerPrefs.GetString("qAbility1"));
		inputs.Add ("nextAbility", PlayerPrefs.GetString("nextAbility"));
		inputs.Add ("previousAbility", PlayerPrefs.GetString("previousAbility"));
		inputs.Add ("nextTarget", PlayerPrefs.GetString("nextTarget"));
		inputs.Add ("previousTarget", PlayerPrefs.GetString("previousTarget"));
		inputs.Add ("pause", PlayerPrefs.GetString("pause"));

		return inputs;
	}

}
