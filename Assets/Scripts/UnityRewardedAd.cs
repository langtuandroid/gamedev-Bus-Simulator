using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.Advertisements;
//using AppAdvisory.Ads;

public class UnityRewardedAd : MonoBehaviour
{
	public GameObject addNotAvailableDialog;
	public GameObject rewardDoneDialog;
	public Text userEarningText;
	//	void Start(){
	//		Advertisement.Initialize (Advertisement.gameId, true);
	//	}s
	void Update(){
		if(PlayerPrefs.GetInt("rewardvdo",0)==1){
			PlayerPrefs.SetInt ("rewardvdo", 0);
			PlayerPrefs.SetInt ("UserEarning", PlayerPrefs.GetInt ("UserEarning") + 200);
			GameObject.Find ("UserEarningText").GetComponent<Text> ().text = "" + PlayerPrefs.GetInt ("UserEarning");
			rewardDoneDialog.SetActive (true);
		}else if(PlayerPrefs.GetInt("rewardvdo",0)==2){
			PlayerPrefs.SetInt ("rewardvdo", 0);
			addNotAvailableDialog.SetActive (true);
		}
	}
	public void ShowRewardedAd ()
	{
//mm		AdManager.instance.ShowRewardedAd ("UnityRewardedAd");
		//AdsManager._instance.ShowUnityRewardedvideo();
		print ("rewarded btn click");

//		if (AdsManager.instance.IsReadyRewardedVideo ())
//			AdsManager.instance.ShowRewardedVideo (Method);
//		else if (AdsManager.instance.IsReadyVideoAds ()) {
//			AdsManager.instance.ShowVideoAds ();
//			PlayerPrefs.SetInt ("UserEarning", PlayerPrefs.GetInt ("UserEarning") + 2000);
//			GameObject.Find ("UserEarningText").GetComponent<Text> ().text = "" + PlayerPrefs.GetInt ("UserEarning");
//			rewardDoneDialog.SetActive (true);
//		} else {
//			addNotAvailableDialog.SetActive (true);
//		}
//		if (Advertisement.IsReady ("rewardedVideo")) {
//			var options = new ShowOptions { resultCallback = HandleShowResult };
//			Advertisement.Show ("rewardedVideo", options);
//		} else {
//		//	gameObject.SetActive (false);
//		}
	}

	public void Close ()
	{
		addNotAvailableDialog.SetActive (false);
		rewardDoneDialog.SetActive (false);
		gameObject.SetActive (false);
	}

	void Method (bool success)
	{	
		if (success) {
			Debug.Log ("Showed");
			PlayerPrefs.SetInt ("UserEarning", PlayerPrefs.GetInt ("UserEarning") + 2000);
			//GameObject.Find ("UserEarningText").GetComponent<Text>().text=""+PlayerPrefs.GetInt ("UserEarning");
			userEarningText.text=""+PlayerPrefs.GetInt ("UserEarning");
			rewardDoneDialog.SetActive (true);
		} else {	
			Debug.Log ("Not Showed");
			gameObject.SetActive (false);
		}	

	}
	public void OnAdCompleted(){
		PlayerPrefs.SetInt ("UserEarning", PlayerPrefs.GetInt ("UserEarning") + 2000);
		//GameObject.Find ("UserEarningText").GetComponent<Text>().text=""+PlayerPrefs.GetInt ("UserEarning");
		userEarningText.text=""+PlayerPrefs.GetInt ("UserEarning");
		rewardDoneDialog.SetActive (true);

	}
	public void OnAdFailed(){
		gameObject.SetActive (false);
	}

	//	private void HandleShowResult(ShowResult result)
	//	{
	//		switch (result)
	//		{
	//		case ShowResult.Finished:
	//			Debug.Log("The ad was successfully shown.");
	//			//
	//			// YOUR CODE TO REWARD THE GAMER
	//			// Give coins etc.
	//			break;
	//		case ShowResult.Skipped:
	//			Debug.Log("The ad was skipped before reaching the end.");
	//			break;
	//		case ShowResult.Failed:
	//			Debug.LogError("The ad failed to be shown.");
	//			break;
	//		}
	//	}
	//	void Update(){
	//		Debug.Log(Advertisement.IsReady("rewardedVideo"));
	//
	//	}
}
