using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/*! 
 *	This class is to manage the saving of any type of game data.
 *	Save data as a json string
 *	Unity's PlayerPref class will be used as the method of serialization
 */
public class SerializationManager : MonoBehaviour {

    //! Save string
	protected void Save(string key, string value){
    	PlayerPrefs.SetString(key, value);
    }

    //! Load string
    protected string Load(string key){
    	if (PlayerPrefs.HasKey (key))
			return PlayerPrefs.GetString (key);
		else
			Log.E ("core", "Loading from PlayerPrefs failed. Key \"" + key + "\" does not exist.");
		return null;
    }

    //! Encode dictionary
	protected string EncodeSaveData(Dictionary<string, string> d){
		string s;
		s = "";

		//use simplejson to encode data

		return s;
    }

    //! Decode dictionary
	protected Dictionary<string, string> DecodeSaveData(string s){
		Dictionary<string, string> d = new Dictionary<string, string>();

		//use simplejson to decode data

		return d;
    }
}
