using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

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

	public AudioMixer masterMixer;



	//! Unity Start function
    void Start() {
		
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


	}

    //! Unity Update function
    //void Update() {
	//}
	#region setFunct
	public void setMasterVol(float masterLvl){
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

	}

	#endregion
}
