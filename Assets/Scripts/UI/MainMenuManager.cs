using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//******************************************************************************************
/* This is the Main Menu Manager, it manages all the button and UI interactions done
 * with the stuff in the canvasors in the Menu Scene
 * Note:
 * All UI functions must be public to be seen by the UI Gameobjects
 * */
//******************************************************************************************

public class MainMenuManager : MonoBehaviour {

	public float cursorSpeed = 7f;							//Controller cursor speed

	//Editor
	public bool useDefaultSettings = false;					//check this in the editor to load default settings
	public bool seizureModeOn = false;						//check this if you don't want your eyeballs anymore

	//Checkers
	string currentMenu;						                //Current menu

	//Canvasies
	public GameObject mainCanvas;						    //The main menu
	public GameObject optionsCanvas;					    //The left options buttons
	public GameObject videoOpCanvas;						//Video options menu
	public GameObject gameplayOpCanvas;						//gameplay options menu
	public GameObject audioOpCanvas;						//audio options menu
	public GameObject controlsOpCanvas;						//controls options menu

	//Sliders and Toggles
	public GameObject qualitySlider;						//Slider for video quality
	public GameObject FoVSlider;							//Slider for camera FoV
	public GameObject bloomToggle;							//Toggle for bloom
	public GameObject difficultySlider;						//Slider for gameplay difficulty
	public GameObject invertToggle;							//Toggle for mouse Y axis inversion
	public GameObject gimbleToggle;							//Toggle for gimble mode
	public GameObject volumeSlider; 						//Slider for master volume
	public GameObject effectsVSlider; 						//Slider for effects volume
	public GameObject musicVSlider; 						//Slider for music volume
	public GameObject playInBGToggle;						//Toggle for play in background sound

	//Advanced Video Sliders
	public GameObject textureQSlider;						//The Sliders for Advanced Video settings
	public GameObject aaSlider;								//They are all pretty self explanatory
	public GameObject anistropicTSlider;
	public GameObject aaText;
	public GameObject aTextureText;

	//Cursor stuff
	public GameObject cursor;								//Cursor gameobject
	public GameObject cursorPoint;							//point where raycast is shot from
	bool controllerUsed = false;							//Controller mode checker
	Vector2 cursorPos;										//cursor pos on canvas
	GameObject lastButtonHovered;							//last gameobject selected with cursor

	//Important Things
	string url = "http://i.imgur.com/eBXyo2w.jpg";
	GameObject urlTar;

	//---------------------------
	//UNITY
	//---------------------------

	void Start () {

		cursorPos = cursor.transform.position;
		//cursor.SetActive(false);

		//trust me, this is important
		urlTar = GameObject.Find ("urlTar");
		StartCoroutine (loadImgFromWeb ());

		//Make sure correct canvas is loaded first
		videoOpCanvas.SetActive(false);
		gameplayOpCanvas.SetActive(false);
		optionsCanvas.SetActive(false);
		controlsOpCanvas.SetActive(false);
		audioOpCanvas.SetActive (false);
		
		mainCanvas.SetActive(true);

		currentMenu = "main";

		InitFirstSettings ();


	}
	

	void Update () {

		if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 ){
			//cursor.transform.position = Input.mousePosition;
		}

		//Controller cursor control
		if(Input.GetAxis("Vertical") > 0){
			if(controllerUsed == false){
				cursor.SetActive(true);
				controllerUsed = true;
			}
			cursorPos.y += Input.GetAxis("Vertical") * cursorSpeed;
			if(cursorPos.y > Screen.height){
				cursorPos.y = Screen.height;
			}
			cursor.transform.position = cursorPos;
			//cursor.transform.Translate(Vector2.up * Time.deltaTime * 100);
		}
		if(Input.GetAxis("Vertical") < 0){
			if(controllerUsed == false){
				cursor.SetActive(true);
				controllerUsed = true;
			}
			cursorPos.y += Input.GetAxis("Vertical") * cursorSpeed;
			if(cursorPos.y < 0){
				cursorPos.y = 0;
			}
			cursor.transform.position = cursorPos;
		}
		if(Input.GetAxis("Horizontal") > 0 ){
			if(controllerUsed == false){
				cursor.SetActive(true);
				controllerUsed = true;
			}
			cursorPos.x += Input.GetAxis("Horizontal") * cursorSpeed;
			if(cursorPos.x > Screen.width){
				cursorPos.x = Screen.width;
			}
			cursor.transform.position = cursorPos;
			//cursor.transform.Translate(Vector2.right * Time.deltaTime * 100);
		}
		if(Input.GetAxis("Horizontal") < 0){
			if(controllerUsed == false){
				cursor.SetActive(true);
				controllerUsed = true;
			}

			cursorPos.x += Input.GetAxis("Horizontal") * cursorSpeed;
			if(cursorPos.x < 10){
				cursorPos.x = 10;
			}
			cursor.transform.position = cursorPos;
			//cursor.transform.Translate(-Vector2.right * Time.deltaTime * 100);
		}


