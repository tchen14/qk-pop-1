using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestManagerUIController : MonoBehaviour {

	GameObject player;
	public GameObject questContainer;
	QuestManager qm;
	public GameObject questUI;
	Button questButton;
	float buttonHeight;
	public float spacing;
	GameObject moreQuestInfo;
	Text moreQuestInfoTitle;
	Text moreQuestInfoDescription;
	Scrollbar mainScrollbar;
	Scrollbar moreInfoScrollbar;
	bool mainSelected;
	bool isScrolling;
	float newScrollVal;
	EventSystem theEventSystem;
	float qcHeight;
	int lastButtonSelected;
	//GameObject[] buttons;
	List<GameObject> allQuests;
	List<Quest>[] theLists;
	
	void Start(){
		player = GameObject.Find ("_Player");
		if (player) {
			qm = player.GetComponent<QuestManager>();
		}
		else {
			Debug.LogError("QuestManagerUI Script attached to 'QuestManager' object could not find a player in the scene!");
		}
		//questContainer = this.transform.Find ("Quests").gameObject;
		if (!questContainer) {
			Debug.LogError("QuestManagerUI Script attached to 'QuestManager' object could not find a child GameObject called 'Quests' the prefab connection could be broken.");
		}
		moreQuestInfo = transform.FindChild ("MoreQuestInfo").gameObject;
		if (!moreQuestInfo) {
			Debug.LogError ("QuestManagerUI script could not find the child object called 'MoreQuestInfo' the prefab may be broken");
		}
		moreQuestInfoTitle = moreQuestInfo.transform.FindChild ("QuestTitle").GetComponent<Text> ();
		if (!moreQuestInfoTitle) {
			Debug.LogError ("QuestManagerUI script could not find the child object called 'QuestTitle' the prefab may be broken");
		}
		moreQuestInfoDescription = moreQuestInfo.transform.FindChild ("ScrollView").transform.FindChild ("QuestDescription").GetComponent<Text> ();
		if (!moreQuestInfoDescription) {
			Debug.LogError ("QuestManagerUI script could not find the child object called 'QuestDescription' the prefab may be broken");
		}
		mainScrollbar = transform.FindChild("MainScrollbar").GetComponent<Scrollbar>();
		if(!mainScrollbar){
			Debug.LogError ("QuestManagerUI script could not find the child object called 'MainScrollbar' the prefab may be broken");
		}
		moreInfoScrollbar = transform.FindChild("MoreQuestInfo").FindChild("MoreQuestInfoScrollbar").GetComponent<Scrollbar>();
		if(!moreInfoScrollbar){
			Debug.LogError ("QuestManagerUI script could not find the child object called 'MoreQuestInfoScrollbar' the prefab may be broken");
		}
		theEventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();
		if(!theEventSystem){
			Debug.LogError("QuestManagerUI script could not find the EventSystem in the scene. Make sure the scene has an EventSystem");
		}
		questButton = questUI.GetComponent<Button> ();
		buttonHeight = questButton.GetComponent<RectTransform> ().sizeDelta.y;
		theLists = new List<Quest>[3];
		theLists[0] = qm.currentQuests;
		theLists[1] = qm.failedQuests;
		theLists[2] = qm.completedQuests;
		
		for(int i = 0; i < theLists.Length; i++){
			qcHeight += (theLists[i].Count * (buttonHeight + spacing) - spacing);
		}
		
		/*qcHeight = (qm.questCount * (buttonHeight + spacing) - spacing);
		qcHeight += (qm.completedQuests.Count * (buttonHeight + spacing) - spacing);
		qcHeight += (qm.failedQuests.Count * (buttonHeight + spacing) - spacing);*/
		qm.LoadQuests ();
	}

	void Update(){
		//For debugging, remove later.
		if (Input.GetKeyDown (KeyCode.F5)) {
			showQuests();
		}
		if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)) && mainSelected){
			isScrolling = true;
		}
		else{
			isScrolling = false;
		}
		if(isScrolling){
			mainScrollbar.value = 1 - ((Mathf.Abs(theEventSystem.currentSelectedGameObject.transform.localPosition.y) - 50.5f) / (qcHeight - 101f));
			Debug.Log("Name: " + theEventSystem.currentSelectedGameObject.name + "\nVal: " + Mathf.Abs(theEventSystem.currentSelectedGameObject.transform.localPosition.y).ToString() + "\nqcHeight: " + qcHeight);
		}
		if(Input.GetKeyDown (KeyCode.Escape) && !mainSelected){
			//buttons[lastButtonSelected].GetComponent<Button>().Select();
			allQuests[lastButtonSelected].GetComponent<Button>().Select();
			mainSelected = true;
		}
	}

	public void showQuests(){
		removeQuestUIobjects ();
		reorganizeQuests ();
	}

	public void addNewQuest(){

	}

	/* This function is called when the quest UI elements need to be reorganized.
	 * Some times when this would be called is when attempting to add a quest or a quest is completed.
	 * This handles the calculation of the size of the scrollable area and physical placement of the quest UI elements in the quest manager.
	 * This function will also call the sort method on the Quests list to organize by completion. Completed items will fall to the bottom.
	 */
	public void reorganizeQuests(){
		/*qm.currentQuests.Sort (delegate(Quest q1, Quest q2) {
			return q1.GetName().CompareTo(q2.GetName ());
		});*/
		/*qm.currentQuests.Sort (delegate(Quest q1, Quest q2) {
			return q1.IsCompleted().CompareTo(q2.IsCompleted ());
		});*/
		//qm.currentQuests.Sort ();
		allQuests = new List<GameObject> ();
		mainSelected = true;
		moreQuestInfoTitle.text = "";
		moreQuestInfoDescription.text = "";
		//qcHeight = (qm.questCount * (buttonHeight + spacing) - spacing);
		//Debug.Log ("Com quest count" + qm.completedQuests.Count);
		//qcHeight += (qm.completedQuests.Count * (buttonHeight + spacing) - spacing);
		//qcHeight += (qm.failedQuests.Count * (buttonHeight + spacing) - spacing);
		for(int i = 0; i < theLists.Length; i++){
			qcHeight += (theLists[i].Count * (buttonHeight + spacing) - spacing);
		}
		RectTransform containerTransform = questContainer.GetComponent<RectTransform> ();
		containerTransform.sizeDelta = new Vector2 (100, qcHeight);
		newScrollVal = ((buttonHeight + spacing) + spacing) / qcHeight;
		mainScrollbar.value = 0.99f;
		moreInfoScrollbar.value = 0.99f;
		//buttons = new GameObject[qm.questCount];

		/*int loopHelper = 0;
		for (int i = 0; i < qm.questCount; i++) {
			GameObject newQuestButton = Instantiate(questUI, new Vector3(0, 0 - (i * (buttonHeight + spacing) + (buttonHeight/2)), 0), Quaternion.identity) as GameObject;
			newQuestButton.transform.SetParent(questContainer.transform, false);
			//buttons[i] = newQuestButton;
			allQuests.Add(newQuestButton);
			Text newButtonText = newQuestButton.transform.FindChild ("Text").GetComponent<Text>();
			newButtonText.text = qm.currentQuests[i].GetName();
			Button qb = newQuestButton.GetComponent<Button>();
			addListener(qb, i);
			if (i == 0){
				qb.Select();
			}
			loopHelper = i + 1;
		}
		for (int l = loopHelper, i = 0; i < qm.failedQuests.Count; i ++, l++) {
			GameObject newQuestButton = Instantiate(questUI, new Vector3(0, 0 - (l * (buttonHeight + spacing) + (buttonHeight/2)), 0), Quaternion.identity) as GameObject;
			newQuestButton.transform.SetParent(questContainer.transform, false);
			//buttons[l] = newQuestButton;
			allQuests.Add(newQuestButton);
			Text newButtonText = newQuestButton.transform.FindChild ("Text").GetComponent<Text>();
			newButtonText.text = qm.failedQuests[i].GetName();
			Button qb = newQuestButton.GetComponent<Button>();
			addListener(qb, l);
			loopHelper = l + 1;
		}
		for (int l = loopHelper, i = 0; i < qm.completedQuests.Count; i++, l++) {
			GameObject newQuestButton = Instantiate(questUI, new Vector3(0, 0 - (l * (buttonHeight + spacing) + (buttonHeight/2)), 0), Quaternion.identity) as GameObject;
			newQuestButton.transform.SetParent(questContainer.transform, false);
			//buttons[l] = newQuestButton;
			allQuests.Add(newQuestButton);
			Text newButtonText = newQuestButton.transform.FindChild ("Text").GetComponent<Text>();
			newButtonText.text = qm.completedQuests[i].GetName();
			Button qb = newQuestButton.GetComponent<Button>();
			addListener(qb, l);
		}*/
		int iter = 0;
		for(int i = 0; i < theLists.Length; i++){
			for(int j = 0; j < theLists[i].Count; j++){
				GameObject newQuestButton = Instantiate(questUI, new Vector3(0, 0 - (iter * (buttonHeight + spacing) + (buttonHeight/2)), 0), Quaternion.identity) as GameObject;
				newQuestButton.transform.SetParent(questContainer.transform, false);
				allQuests.Add(newQuestButton);
				Text newButtonText = newQuestButton.transform.FindChild("Text").GetComponent<Text>();
				newButtonText.text = theLists[i][j].GetName();
				Button qb = newQuestButton.GetComponent<Button>();
				addListener(qb, iter);
				if (i == 0 && j == 0){
					qb.Select();
				}
				iter++;
			}
		}
		if (qm.questCount > 0) {
		
			showMoreQuestInfo(0);
		}
		/*mainScrollbar.value = 0.99f;
		moreInfoScrollbar.value = 0.99f;
		*/
		if (qm.questCount < 7) {
			GameObject scrollingHandle = mainScrollbar.transform.FindChild ("Sliding Area").transform.FindChild ("Handle").gameObject;
			scrollingHandle.SetActive (false);
		}
		else {
			GameObject scrollingHandle = mainScrollbar.transform.FindChild ("Sliding Area").transform.FindChild ("Handle").gameObject;
			scrollingHandle.SetActive (true);
		}
	}

	void addListener(Button b, int i){
		b.onClick.AddListener (() => clickButton (i));
	}

	void clickButton(int iter){
		mainSelected = false;
		lastButtonSelected = iter;
		showMoreQuestInfo(iter);
		moreInfoScrollbar.Select();
		moreInfoScrollbar.value = 1f;
		/*if(moreQuestInfoDescription.GetComponent<RectTransform>().rect.height < 560){
			moreInfoScrollbar.gameObject.SetActive(false);
		}
		else{
			moreInfoScrollbar.gameObject.SetActive(true);
		}*/
	}

	public void removeQuestUIobjects(){
		foreach (Transform child in questContainer.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
	
	void showMoreQuestInfo(int iter){
		Debug.Log (iter.ToString ());
		int i;
		int j;
		if(iter < theLists[0].Count){
			i = 0;
			j = iter;
		}
		else if(iter < theLists[0].Count + theLists[1].Count){
			i = 1;
			j = iter - theLists[0].Count;
		}
		else{
			i = 2;
			j = iter - theLists[0].Count - theLists[1].Count;
		}
		moreQuestInfoTitle.text = theLists[i][j].GetName();
		Goal[] currQuestGoals = theLists[i][j].GetGoal();
		string goalText = "\n";
		if(theLists[i][j].HasTimer()){
			goalText += "Quest Duration: " + theLists[i][j].GetTimerLength () + " Seconds";
		}
		for(int x = 0; x < currQuestGoals.Length; x++){
			goalText += "\n" + currQuestGoals[x].GetName() + ": ";
			if(currQuestGoals[x].GetProgress() == -1){
				if(currQuestGoals[x].IsCompleted()){
					goalText += "Complete";
				}
				else{
					goalText += "Incomplete";
				}
			}
			else{
                //commented this out due to errors given 
				// goalText += currQuestGoals [i].GetProgress () + " / " + currQuestGoals [i].GetProgrssNeeded ();
			}
		}
        //commented this out due to errors given 
        //moreQuestInfoDescription.text = theLists[i][j].GetDescription() + "\n" + theLists[i][j].GetObjective() + goalText;
        GameObject moreQuestInfoScrollView = moreQuestInfo.transform.FindChild("ScrollView").gameObject;
		
		/*if (iter < qm.currentQuests.Count) {
			moreQuestInfoTitle.text = qm.currentQuests [iter].GetName ();
			Goal[] currQuestGoals = qm.currentQuests [iter].GetGoal ();
			string goalText = "\n";
			if (qm.currentQuests [iter].HasTimer ()) {
				goalText += "Quest Duration: " + qm.currentQuests [iter].GetTimerLength () + " Seconds";
			}
			for (int i = 0; i < currQuestGoals.Length; i++) {
				goalText += "\n" + currQuestGoals [i].GetName () + ": ";
				if (currQuestGoals [i].GetProgress () == -1) {
					if (currQuestGoals [i].IsCompleted ()) {
						goalText += "Complete";
					} else {
						goalText += "Incomplete";
					}
				} else {
					goalText += currQuestGoals [i].GetProgress () + " / " + currQuestGoals [i].GetProgrssNeeded ();
				}
			}
			moreQuestInfoDescription.text = qm.currentQuests [iter].GetDescription () + "\n" + qm.currentQuests [iter].GetObjective () + goalText;
		} else if (iter < qm.currentQuests.Count + qm.failedQuests.Count) {
			moreQuestInfoTitle.text = qm.failedQuests [iter - qm.currentQuests.Count].GetName ();
			Goal[] currQuestGoals = qm.failedQuests [iter - qm.currentQuests.Count].GetGoal ();
			string goalText = "\n";
			if (qm.failedQuests [iter - qm.currentQuests.Count].HasTimer ()) {
				goalText += "Quest Duration: " + qm.failedQuests [iter - qm.currentQuests.Count].GetTimerLength () + " Seconds";
			}
			for (int i = 0; i < currQuestGoals.Length; i++) {
				goalText += "\n" + currQuestGoals [i].GetName () + ": ";
				if (currQuestGoals [i].GetProgress () == -1) {
					if (currQuestGoals [i].IsCompleted ()) {
						goalText += "Complete";
					} else {
						goalText += "Incomplete";
					}
				} else {
					goalText += currQuestGoals [i].GetProgress () + " / " + currQuestGoals [i].GetProgrssNeeded ();
				}
			}
			moreQuestInfoDescription.text = qm.failedQuests [iter - qm.currentQuests.Count].GetDescription () + "\n" + qm.failedQuests [iter - qm.currentQuests.Count].GetObjective () + goalText;
		} 
		else {
			moreQuestInfoTitle.text = qm.completedQuests [iter - (qm.currentQuests.Count + qm.failedQuests.Count)].GetName ();
			Goal[] currQuestGoals = qm.completedQuests [iter - (qm.currentQuests.Count + qm.failedQuests.Count)].GetGoal ();
			string goalText = "\n";
			if (qm.currentQuests [iter - (qm.currentQuests.Count + qm.failedQuests.Count)].HasTimer ()) {
				goalText += "Quest Duration: " + qm.completedQuests [iter - (qm.currentQuests.Count + qm.failedQuests.Count)].GetTimerLength () + " Seconds";
			}
			for (int i = 0; i < currQuestGoals.Length; i++) {
				goalText += "\n" + currQuestGoals [i].GetName () + ": ";
				if (currQuestGoals [i].GetProgress () == -1) {
					if (currQuestGoals [i].IsCompleted ()) {
						goalText += "Complete";
					} else {
						goalText += "Incomplete";
					}
				} else {
					goalText += currQuestGoals [i].GetProgress () + " / " + currQuestGoals [i].GetProgrssNeeded ();
				}
			}
			moreQuestInfoDescription.text = qm.completedQuests [iter - (qm.currentQuests.Count + qm.failedQuests.Count)].GetDescription () + "\n" + qm.completedQuests [iter - (qm.currentQuests.Count + qm.failedQuests.Count)].GetObjective () + goalText;
		}*/
	}
}
