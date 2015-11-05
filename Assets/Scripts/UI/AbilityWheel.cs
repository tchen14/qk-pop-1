using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Debug = FFP.Debug;

/*this is the helper script for the AbilityWheel, everything
*related to the prefab is in this just to try to keep GameHUD
*cleaner and easier to remove or update this.
*/
public class AbilityWheel : MonoBehaviour {

	public GameObject[] abilityButtons;//this array should be already populated with the buttons in ascending order from top to bottom
	
	RectTransform abscroller; 		//these values are how I do my logic when moving the pieces around,
	RectTransform abviewport; 		//they correspond to different components of the prefab
	GameObject abilityWheelAnchor;
	GameObject skillWheelCursor;
	RectTransform skillWheelBounds;

	public float [] ablocy = {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f}; // the variable that stores the location data of the ability buttons
	public bool skillsOpen = false;
	public bool skillsMoving = false;
	public bool skillsRotating = false;
	public int cur_ability = 2;

	// Use this for initialization
	void Awake() 
	{
		//These locate the corresponding pieces of the prefab and stores them in the the variables above
		GameObject sWheelTmp = GameObject.Find ("abBounds");
		skillWheelBounds = sWheelTmp.GetComponent<RectTransform>();
		sWheelTmp = GameObject.Find ("AbilityWheel");
		RectTransform tmprect = sWheelTmp.GetComponent<RectTransform> ();

		setupabilityWheelLocation (tmprect,sWheelTmp );

		abilityWheelAnchor = GameObject.Find ("AbilityWheelAnchor");
		abscroller = abilityWheelAnchor.GetComponent<RectTransform>();
		sWheelTmp = GameObject.Find ("abilityWheelView");
		abviewport = sWheelTmp.GetComponent<RectTransform> ();
		skillWheelCursor = GameObject.Find("AbilityWheelCursor");
		
		settingUpAbilityWheel ();//this sets up the array locations of the buttons

		skillWheelCursor.SetActive (false);//makes sure the skillWheelCursor is not visible at startup
	}

	/***********************************************************************
	 *------<These are the functions Involving the the ability wheel>------*
	 **********************************************************************/

	void setupabilityWheelLocation(RectTransform tmp, GameObject gotmp){
		GameObject tmpHUD = GameObject.Find ("mainHUD");
		if (gotmp.transform.parent != tmpHUD.transform)
			gotmp.transform.SetParent(tmpHUD.transform);

		tmp.localPosition = new Vector3(-123.3f, -65.0f, 0.0f);

	}

	public int getSelectedAbility()
	{		/*0 is Cut
			* 1 is Sound Throw
			* 2 is Taze
			* 3 is Push
			* 4 is Pull*/
		return cur_ability;
	}

	//if the showskills is called, and skills are not open or not moving, then show them
	public void showSkills()
	{
		if (!skillsMoving && !skillsOpen) 
		{
			skillsOpen = true;
			skillsMoving = true;
		}
		
	}

	//if the hideskills is called, and skills are open or not moving, then it hides them
	public void hideSkills()
	{
		if(!skillsMoving && skillsOpen)
		{
			skillsMoving = true;
			skillsOpen = false;
		}
	}
	
	//This is the function that causes the ability wheel to move into position, using the bound box from before
	//it moves the abilitywheelanchor to either the top edge(should be in middle of screen) of the skillWheelBound 
	//or the bottom(should be next to phone aligned with the map icon)
	public void moveAbilities() 
	{

		float movespeed = Time.deltaTime * 250; //this is the speed the wheel travels to the designated locations
		
		if(skillsOpen)
		{	
			skillWheelCursor.SetActive (true);
			abviewport.offsetMax = new Vector2(abviewport.offsetMax.x, Mathf.MoveTowards(abviewport.offsetMax.y, 160.0f, movespeed*2));
			abviewport.offsetMin = new Vector2(abviewport.offsetMin.x, Mathf.MoveTowards(abviewport.offsetMin.y, -160.0f, movespeed*2));
			abscroller.localPosition = new Vector2(abscroller.localPosition.x, Mathf.MoveTowards(abscroller.localPosition.y, skillWheelBounds.offsetMax.y, movespeed));
		}
		else
		{
			skillWheelCursor.SetActive (false);
			abviewport.offsetMax = new Vector2(abviewport.offsetMax.x, Mathf.MoveTowards(abviewport.offsetMax.y, 23.0f, movespeed*2));
			abviewport.offsetMin = new Vector2(abviewport.offsetMin.x, Mathf.MoveTowards(abviewport.offsetMin.y, -23.0f, movespeed*2));
			abscroller.localPosition = new Vector2(abscroller.localPosition.x, Mathf.MoveTowards(abscroller.localPosition.y, skillWheelBounds.offsetMin.y, movespeed));
		}
		skillsMoving = false;
	}
	