		Ray ray = new Ray(cursorPoint.transform.position, cursorPoint.transform.forward * 10);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

		if (Physics.Raycast (ray, out hit, 25)) {
			var pointer = new PointerEventData(EventSystem.current);
			if(hit.collider.gameObject.tag == "menuButton" && hit.collider != null) {
				ExecuteEvents.Execute(hit.collider.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
				lastButtonHovered = hit.collider.gameObject;
				if(Input.GetButtonDown("Jump")){
					ExecuteEvents.Execute(hit.collider.gameObject, pointer, ExecuteEvents.pointerClickHandler);
				}
			}

			if(hit.collider.gameObject.tag == "menuBack" && hit.collider != null) {
				ExecuteEvents.Execute(lastButtonHovered, pointer, ExecuteEvents.pointerExitHandler);
				
			}
		}



	}

	//---------------------------
	//PLAY BUTTON
	//---------------------------

	//loads new scene
	public void GoToScene (string name) {
		Application.LoadLevel (name);
	}

	//---------------------------
	//SCENE NAVIGATION
	//---------------------------

	//Disables main menu, enables options
	public void GoToOptions () {
		mainCanvas.SetActive (false);
		audioOpCanvas.SetActive (false);
		optionsCanvas.SetActive (true);
		currentMenu = "options";
	}

	//Enables video options
	public void GoToVideoOptions () {
		if(currentMenu != "video options"){
			gameplayOpCanvas.SetActive(false);
			audioOpCanvas.SetActive (false);
			controlsOpCanvas.SetActive(false);
			videoOpCanvas.SetActive(true);

			UpdateAVideoSliders();

			//uncheck "use default settings" in the editor if you dont want to load changes
			if(!useDefaultSettings){
				ApplyVideoSettings ();
			}

			currentMenu = "video options";
		}
	}

	//Enables gameplay options
	public void GoToGameplayOptions () {
		if(currentMenu != "gameplay options"){
			videoOpCanvas.SetActive(false);
			audioOpCanvas.SetActive (false);
			controlsOpCanvas.SetActive(false);
			gameplayOpCanvas.SetActive(true);

			//Change PlayerFoV into string
			float newFoVvalue = PlayerPrefs.GetFloat("FoV") * 100f;
			int newFoVvalue2 = (int)newFoVvalue;
			string newFoVtext = newFoVvalue2.ToString();
			GameObject.Find ("FoVvalueText").GetComponent<Text> ().text = newFoVtext;

			//uncheck "use default settings" in the editor if you dont want to load changes
			if(!useDefaultSettings){
				ApplyGameplaySettings ();
			}

			currentMenu = "gameplay options";
		}
	}

	//Enables audio options
	public void GoToAudioOptions () {
		if(currentMenu != "audio options"){
			videoOpCanvas.SetActive(false);
			gameplayOpCanvas.SetActive(false);
			controlsOpCanvas.SetActive(false);
			audioOpCanvas.SetActive (true);

			//uncheck "use default settings" in the editor if you dont want to load changes
			if(!useDefaultSettings){
				ApplyAudioSettings ();
			}
			currentMenu = "audio options";
		}
	}

