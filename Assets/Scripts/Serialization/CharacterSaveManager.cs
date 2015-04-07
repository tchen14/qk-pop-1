using UnityEngine;
using System.Collections;

/*
 *	This will save character related data. The character in this context is defined as art (2D/3D) data.
 *	Data such as the character's gender, clothing and hair style will be kept here.
 *
 *	This script must be on the same object as the "CharacterBuilder" script
 */

public class CharacterSaveManager : SaveManager {

	//!Saves character builder data out to playerprefs
	public void SaveCharacter(params string[] characterParts)
	{
		//Saves out current set of models that the player has chosen for their character
		PlayerPrefs.SetString("PlayerHead", characterParts[0]);
		PlayerPrefs.SetString("PlayerHair", characterParts[1]);
		PlayerPrefs.SetString("PlayerHat", characterParts[2]);
		PlayerPrefs.SetString("PlayerTorso", characterParts[3]);
		PlayerPrefs.SetString("PlayerShirt", characterParts[4]);
		PlayerPrefs.SetString("PlayerLeg", characterParts[5]);
		PlayerPrefs.SetString("PlayerPant", characterParts[6]);
		PlayerPrefs.SetString("PlayerShoe", characterParts[7]);
		
		//save to playerprefs just in case
		PlayerPrefs.Save();
	}
}
