using UnityEngine;
using System.Collections;

/*
 *	This is player dependent save manager. This is designed with multiple player profiles in mind.
 */
public abstract class SaveManager : SerializationManager {
	//! This value will set the active profile for all SaveManager base classes
	static int activeProfile = 0;
}