	//Go back to main from options
	public void GoToMain () {
		if(currentMenu != "main"){
			videoOpCanvas.SetActive(false);
			gameplayOpCanvas.SetActive(false);
			optionsCanvas.SetActive(false);
			controlsOpCanvas.SetActive(false);
			audioOpCanvas.SetActive (false);

			mainCanvas.SetActive(true);
			currentMenu = "main";
		}
	}
	//Enables audio options
	public void GoToControlsOptions () {
		if(currentMenu != "controls options"){
			videoOpCanvas.SetActive(false);
			gameplayOpCanvas.SetActive(false);
			audioOpCanvas.SetActive (false);
			controlsOpCanvas.SetActive(true);
			
			currentMenu = "controls options";
		}
	}


	//---------------------------
	//OPTIONS MANIPULATION
	//---------------------------

	//changes game difficulty and text indicator
	public void SetDifficulty () {

		int diff = (int)GameObject.Find ("difficultySlider").GetComponent<Slider> ().value;
		string newDiff = " ";

		switch (diff) {
		
		case 0:
			newDiff = "Easiest";
			break;
		case 1:
			newDiff = "Easy";
			break;
		case 2:
			newDiff = "Medium";
			break;
		case 3:
			newDiff = "Hard";
			break;
		case 4:
			newDiff = "Hardest";
			break;
		}

		PlayerPrefs.SetFloat ("UI_difficulty", GameObject.Find ("difficultySlider").GetComponent<Slider> ().value);

		GameObject.Find ("difficultyText").GetComponent<Text> ().text = newDiff;
	}

	//Changes Game Video quality and text indicator
	public void SetQuality () {
		
		int qual = (int)GameObject.Find ("qualitySlider").GetComponent<Slider> ().value;
		string newQual = " ";
		
		switch (qual) {
			
		case 0:
			newQual = "Lowest";
			QualitySettings.SetQualityLevel(1,true);
			break;
		case 1:
			newQual = "Low";
			QualitySettings.SetQualityLevel(2,true);
			break;
		case 2:
			newQual = "Medium";
			QualitySettings.SetQualityLevel(3,true);
			break;
		case 3:
			newQual = "High";
			QualitySettings.SetQualityLevel(4,true);
			break;
		case 4:
			newQual = "Highest";
			QualitySettings.SetQualityLevel(5,true);
			break;
		}

		UpdateAVideoSliders ();
		PlayerPrefs.SetFloat ("UI_quality", GameObject.Find ("qualitySlider").GetComponent<Slider> ().value);
		GameObject.Find ("qualityText").GetComponent<Text> ().text = newQual;
	}

	//Applys Gameplay changes
	public void SetGameplayToggles () {

		//invert Y axis
		if(invertToggle.GetComponent<Toggle>().isOn){
			PlayerPrefs.SetInt ("UI_invert", 1);
		}
		else{
			PlayerPrefs.SetInt ("UI_invert", 0);
		}

		//gimble mode
		if(gimbleToggle.GetComponent<Toggle>().isOn){
			PlayerPrefs.SetInt ("UI_gimble", 1);
		}
		else{
			PlayerPrefs.SetInt ("UI_gimble", 0);
		}
	}

	//Applys video changes
	public void SetVideoChanges () {

		//FoV
		float newFoVvalue = FoVSlider.GetComponent<Slider> ().value;
		int newFoVvalue2 = (int)newFoVvalue;
		string newFoVtext = newFoVvalue2.ToString();
		GameObject.Find ("FoVvalueText").GetComponent<Text> ().text = newFoVtext;
		PlayerPrefs.SetFloat ("UI_FoV", GameObject.Find ("FoVSlider").GetComponent<Slider> ().value);
		GameObject.Find ("FoVvalueText").GetComponent<Text> ().text = newFoVtext;
	}

	//Applys audio toggle changes
	public void SetAudioToggles () {
		//bloom
		if(bloomToggle.GetComponent<Toggle>().isOn){
			PlayerPrefs.SetInt ("UI_playInBG", 1);
		}
		else{
			PlayerPrefs.SetInt ("UI_playInBG", 0);
		}
		
	}

