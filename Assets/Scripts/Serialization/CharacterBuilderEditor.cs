	using UnityEngine;
using System.Collections;
using UnityEditor;

/*!
 *  Editor Script to change and display the character builder
 */

[CustomEditor(typeof(CharacterBuilder))]
public class CharacterBuilderEditor : Editor {

	//!Creates the buttons and calls the correct functions to display the character builder and be able to edit it
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		CharacterBuilder characterBuilder = (CharacterBuilder)target;

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Gender");

			if(characterBuilder.isMale)
			{	
				GUILayout.Label("Male");
			}

			if(characterBuilder.isFemale)
			{	
				GUILayout.Label("Female");
			}

			GUI.backgroundColor = Color.magenta;
			if(GUILayout.Button("Change Gender"))
			{
				characterBuilder.SwitchGender();
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			
			GUILayout.Label("Head");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Head"))
			{
				characterBuilder.SwitchHead("Previous");
			}
			
			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentHeadObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Head"))
			{
				characterBuilder.SwitchHead("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Hair");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Hair"))
			{
				characterBuilder.SwitchHair("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentHairObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Hair"))
			{
				characterBuilder.SwitchHair("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Hat");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Hat"))
			{
				characterBuilder.SwitchHat("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentHatObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Hat"))
			{
				characterBuilder.SwitchHat("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Torso");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Torso"))
			{
				characterBuilder.SwitchTorso("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentTorsoObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Torso"))
			{
				characterBuilder.SwitchTorso("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Shirt");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Shirt"))
			{
				characterBuilder.SwitchShirt("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentShirtObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Shirt"))
			{
				characterBuilder.SwitchShirt("Next");
			}
		
		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Leg");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Leg"))
			{
				characterBuilder.SwitchLeg("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentLegObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Leg"))
			{
				characterBuilder.SwitchLeg("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Pant");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Pant"))
			{
				characterBuilder.SwitchPant("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentPantObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Pant"))
			{
				characterBuilder.SwitchPant("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUILayout.Label("Shoe");

			GUI.backgroundColor = Color.red;
			if(GUILayout.Button("Previous Shoe"))
			{
				characterBuilder.SwitchShoe("Previous");
			}

			if(characterBuilder.gamePlaying)
			{
				GUILayout.Label(characterBuilder.currentShoeObject.name);
			}

			GUI.backgroundColor = Color.blue;
			if(GUILayout.Button("Next Shoe"))
			{
				characterBuilder.SwitchShoe("Next");
			}

		GUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
		GUILayout.BeginHorizontal("box");

			GUI.backgroundColor = Color.green;
			if(GUILayout.Button("Save Character"))
			{
				characterBuilder.SaveCharacter();
			}

		GUILayout.EndHorizontal();
	}
}
