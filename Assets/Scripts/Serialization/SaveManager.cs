using UnityEngine;
using System.Collections;

/*
 *	This is player dependent save manager. This is designed with multiple player profiles in mind.
 */
public abstract class SaveManager : SerializationManager {
#pragma warning disable 0414
	//! This value will set the active profile for all SaveManager base classes
	static int activeProfile = 0;
#pragma warning restore 0414
}