	//Applys video toggle changes
	public void SetVideoToggles () {
		//bloom
		if(bloomToggle.GetComponent<Toggle>().isOn){
			PlayerPrefs.SetInt ("UI_bloom", 1);
		}
		else{
			PlayerPrefs.SetInt ("UI_bloom", 0);
		}

	}

	//Apply AA Changes
	public void SetAAChanges () {
		int aa = (int)aaSlider.GetComponent<Slider>().value;

		switch (aa) {
		
		case 0:
			QualitySettings.antiAliasing = 0;
			aaText.GetComponent<Text>().text = "Off";
			break;
		case 1:
			QualitySettings.antiAliasing = 2;
			aaText.GetComponent<Text>().text = "2x";
			break;
		case 2:
			QualitySettings.antiAliasing = 4;
			aaText.GetComponent<Text>().text = "4x";
			break;
		case 3:
			QualitySettings.antiAliasing = 8;
			aaText.GetComponent<Text>().text = "8x";
			break;
		}
	}

	//Apply Texture Quality Settings
	public void SetTextureQSettings () {
		int tQ = (int)textureQSlider.GetComponent<Slider> ().value;

		switch (tQ) {
		
		case 0:
			QualitySettings.masterTextureLimit = 3;
			break;
		case 1:
			QualitySettings.masterTextureLimit = 2;
			break;
		case 2:
			QualitySettings.masterTextureLimit = 1;
			break;
		case 3:
			QualitySettings.masterTextureLimit = 0;
			break;
		}
	}

	//Apply Anistropicalbeach filtering
	public void SetAnistropicSettings () {
		int aF = (int)anistropicTSlider.GetComponent<Slider> ().value;

		switch (aF) {
		
		case 0:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
			aTextureText.GetComponent<Text>().text = "Disabled";
			break;
		case 1:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
			aTextureText.GetComponent<Text>().text = "Enabled";
			break;
		case 2:
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
			aTextureText.GetComponent<Text>().text = "ForceEnabled";
			break;
		}
	}

	//Applys audio changes
	public void SetAudioChanges () {

		PlayerPrefs.SetFloat ("UI_volume", GameObject.Find ("volumeSlider").GetComponent<Slider> ().value);

	}

	//Applys music changes
	public void SetAudioMusicChanges () {

		PlayerPrefs.SetFloat ("UI_music", GameObject.Find ("musicVSlider").GetComponent<Slider> ().value);
		
	}

	//Applys effects changes
	public void SetAudioEffectsChanges () {
		
		PlayerPrefs.SetFloat ("UI_effects", GameObject.Find ("effectsVSlider").GetComponent<Slider> ().value);
		
	}


	public void GoToDog () {
		Application.OpenURL("https://youtu.be/oT3mCybbhf0?t=76");
	}

	//Apply Music Vol text
	public void changeMainVolText () {
		float newValue = volumeSlider.GetComponent<Slider> ().value;
		int newValue2 = (int)newValue;
		string newText = newValue2.ToString();

		GameObject.Find ("masterVolNum").GetComponent<Text> ().text = newText;
	}

	public void changeMusicVolText () {
		float newValue = musicVSlider.GetComponent<Slider> ().value;
		int newValue2 = (int)newValue;
		string newText = newValue2.ToString ();
		
		GameObject.Find ("musicVolNum").GetComponent<Text> ().text = newText;
	}

	public void changeEffectsVolText () {
		float newValue = effectsVSlider.GetComponent<Slider> ().value;
		int newValue2 = (int)newValue;
		string newText = newValue2.ToString ();
			
		GameObject.Find ("effectsVolNum").GetComponent<Text> ().text = newText;
	}

	//---------------------------
	//LOADING AND APPLYING CHANGES
	//---------------------------

	//Checks if pre-existing settings exist and sets defaults if there is not
	void InitFirstSettings () {
		if(!PlayerPrefs.HasKey("UI_settings")){
			PlayerPrefs.SetString("UI_settings", "set");
				
				//Audio
			PlayerPrefs.SetFloat("UI_volume",1);
			PlayerPrefs.SetFloat("UI_effects",1);
			PlayerPrefs.SetFloat("UI_music",1);
			PlayerPrefs.SetInt("UI_playInBG",1);

				//Gameplay
			PlayerPrefs.SetFloat("UI_difficulty",3);
			PlayerPrefs.SetFloat("UI_invertY",0);
			PlayerPrefs.SetFloat("UI_gimble",0);
			PlayerPrefs.SetFloat("UI_FoV",0.5f);
				
				//Video
			PlayerPrefs.SetInt("UI_quality",QualitySettings.GetQualityLevel());
			PlayerPrefs.SetInt("UI_bloom",1);

				//Controls
				
			}
		}

