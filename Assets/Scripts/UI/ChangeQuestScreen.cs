using UnityEngine;
using System.Collections;

public class ChangeQuestScreen : MonoBehaviour {

	public GameObject quests;
	public GameObject questTitle;
	public GameObject scrollbar;

	GameObject moreQuestInfo;

	void Start(){
		moreQuestInfo = gameObject.transform.FindChild("MoreQuestInfo").gameObject;
	}

	public void showMoreInfo(){
		quests.SetActive (false);
		questTitle.SetActive (false);
		scrollbar.SetActive (false);
		moreQuestInfo.SetActive (true);
	}

	public void backToQuestlist(){
		quests.SetActive (true);
		questTitle.SetActive (true);
		scrollbar.SetActive (true);
		moreQuestInfo.SetActive (false);
	}

}
