using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Debug = FFP.Debug;

/*!
 *  Loads in Character Models for selecting a character and building it
 *  Character takes place when the player starts a new game file and decides on how they want their character built
 * 
 *  This script requires the script "CharacterSaveManager" to be on the same object as this script
 */

public class CharacterBuilder : MonoBehaviour {

	//Instance of Player Save Manager to save out character when built
	CharacterSaveManager characterSaveManager;

	//All lists for the different body parts for each gender
	private List <GameObject> maleHead = new List <GameObject>();			
	private List <GameObject> maleHair = new List <GameObject>();			
	private List <GameObject> maleHat = new List <GameObject>();			
	private List <GameObject> maleTorso = new List <GameObject>();			
	private List <GameObject> maleShirt = new List <GameObject>();			
	private List <GameObject> maleLeg = new List <GameObject>();			
	private List <GameObject> malePant = new List <GameObject>();			
	private List <GameObject> maleShoe = new List <GameObject>();			
	private List <GameObject> femaleHead = new List <GameObject>();			
	private List <GameObject> femaleHair = new List <GameObject>();
	private List <GameObject> femaleHat = new List <GameObject>();
	private List <GameObject> femaleTorso = new List <GameObject>();
	private List <GameObject> femaleShirt = new List <GameObject>();
	private List <GameObject> femaleLeg = new List <GameObject>();
	private List <GameObject> femalePant = new List <GameObject>();
	private List <GameObject> femaleShoe = new List <GameObject>();

	[HideInInspector]
	//a game object for each body part to hold the current displayed and instantiated gameobject
	public GameObject
		currentHeadObject,
   		currentHairObject,
     	currentHatObject,
     	currentTorsoObject,
     	currentShirtObject,
     	currentLegObject,
     	currentPantObject,
     	currentShoeObject;
	[HideInInspector]
	//Variable to tell the custom inspector if the game is running
	public bool gamePlaying = false;

	//a numerator for each body part that is the current position in the list that is to be displayed
	private int currentHead = 0;
	private int currentHair = 0;
	private int currentHat = 0;
	private int currentTorso = 0;
	private int currentShirt = 0;
	private int currentLeg = 0;
	private int currentPant = 0;
	private int currentShoe = 0;

	//a boolean that tells the display functions whether a body part needs to be changed and displayed
	//if the boolean is false then the display function does not need to change the displayed gameobject
	//if the boolean is true then the display function needs to change the current displaying gameobject
	[HideInInspector]
	public bool isMale = true;
	[HideInInspector]
	public bool isFemale = false;
	private bool changeHead = true;
	private bool changeHair = true;
	private bool changeHat = true;
	private bool changeTorso = true;
	private bool changeShirt = true;
	private bool changeLeg = true;
	private bool changePant = true;
	private bool changeShoe = true;

	// Use this for initialization
	void Start () {

		//Initialize and fill all the lists of all the loaded body part models
		characterSaveManager = GetComponent<CharacterSaveManager> ();
		LoadCharacterModelsFromJson ();
		gamePlaying = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//Calls the display functions for each body part
		DisplayHead();
		DisplayHair();
		DisplayHat();
		DisplayTorso();
		DisplayShirt();
		DisplayLeg();
		DisplayPant();
		DisplayShoe();
	}

