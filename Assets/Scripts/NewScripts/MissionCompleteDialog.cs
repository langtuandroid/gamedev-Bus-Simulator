using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MissionCompleteDialog : MonoBehaviour {
	public GameController gameController;
	public AudioSource audioSource;
	public Text earningText;
	public Text timeText;
	public GameObject newBusUnlockNotification;
	public int [] busPrice=new int[0];
	void OnEnable(){
//mm		AdManager.instance.ShowInterstitial ();
		if (Application.internetReachability != NetworkReachability.NotReachable) {
		//	AdsManager._instance.ShowInterstitial ();
		//	gameController.adspanel.SetActive (true);
			Invoke ("missioncompleteads", 0.1f);
		} else {
			print("mission complete--");
			if (GameManager.Instance.mission_no < GameManager.Instance.totalMissions) {
				if (GameManager.Instance.mission_no >= PlayerPrefs.GetInt ("LevelLocked")) {
					PlayerPrefs.SetInt ("LevelLocked", (PlayerPrefs.GetInt ("LevelLocked") + 1));  		
				}
			}
			earningText.text = "" + GameManager.Instance.mission_reward;
			timeText.text = "" + GameManager.Instance.mission_end_time;
			audioSource.Play ();
			for (int i = 0; i < 6; i++) {
				string busName = "RCCBus" + (i + 1);
				if (PlayerPrefs.GetInt (busName) == 1){
					if (PlayerPrefs.GetInt ("UserEarning") > busPrice [i] ) {
						newBusUnlockNotification.SetActive (true);
						break;
					} else {
						newBusUnlockNotification.SetActive (false);
					}
				}
			}
		}


	}
	void missioncompleteads(){
		gameController.adspanel.SetActive (false);
		print("mission complete--");
		if (GameManager.Instance.mission_no < GameManager.Instance.totalMissions) {
			if (GameManager.Instance.mission_no >= PlayerPrefs.GetInt ("LevelLocked")) {
				PlayerPrefs.SetInt ("LevelLocked", (PlayerPrefs.GetInt ("LevelLocked") + 1));  		
			}
		}
		earningText.text = "" + GameManager.Instance.mission_reward;
		timeText.text = "" + GameManager.Instance.mission_end_time;
		audioSource.Play ();
		for (int i = 0; i < 6; i++) {
			string busName = "RCCBus" + (i + 1);
			if (PlayerPrefs.GetInt (busName) == 1){
				if (PlayerPrefs.GetInt ("UserEarning") > busPrice [i] ) {
					newBusUnlockNotification.SetActive (true);
					break;
				} else {
					newBusUnlockNotification.SetActive (false);
				}
			}
		}
	}
	public void Buttons(string name){
		switch (name) {
		case "Restart":
			gameController.isMissionComplete = false;
			SceneManager.LoadScene ("GamePlay");
			break;
		case "Menu":
			SceneManager.LoadScene ("NewMenu");
			break;
		case "Garage":
			GameManager.Instance.isForGarage = true;
			SceneManager.LoadScene ("NewMenu");
			break;
		case "Next":
			if (GameManager.Instance.mission_no + 1 >= gameController.levels.Length) break;
			int count = GameManager.Instance.bus_stops.Count;
			for (int i = 0; i < count; i++) {
				Destroy (GameManager.Instance.bus_stops [0]);			    
			}
			GameManager.Instance.bus_stops.Clear ();
			gameController.levels [GameManager.Instance.mission_no].SetActive (false);
			GameManager.Instance.mission_no += 1;
			if (GameManager.Instance.mission_no < GameManager.Instance.totalMissions)
				gameController.levels [GameManager.Instance.mission_no].SetActive (true);
			
			gameController.isMissionComplete = false;
			gameObject.SetActive (false);
			gameController.SetMissionState ();
			break;
		}
	}
}
