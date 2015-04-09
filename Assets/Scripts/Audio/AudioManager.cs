using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using SimpleJSON;
using Debug = FFP.Debug;

/*!
 *	Manages audio for all levels. This class pretty much just loads and plays sounds, on a (timed or untimed) repeat if desired.
 *	\todo implementation + code
 *	written by Ace
 */
public class AudioManager : MonoBehaviour {

	#region singletonEnforcement
	private static AudioManager _instance;

	public static AudioManager instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<AudioManager>();

				//Dont distroy on new scene load
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion

	public string soundListFilePath = "/Resources/Json/soundListData.json";

	public AudioMixer masterMixer;

	Dictionary<string, bool> ambianceDict = new Dictionary<string, bool>();
	Dictionary<string, bool> effectDict = new Dictionary<string, bool>();
	Dictionary<string, bool> musicDict = new Dictionary<string, bool>();
	Dictionary<string, bool> voiceDict = new Dictionary<string, bool>();


	//! Unity Start function
    void Start() {
		Debug.Warning("audio", "SoundManager has started");
		#region singletonCreation
		if(_instance == null) {
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		} else {
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}
		#endregion

		if(!loadListFromFile(soundListFilePath)) {
			Debug.Error("audio", "JSON file did not load");
		} else
			Debug.Warning("audio", "JSON file loaded");


	}

	//! Unity Update function
	//void Update() {
	//}

	#region json file reading
	//!checks JSON file exists and loads it
	public bool loadListFromFile(string path) {		//checks to make sure json file is there
		if(!System.IO.File.Exists(Application.dataPath + path)) {
			Debug.Error("audio", "File does not exist: " + path);
			return false;
		}
		string json = System.IO.File.ReadAllText(Application.dataPath + path);
		return loadListFromJson(json);
	}

	//!checks JSON file isnt empty and loads data into audio dictionaries
	public bool loadListFromJson(string json) {		//loads json data into
		JSONNode soundData = JSON.Parse(json);
		//print(soundData);
		if(soundData == null) {						//there is no json data
			Debug.Error("audio", "Json file is empty");
			return false;
		}
		
		JSONNode audioNode = soundData["audio"];
		string[] types = { "Ambiance", "Effect", "Music", "Voice" };

		foreach(string type in types) {
			JSONArray tempArray = audioNode[type].AsArray;
			foreach(JSONNode j in tempArray) {
				var name = j[0];				//name of object
				bool priority = j[1].AsBool;	//priority of object
				addSound(type, name, priority);
			}
		}
		return true;
	}
	#endregion 
	
	//!Adds sound info to Dictionaries
	public void addSound(string type, string name, bool priority){
			switch(type) {
				case "Ambiance":
					ambianceDict.Add(name, priority);
					break;
				case "Effect":
					effectDict.Add(name, priority);
					break;
				case "Music":
					musicDict.Add(name, priority);
					break;
				case "Voice":
					voiceDict.Add(name, priority);
					break;
				default:
					Debug.Log("audio", type + "is not an option, or you spelled it wrong");
					break;
			}
		} 


	//!checks if sounds in dictionary
	public bool findSound(string type, string name) {
		switch(type) {
			case "Ambiance":
				if(ambianceDict.ContainsKey(name))
					return true;
				else
					return false;
			case "Effect":
				if(effectDict.ContainsKey(name))
					return true;
				else
					return false;
			case "Music":
				if(musicDict.ContainsKey(name))
					return true;
				else
					return false;
			case "Voice":
				if(voiceDict.ContainsKey(name))
					return true;
				else
					return false;
			default:
				Debug.Log("audio", type + "is not an option, or you spelled it wrong");
				return false;
		}
	}
	


	#region Sound volume
	[EventVisible]
	//!Change volume for groups in MasterMixer
	public void changeVol(string name, float level){
		switch(name){
			case "Master":
				masterMixer.SetFloat ("MasterVol", level);
				break;
			case "Ambiance":
				masterMixer.SetFloat ("MaxAmbianceVol", level);
				break;
			case "Effect":
				masterMixer.SetFloat ("MaxEffectVol", level);
				break;
			case "Music":
				masterMixer.SetFloat ("MaxMusicVol", level);
				break;
			case "Voice":
				masterMixer.SetFloat ("MaxVoiceVol", level);
				break;
			case "Reset":
				masterMixer.ClearFloat ("MasterVol");
				masterMixer.ClearFloat ("MaxAmbianceVol");
				masterMixer.ClearFloat ("MaxEffectVol");
				masterMixer.ClearFloat ("MaxMusicVol");
				masterMixer.ClearFloat ("MaxVoiceVol");
				break;
			default:
				Debug.Log("audio", name + "is not an option, or you spelled it wrong");
				break;
		}

	}

	//!Check the current volume levels in MasterMixer
	public float seeVol(string name){
		float level = 100;
		switch(name){
			case "Master":
				masterMixer.GetFloat ("MasterVol", out level);
				return level;
			case "Ambiance":
				masterMixer.GetFloat ("MaxAmbianceVol", out level);
				return level;
			case "Effect":
				masterMixer.GetFloat ("MaxEffectVol", out level);
				return level;
			case "Music":
				masterMixer.GetFloat ("MaxMusicVol", out level);
				return level;
			case "Voice":
				masterMixer.GetFloat ("MaxVoiceVol", out level);
				return level;
			default:
				Debug.Log("audio", name + "is not an option, or you spelled it wrong");
				return 300;
		}

	}
	
	//public void setObjectVolume(string objectName, float objectLvl){
		//needs work
	//}



	//cleanup
	/*public void setMasterVol(float masterLvl){
		masterMixer.SetFloat ("MasterVol", masterLvl);
	}
	public void setMaxAmbiance(float ambianceLvl){
		masterMixer.SetFloat ("MaxAmbianceVol", ambianceLvl);
	}
	public void setMaxEffect(float effectLvl){
		masterMixer.SetFloat ("MaxEffectVol", effectLvl);
	}
	public void setMaxMusic(float musicLvl){
		masterMixer.SetFloat ("MaxMusicVol", musicLvl);
	}
	public void setMaxVoice(float voiceLvl){
		masterMixer.SetFloat ("MaxVoiceVol", voiceLvl);
	}
	public void resetToDefault(){
		masterMixer.ClearFloat ("MasterVol");
		masterMixer.ClearFloat ("MaxAmbianceVol");
		masterMixer.ClearFloat ("MaxEffectVol");
		masterMixer.ClearFloat ("MaxMusicVol");
		masterMixer.ClearFloat ("MaxVoiceVol");

	}*/
	#endregion

	#region playFunct
	//set up each type of sound play functions

	//generic idea for sending and playing an audio on object
	//AudioManager.playMe(this, "fire");
	//bool playMe(monobehavior mono, string name){
	//sound = Mono.gameObject.addComponent<AudioSource>;
	//as.clip;
	//}

	#endregion


}		//end of audio manager



/*Ideas/todo
 * 
 * json sound list
 *	json file: filename, type of sound, priority always load or on use load			- DONE
 *	http://jsonformatter.curiousconcept.com/ - format json, may not be simplejson
 *	http://json.org/example
 *	look at checkpointManager.cs & AchievementManager.cs
 *	http://wiki.unity3d.com/index.php/SimpleJSON
 * 
 * check sounds are on list and ready to load
 * when called play sound or throw error
 * gui should call sound manager for volume changes
 * 
 * play functions
 *	void play sound
 *	void pause sound
 *	bool is playing
 *	void stop playing
 *	void set loop sound
*/