	//!Displays the objects in the head lists for each gender
	protected void DisplayHead()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeHead)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentHeadObject != null)
			{
				Destroy(currentHeadObject);
			}

			if(isMale)
			{
				currentHeadObject = Instantiate(maleHead[currentHead], new Vector3(0,2,0), Quaternion.identity) as GameObject;
			}

			if(isFemale)
			{
				currentHeadObject = Instantiate(femaleHead[currentHead], new Vector3(0,2,0), Quaternion.identity) as GameObject;
			}
			changeHead = false;
		}
	}

	//!Displays the objects in the hair lists for each gender
	protected void DisplayHair()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeHair)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentHairObject != null)
			{
				Destroy(currentHairObject);
			}
			
			if(isMale)
			{
				currentHairObject = Instantiate(maleHair[currentHair], new Vector3(0,4,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentHairObject = Instantiate(femaleHair[currentHair], new Vector3(0,4,0), Quaternion.identity) as GameObject;
			}
			changeHair = false;
		}
	}

	//!Displays the objects in the hat lists for each gender
	protected void DisplayHat()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeHat)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentHatObject != null)
			{
				Destroy(currentHatObject);
			}
			
			if(isMale)
			{
				currentHatObject = Instantiate(maleHat[currentHat], new Vector3(0,6,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentHatObject = Instantiate(femaleHat[currentHat], new Vector3(0,6,0), Quaternion.identity) as GameObject;
			}
			changeHat = false;
		}
	}
	
	//!Displays the objects in the torso lists for each gender
	protected void DisplayTorso()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeTorso)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentTorsoObject != null)
			{
				Destroy(currentTorsoObject);
			}
			
			if(isMale)
			{
				currentTorsoObject = Instantiate(maleTorso[currentTorso], new Vector3(0,0,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentTorsoObject = Instantiate(femaleTorso[currentTorso], new Vector3(0,0,0), Quaternion.identity) as GameObject;
			}
			changeTorso = false;
		}
	}

	//!Displays the objects in the shirt lists for each gender
	protected void DisplayShirt()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeShirt)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentShirtObject != null)
			{
				Destroy(currentShirtObject);
			}
			
			if(isMale)
			{
				currentShirtObject = Instantiate(maleShirt[currentShirt], new Vector3(2,0,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentShirtObject = Instantiate(femaleShirt[currentShirt], new Vector3(2,0,0), Quaternion.identity) as GameObject;
			}
			changeShirt = false;
		}
	}

	//!Displays the objects in the leg lists for each gender
	protected void DisplayLeg()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeLeg)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentLegObject != null)
			{
				Destroy(currentLegObject);
			}
			
			if(isMale)
			{
				currentLegObject = Instantiate(maleLeg[currentLeg], new Vector3(0,-2,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentLegObject = Instantiate(femaleLeg[currentLeg], new Vector3(0,-2,0), Quaternion.identity) as GameObject;
			}
			changeLeg = false;
		}
	}

	//!Displays the objects in the pant lists for each gender
	protected void DisplayPant()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changePant)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentPantObject != null)
			{
				Destroy(currentPantObject);
			}
			
			if(isMale)
			{
				currentPantObject = Instantiate(malePant[currentPant], new Vector3(-2,-2,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentPantObject = Instantiate(femalePant[currentPant], new Vector3(-2,-2,0), Quaternion.identity) as GameObject;
			}
			changePant = false;
		}
	}

	//!Displays the objects in the shoe lists for each gender
	protected void DisplayShoe()
	{
		//checks whether there was a request to change the current displayed object in the list
		if(changeShoe)
		{
		//Destroys the current displayed object in the list and displays the new current object in the list depending on the gender
			if(currentShoeObject != null)
			{
				Destroy(currentShoeObject);
			}
			
			if(isMale)
			{
				currentShoeObject = Instantiate(maleShoe[currentShoe], new Vector3(0,-4,0), Quaternion.identity) as GameObject;
			}
			
			if(isFemale)
			{
				currentShoeObject = Instantiate(femaleShoe[currentShoe], new Vector3(0,-4,0), Quaternion.identity) as GameObject;
			}
			changeShoe = false;
		}
	}
	//!Switches the current gender to the opposite gender
	public void SwitchGender()
	{
	//Switches the gander to opposite of the current gender
		if(isMale)
		{
			isMale = false;
			isFemale = true;
		}

		else
		{
			isFemale = false;
			isMale = true;
		}
		
		//resets all variables so that the first object in each list will displayed for the new current gender
		currentHead = 0;
		currentHair = 0;
		currentHat = 0;
		currentTorso = 0;
		currentShirt = 0;
		currentLeg = 0;
		currentPant = 0;
		currentShoe = 0;
		changeHead = true;
		changeHair = true;
		changeHat = true;
		changeTorso = true;
		changeShirt = true;
		changeLeg = true;
		changePant = true;
		changeShoe = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchHead(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentHead != maleHead.Count - 1)
				{
					currentHead = currentHead + 1;
				}
				else
					return;
			}

			else if(isFemale)
			{
				if(currentHead != femaleHead.Count - 1)
				{
					currentHead = currentHead + 1;
				}
				else
					return;
			}
		}

		if(nextOrPrev == "Previous")
		{
			if(currentHead != 0)
			{
				currentHead = currentHead - 1;
			}
			else
				return;
		}
		changeHead = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchHair(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentHair != maleHair.Count - 1)
				{
					currentHair = currentHair + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentHair != femaleHair.Count - 1)
				{
					currentHair = currentHair + 1;
				}
				else
					return;
			}
		}

		else if(nextOrPrev == "Previous")
		{
			if(currentHair != 0)
			{
				currentHair = currentHair - 1;
			}
			else
				return;
		}
		changeHair = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchHat(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentHat != maleHat.Count - 1)
				{
					currentHat = currentHat + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentHat != femaleHat.Count - 1)
				{
					currentHat = currentHat + 1;
				}
				else
					return;
			}
		}

		if(nextOrPrev == "Previous")
		{
			if(currentHat != 0)
			{
				currentHat = currentHat - 1;
			}
			else
				return;
		}
		changeHat = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchTorso(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentTorso != maleTorso.Count - 1)
				{
					currentTorso = currentTorso + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentTorso != femaleTorso.Count - 1)
				{
					currentTorso = currentTorso + 1;
				}
				else
					return;
			}
		}
	
		if(nextOrPrev == "Previous")
		{
			if(currentTorso != 0)
			{
				currentTorso = currentTorso - 1;
			}
			else
				return;
		}
		changeTorso = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchShirt(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentShirt != maleShirt.Count - 1)
				{
					currentShirt = currentShirt + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentShirt != femaleShirt.Count - 1)
				{
					currentShirt = currentShirt + 1;
				}
				else
					return;
			}
		}
	
		if(nextOrPrev == "Previous")
		{
			if(currentShirt != 0)
			{
				currentShirt = currentShirt - 1;
			}
			else
				return;
		}
		changeShirt = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchLeg(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentLeg != maleLeg.Count - 1)
				{
					currentLeg = currentLeg + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentLeg != femaleLeg.Count - 1)
				{
					currentLeg = currentLeg + 1;
				}
				else
					return;
			}
		}
	
		if(nextOrPrev == "Previous")
		{
			if(currentLeg != 0)
			{
				currentLeg = currentLeg - 1;
			}
			else
				return;

			changeLeg = true;
		}
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchPant(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentPant != malePant.Count - 1)
				{
					currentPant = currentPant + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentPant != femalePant.Count - 1)
				{
					currentPant = currentPant + 1;
				}
				else
					return;
			}
		}
	
		if(nextOrPrev == "Previous")
		{
			if(currentPant != 0)
			{
				currentPant = currentPant - 1;
			}
			else
				return;
		}
		changePant = true;
	}

	//!Iterates the list to the next or previous position in the object's list
	public void SwitchShoe(string nextOrPrev)
	{
	//Adds or subtracts one from the variable holding the current position in the object's list
		if(nextOrPrev == "Next")
		{
			if(isMale)
			{
				if(currentShoe != maleShoe.Count - 1)
				{
					currentShoe = currentShoe + 1;
				}
				else
					return;
			}
			
			else if(isFemale)
			{
				if(currentShoe != femaleShoe.Count - 1)
				{
					currentShoe = currentShoe + 1;
				}
				else
					return;
			}
		}
	
		if(nextOrPrev == "Previous")
		{
			if(currentShoe != 0)
			{
				currentShoe = currentShoe - 1;
			}
			else
				return;
		}
		changeShoe = true;
	}

	//!Builds each list of gameobjects with the imported names of the models from JSON file
	protected void BuildLists(JSONNode jsonNode)
	{
	//Iterates through the arrays in the JSON node and adds and fills the lists with their appropriate models from the project file
		/*for(int count = 0; count < jsonNode["Head"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Head"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Head/Male/" + object_name + ".prefab");
			maleHead.Add(loaded_object);
		}

		for(int count = 0; count < jsonNode["Hair"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Hair"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Hair/Male/" + object_name + ".prefab");
			maleHair.Add(loaded_object);
		}

		for(int count = 0; count < jsonNode["Hat"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Hat"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Hat/Male/" + object_name + ".prefab");
			maleHat.Add(loaded_object);
		}

		for(int count = 0; count < jsonNode["Torso"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Torso"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Torso/Male/" + object_name + ".prefab");
			maleTorso.Add(loaded_object);
		}

		for(int count = 0; count < jsonNode["Shirt"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Shirt"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Shirt/Male/" + object_name + ".prefab");
			maleShirt.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Leg"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Leg"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Leg/Male/" + object_name + ".prefab");
			maleLeg.Add(loaded_object);
		}

		for(int count = 0; count < jsonNode["Pant"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Pant"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Pant/Male/" + object_name + ".prefab");
			malePant.Add(loaded_object);
		}

		for(int count = 0; count < jsonNode["Shoe"]["Male"].Count; count = count + 1)
		{
			string object_name = jsonNode["Shoe"]["Male"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Shoe/Male/" + object_name + ".prefab");
			maleShoe.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Head"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Head"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Head/Female/" + object_name + ".prefab");
			femaleHead.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Hair"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Hair"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Hair/Female/" + object_name + ".prefab");
			femaleHair.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Hat"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Hat"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Hat/Female/" + object_name + ".prefab");
			femaleHat.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Torso"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Torso"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Torso/Female/" + object_name + ".prefab");
			femaleTorso.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Shirt"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Shirt"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Shirt/Female/" + object_name + ".prefab");
			femaleShirt.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Leg"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Leg"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Leg/Female/" + object_name + ".prefab");
			femaleLeg.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Pant"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Pant"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Pant/Female/" + object_name + ".prefab");
			femalePant.Add(loaded_object);
		}
		
		for(int count = 0; count < jsonNode["Shoe"]["Female"].Count; count = count + 1)
		{
			string object_name = jsonNode["Shoe"]["Female"][count];
			GameObject loaded_object = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/CharacterBuilder/Shoe/Female/" + object_name + ".prefab");
			femaleShoe.Add(loaded_object);
		}*/
	}

	//!Creates a JSONNode from the JSON file and calls a function to build the lists of gameobjects
	protected void LoadCharacterModelsFromJson()
	{
		//Checking to see if character builder json file exists if it doesnt return
		if(!System.IO.File.Exists(Application.dataPath + "/Resources/Json/characterBuilderJson"))
		{
			Debug.Error ("serilaization", "Could not find Character Models JSON file");
			return;
		}
		
		//read in character builder json file
		string jsonRead = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Json/characterBuilderJson");
		JSONNode jsonParsed = JSON.Parse (jsonRead);
		
		//Call function to build and fill the appropriate lists with gameobjects and their respective models from the project folder
		BuildLists(jsonParsed);
	}

	//!Saves character builder data out to playerprefs
	public void SaveCharacter()
	{
	//Saves out current set of models that the player has chosen for their character
		if(isMale)
		{
			characterSaveManager.SaveCharacter(maleHead[currentHead].name, maleHair[currentHair].name, maleHat[currentHat].name, maleTorso[currentTorso].name,
			                                   maleShirt[currentShirt].name, maleLeg[currentLeg].name, malePant[currentPant].name, maleShoe[currentShoe].name);
		}
		
		if(isFemale)
		{
			characterSaveManager.SaveCharacter(femaleHead[currentHead].name, femaleHair[currentHair].name, femaleHat[currentHat].name, femaleTorso[currentTorso].name,
			                                  femaleShirt[currentShirt].name, femaleLeg[currentLeg].name, femalePant[currentPant].name, femaleShoe[currentShoe].name);
		}
	}
}
