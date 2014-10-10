using UnityEngine;
using System.Collections;

public class QuestSaveManager : SaveManager {

	public float checkUpdateSpeed = 1.0f;
	
	void Start()
	{
		StartCoroutine("SlowUpdate");
	}
	
	IEnumerator SlowUpdate()
	{
		
		yield return new WaitForSeconds(checkUpdateSpeed);
		StartCoroutine("SlowUpdate");
	}

	public void StartQuest(string questName){

	}

	public void StartQuestWithTimer(string questName, float time){
		
	}

	public void StartQuestWithCounter(string questName, int count, bool countdown = true){
		
	}

	public void StartQuestWithParts(string questName, string[] parts){
		
	}

	public void StartQuestWithTimer(string questName){
		
	}

	public void CompleteQuest(string questName){
		
	}

	public void TerminateQuest(string questName){
		
	}

	private void EndQuest(string questName){
		
	}
}