	//this gets the location of each button y coordinate and stores them in an array to determine where the icons go to next during the rotation function
	void settingUpAbilityWheel()
	{
		for (int i = 0; i < abilityButtons.Length; i++) {
			RectTransform tmp = abilityButtons [i].GetComponent<RectTransform> ();
			ablocy [i] = tmp.localPosition.y;
		}
		updateAbilityIcons();
	}
	
	//this updates the outside dummy ability icons to look like the icons on the opposing sides of the array, 
	//****this can be changed or updated: this was just my thought process at the time****
	void updateAbilityIcons()
	{
		GameObject abbotim = Instantiate(abilityButtons[5]);
		abbotim.transform.SetParent (GameObject.Find ("content Panel").transform, false);
		abbotim.transform.localPosition = new Vector2 (0.0f,ablocy[0]);
		Destroy (abilityButtons [0], 0.0f);
		abilityButtons [0] = abbotim;
		
		GameObject abtop = Instantiate(abilityButtons[1]);
		abtop.transform.SetParent (GameObject.Find ("content Panel").transform, false);
		abtop.transform.localPosition = new Vector2 (0.0f,ablocy[6]);
		Destroy (abilityButtons [6], 0.0f);
		abilityButtons [6] = abtop;
	}
	
	//this is the function that rotates the icons up in the GUI and updates the ability icon array accordingly
	public IEnumerator Rotate_skills_up()
	{
		skillsRotating = true;
		GameObject abtemp = abilityButtons[1];
		int ab_amount = abilityButtons.Length - 1;
		
		for (int i = ab_amount; i >=1; i--) {
			RectTransform tmp = abilityButtons [i].GetComponent<RectTransform> ();
			tmp.localPosition = new Vector2 (tmp.localPosition.x, Mathf.MoveTowards (tmp.localPosition.y, ablocy [i - 1],	250.0f));
		}
		
		yield return new WaitForSeconds (0.1f);
		
		RectTransform topImage = abilityButtons[1].GetComponent<RectTransform>();
		RectTransform abBottom = abilityButtons[6].GetComponent<RectTransform>();
		topImage.localPosition = abBottom.localPosition;
		abBottom.localPosition = new Vector2 (abBottom.localPosition.x, ablocy [6]);
		
		for (int i = 1; i < ab_amount - 1; i++)
			abilityButtons[i] = abilityButtons[i+1];
		abilityButtons[ab_amount - 1] = abtemp;

		if (cur_ability == 4)
			cur_ability = 0;
		else
			cur_ability = cur_ability + 1;
		
		updateAbilityIcons ();
		
		skillsRotating = false;
	}
	
	//this is the function that rotates the icons down in GUI and updates the ability icon array accordingly
	public IEnumerator Rotate_skills_down()
	{
		skillsRotating = true;
		GameObject abtemp = abilityButtons[5];
		int ab_amount = abilityButtons.Length - 1;
		
		for (int i = 0; i < ab_amount; i++) {
			RectTransform tmp = abilityButtons [i].GetComponent<RectTransform> ();
			tmp.localPosition = new Vector2 (tmp.localPosition.x, Mathf.MoveTowards (tmp.localPosition.y, ablocy [i + 1], 250.0f));
		}
		
		yield return new WaitForSeconds (0.1f);
		
		RectTransform botImage = abilityButtons[5].GetComponent<RectTransform>();
		RectTransform abtop = abilityButtons[0].GetComponent<RectTransform>();
		botImage.localPosition = abtop.localPosition;
		abtop.localPosition = new Vector2 (abtop.localPosition.x, ablocy [0]);
		
		for (int i = ab_amount - 1; i > 1; i--)
			abilityButtons[i] = abilityButtons[i-1];
		abilityButtons[1] = abtemp;

		if (cur_ability == 0)
			cur_ability = 4;
		else
			cur_ability = cur_ability - 1;
		
		updateAbilityIcons ();
		
		skillsRotating = false;
	}
}