	//Applys the settings from PlayerPrefs
	void ApplyVideoSettings () {

		qualitySlider.GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("UI_quality");

		if(PlayerPrefs.GetInt("UI_bloom") == 1){
			bloomToggle.GetComponent<Toggle>().isOn = true;
		}
		else{
			bloomToggle.GetComponent<Toggle>().isOn = false;
		}
	}

	//Applys the settings from PlayerPrefs
	void ApplyGameplaySettings () {
		
		difficultySlider.GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("UI_difficulty");
		FoVSlider.GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("UI_FoV");
		
		if(PlayerPrefs.GetInt("UI_invert") == 1){
			invertToggle.GetComponent<Toggle>().isOn = true;
		}
		else{
			invertToggle.GetComponent<Toggle>().isOn = false;
		}

		if(PlayerPrefs.GetInt("UI_gimble") == 1){
			gimbleToggle.GetComponent<Toggle>().isOn = true;
		}
		else{
			gimbleToggle.GetComponent<Toggle>().isOn = false;
		}
	}

	//Applys the settings from Playerprefs
	void ApplyAudioSettings () {

		//Sliders
		volumeSlider.GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("UI_volume");
		effectsVSlider.GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("UI_effects");
		musicVSlider.GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("UI_music");

		//Toggle
		if(PlayerPrefs.GetInt("UI_playInBG") == 1){
			playInBGToggle.GetComponent<Toggle>().isOn = true;
		}
		else{
			playInBGToggle.GetComponent<Toggle>().isOn = false;
		}
	}

	//Updates Advanced Video Sliders when Overall Quality is changed
	void UpdateAVideoSliders () {

		//Texture Quality Slider
		int tQ = QualitySettings.masterTextureLimit;

		switch (tQ) {
		
		case 0:
			textureQSlider.GetComponent<Slider>().value = 3;
			break;
		case 1:
			textureQSlider.GetComponent<Slider>().value = 2;
			break;
		case 2:
			textureQSlider.GetComponent<Slider>().value = 1;
			break;
		case 3:
			textureQSlider.GetComponent<Slider>().value = 0;
			break;
		}

		//Antialiasing
		int aa = QualitySettings.antiAliasing;

		switch (aa) {
			
		case 0:
			aaSlider.GetComponent<Slider>().value = 0;
			aaText.GetComponent<Text>().text = "Off";
			break;
		case 2:
			aaSlider.GetComponent<Slider>().value = 1;
			aaText.GetComponent<Text>().text = "2x";
			break;
		case 4:
			aaSlider.GetComponent<Slider>().value = 2;
			aaText.GetComponent<Text>().text = "4x";
			break;
		case 8:
			aaSlider.GetComponent<Slider>().value = 3;
			aaText.GetComponent<Text>().text = "8x";
			break;
		}

		//Aniascrtripsosodoppy filtering
		if(QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable){
			anistropicTSlider.GetComponent<Slider>().value = 0;
			aTextureText.GetComponent<Text>().text = "Disabled";
		}
		else if(QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable){
			anistropicTSlider.GetComponent<Slider>().value = 1;
			aTextureText.GetComponent<Text>().text = "Enabled";
		}
		else if(QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable){
			anistropicTSlider.GetComponent<Slider>().value = 2;
			aTextureText.GetComponent<Text>().text = "ForceEnabled";
		}
		else {
			//Debug.Log ("ui", "Anistripic Filtering is fucked");
		}


	}

	//Things
	IEnumerator loadImgFromWeb() {
		WWW www = new WWW(url);
		yield return www;
		urlTar.GetComponent<RawImage>().texture = www.texture;
	}
}



