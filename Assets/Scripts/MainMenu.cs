	using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using AppAdvisory.Ads;
using UnityEngine.EventSystems;
//*****************************************//
//This class controls the overall Main Menu
//****************************************//
public class MainMenu : MonoBehaviour
{
	public GameObject adspanel;
	public GameObject level_selection;
	public GameObject bus_selection;
	public GameObject buy_bus_button;
	public GameObject black_image;
	public GameObject menu_buttons;
	public GameObject menu_Exit;
	public GameObject[] menu_buses = new GameObject[0];
	public GameObject[] game_levels = new GameObject[0];
	public GameObject locked_label;
	public GameObject select_bus_button;
	public GameObject loading_screen;
	public GameObject exit_dialog;
	public Text earning_text,earning_Text_Menu;
	public Text bus_prize_text;
	int bus_index = 0;
	public int[] bus_prizes = new int[0];
	public AudioSource audio_source;
	public GameObject mainBackGround;
	public AudioClip defualtClip;
	public AudioClip backClip;
	public AudioClip unlockBusClip;
	public GameObject unityRewadedAdDialog;
	public GameObject unlockAllButton;
	public GameObject removeAdsButton;
	public GameObject unlockAllButton2;
	public GameObject removeAdsButton2;
	public GameObject inSufficientCashDialog;

	private Text userEarningText;
	void SetBlackScreen ()
	{
		black_image.SetActive (false);
	}

	void ShowUnlockedLevels ()
	{
		int levels = PlayerPrefs.GetInt ("LevelLocked");
		for (int i = 1; i <= levels; i++) {
			game_levels [i].transform.Find ("Locked").gameObject.SetActive (false);
			game_levels [i].transform.GetComponent<Button> ().enabled = true;
		}
	}

	void IsLocked ()
	{
		
		if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 0) {
			select_bus_button.SetActive (true);
			buy_bus_button.SetActive (false);
			bus_prize_text.text = "";
			locked_label.SetActive (false);

		} else if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 1) {
			bus_prize_text.text = bus_prizes [bus_index] + "$";
			if (PlayerPrefs.GetInt ("UserEarning") > bus_prizes [bus_index] && bus_index != 0) {
			//	buy_bus_button.SetActive (true);
				select_bus_button.SetActive (false);
			} else {
				select_bus_button.SetActive (false);
			//	buy_bus_button.SetActive (false);
			}
			buy_bus_button.SetActive (true);
			locked_label.SetActive (true);

		}
	}

	public void NextBus ()
	{
		audio_source.clip = defualtClip;
		audio_source.Play ();
		if (bus_index < menu_buses.Length - 1) {			
			menu_buses [bus_index].SetActive (false);
			bus_index += 1;
			IsLocked ();
			menu_buses [bus_index].SetActive (true);
		}
		if (bus_index == 0) {
			bus_prize_text.text = "";
		}
	}

	public void LastBus ()
	{
		audio_source.clip = defualtClip;
		audio_source.Play ();
		if (bus_index > 0) {
			menu_buses [bus_index].SetActive (false);
			bus_index -= 1;
			IsLocked ();
			menu_buses [bus_index].SetActive (true);
		}
		if (bus_index == 0) {
			bus_prize_text.text = "";
		}
	}

	public void BuyBusButton ()
	{
		if (PlayerPrefs.GetInt ("UserEarning") > bus_prizes [bus_index] && bus_index != 0) {
			PlayerPrefs.SetInt (menu_buses [bus_index].name, 0);
			int deduct_amount = (PlayerPrefs.GetInt ("UserEarning") - bus_prizes [bus_index]);
			PlayerPrefs.SetInt ("UserEarning", deduct_amount); 
			earning_text.text = "" + PlayerPrefs.GetInt ("UserEarning");
			locked_label.SetActive (false);
			select_bus_button.SetActive (true);
			buy_bus_button.SetActive (false);
			audio_source.clip = unlockBusClip;
			audio_source.Play ();
		} else {
			inSufficientCashDialog.SetActive (true);
			//unityRewadedAdDialog.SetActive (true);	
		}
	}

	void ShowBusSelection ()
	{		
		if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 0) {			
			select_bus_button.SetActive (true);
		} else {
			select_bus_button.SetActive (false);
			locked_label.SetActive (true);
		}

		menu_buttons.SetActive (false);
		bus_selection.SetActive (true);
		mainBackGround.SetActive (false);
		Invoke ("SetBlackScreen", 0.75f);
		Invoke ("NowShowBus", 0.15f);
	}

	void NowShowBus ()
	{
		menu_buses [bus_index].SetActive (true);
	}

	void ShowLevelSelection ()
	{
		
		bus_selection.SetActive (false);
		level_selection.SetActive (true);
		Invoke ("SetBlackScreen", 0.75f);
	}

	public void RemoveAds(){
//mm		Purchaser.Instance.RemoveAds ();
	}
	void IsForGrage(){
		GameManager.Instance.isForGarage = false;
		earning_text.text = "" + PlayerPrefs.GetInt ("UserEarning");
		if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 0) {			
			select_bus_button.SetActive (true);
		} else {
			select_bus_button.SetActive (false);
			locked_label.SetActive (true);
		}
		menu_buttons.SetActive (false);
		bus_selection.SetActive (true);
		mainBackGround.SetActive (false);
		Invoke ("SetBlackScreen", 0.75f);
		Invoke ("NowShowBus", 0.15f);

	}
	public void PlayGameButton ()
	{
//mm		AdManager.instance.ShowInterstitial ();
//		if(AdsManager.instance.IsReadyInterstitial())
//		AdsManager.instance.ShowInterstitial ();
		earning_text.text = "" + PlayerPrefs.GetInt ("UserEarning");
		audio_source.clip = defualtClip;
		audio_source.Play ();
		black_image.SetActive (true);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 1f, 0.25f);
		Invoke ("ShowBusSelection", 0.5f);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 0f, 0.75f).setDelay (0.5f);

	}
	public void ExitBtn(){
		menu_Exit.SetActive (true);
	}
	public void ExitYes(){
		Application.Quit ();
	}
	public void ExitNo(){
		menu_Exit.SetActive (false);
	}
	public void FreeCashButton(){
		unityRewadedAdDialog.SetActive (true);
	}
	public void SelectBusButton ()
	{
//		if(AdsManager.instance.IsReadyInterstitial())
//		AdsManager.instance.ShowInterstitial ();

		GameManager.Instance.isToBusUnLock=false;
		audio_source.clip = defualtClip;
		audio_source.Play ();
		if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 1) {
		    int deduct_amount = (PlayerPrefs.GetInt ("UserEarning") - bus_prizes [bus_index]);
			PlayerPrefs.SetInt ("UserEarning", deduct_amount); 
					earning_text.text = "" + PlayerPrefs.GetInt ("UserEarning");
			PlayerPrefs.SetInt (menu_buses [bus_index].name, 0);
		}
		GameManager.Instance.bus_number = bus_index;
		GameManager.Instance.bus_name = menu_buses [bus_index].name;
		PlayerPrefs.SetInt ("SelectedBus",bus_index);
		locked_label.SetActive (false);
		black_image.SetActive (true);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 1f, 0.25f);
		Invoke ("ShowLevelSelection", 0.5f);
		menu_buses [bus_index].SetActive (false);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 0f, 0.75f).setDelay (0.5f);
	}

	public void BackToMainScreen ()
	{
		audio_source.clip = backClip;
		audio_source.Play ();
		black_image.SetActive (true);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 1f, 0.25f);
		Invoke ("SetMainScreen", .5f);
		menu_buses [bus_index].SetActive (false);
		locked_label.SetActive (false);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 0f, 0.75f).setDelay (0.5f);
	}

	public void BackToBusSelection ()
	{
		audio_source.clip = backClip;
		audio_source.Play ();
		black_image.SetActive (true);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 1f, 0.25f);
		Invoke ("SetBackBusSelection", 0.5f);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 0f, 0.75f).setDelay (0.5f);
	}

	void SetBackBusSelection ()
	{
		if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 0) {
			select_bus_button.SetActive (true);
		}
		level_selection.SetActive (false);
		bus_selection.SetActive (true);
		menu_buses [bus_index].SetActive (true);
		Invoke ("SetBlackScreen", 0.75f);
	}

	void SetMainScreen ()
	{
		menu_buttons.SetActive (true);
		bus_selection.SetActive (false);
		mainBackGround.SetActive (true);
		Invoke ("SetBlackScreen", 0.75f);

	}

	public void LoadGame ()
	{
//mm		AdManager.instance.ShowInterstitial ();
//		print("load game");
		/*if (Application.internetReachability != NetworkReachability.NotReachable) {
			AdsManager._instance.ShowInterstitial ();	
			adspanel.SetActive (true);
		} else {
			adspanel.SetActive (false);
		}*/

		print("play admob--");
		audio_source.clip = defualtClip;
		audio_source.Play ();

		level_selection.SetActive (false);
		loading_screen.SetActive (true);
		LeanTween.alpha (loading_screen.GetComponent<RectTransform> (), 1f, 0.35f);		
		Invoke ("LoadGamePlayScene", 0.5f);
	}
	void LoadGamePlayScene(){
		SceneManager.LoadScene ("GamePlay");
	}
	GameObject lastSelectedObj;

	//Set the level time by selecting the level number
	public void LevelSelection (int level_number)
	{
		
		if (lastSelectedObj == null) {
			lastSelectedObj = EventSystem.current.currentSelectedGameObject;
			LeanTween.scale (lastSelectedObj, new Vector2 (1.1f, 1.1f), 0.2f);
		} else {
			LeanTween.scale (lastSelectedObj, new Vector2 (1.0f, 1.0f), 0.2f);
			lastSelectedObj = EventSystem.current.currentSelectedGameObject;
			LeanTween.scale (lastSelectedObj, new Vector2 (1.1f, 1.1f), 0.2f);
		}
//		if (level_number == 0) {
//			GameManager.Instance.mission_minutes = 2;
//			GameManager.Instance.mission_seconds = 30;
//		} else if (level_number == 1) {
//			GameManager.Instance.mission_minutes = 3;
//			GameManager.Instance.mission_seconds = 45;
//		} else if (level_number == 2) {
//			GameManager.Instance.mission_minutes = 4;
//			GameManager.Instance.mission_seconds = 15;
//		} else if (level_number == 3) {
//			GameManager.Instance.mission_minutes = 4;
//			GameManager.Instance.mission_seconds = 45;
//		} else if (level_number == 4) {
//			GameManager.Instance.mission_minutes = 5;
//			GameManager.Instance.mission_seconds = 20;
//		} else if (level_number == 5) {
//			GameManager.Instance.mission_minutes = 7;
//			GameManager.Instance.mission_seconds = 0;
//		} else if (level_number == 6) {
//			GameManager.Instance.mission_minutes = 8;
//			GameManager.Instance.mission_seconds = 45;
//		} else if (level_number == 7) {
//			GameManager.Instance.mission_minutes = 9;
//			GameManager.Instance.mission_seconds = 15;
//		} else if (level_number == 8) {
//			GameManager.Instance.mission_minutes = 10;
//			GameManager.Instance.mission_seconds = 0;
//		} else if (level_number == 9) {
//			GameManager.Instance.mission_minutes = 11;
//			GameManager.Instance.mission_seconds = 0;
//		}
//		else if (level_number == 10) {
//			GameManager.Instance.mission_minutes = 12;
//			GameManager.Instance.mission_seconds = 15;
//		}
//		else if (level_number == 11) {
//			GameManager.Instance.mission_minutes = 12;
//			GameManager.Instance.mission_seconds = 30;
//		}
//		else if (level_number == 12) {
//			GameManager.Instance.mission_minutes = 13;
//			GameManager.Instance.mission_seconds = 15;
//		}
//		else if (level_number == 13) {
//			GameManager.Instance.mission_minutes = 13;
//			GameManager.Instance.mission_seconds = 45;
//		}
//		else if (level_number == 14) {
//			GameManager.Instance.mission_minutes = 14;
//			GameManager.Instance.mission_seconds = 0;
//		}
//		else if (level_number == 15) {
//			GameManager.Instance.mission_minutes = 15;
//			GameManager.Instance.mission_seconds = 30;
//		}
//		else if (level_number == 16) {
//			GameManager.Instance.mission_minutes = 16;
//			GameManager.Instance.mission_seconds = 15;
//		}
//		else if (level_number == 17) {
//			GameManager.Instance.mission_minutes = 17;
//			GameManager.Instance.mission_seconds = 0;
//		}
//		else if (level_number == 18) {
//			GameManager.Instance.mission_minutes = 17;
//			GameManager.Instance.mission_seconds = 30;
//		}
//		else if (level_number == 19) {
//			GameManager.Instance.mission_minutes = 18;
//			GameManager.Instance.mission_seconds = 0;
//		}

		GameManager.Instance.mission_no = level_number;
		audio_source.clip = defualtClip;
		audio_source.Play ();
		//Invoke ("LoadGame", 0.5f);
	}

	public void NoExitButton ()
	{
		audio_source.clip = defualtClip;
		audio_source.Play ();
		LeanTween.scale (exit_dialog, new Vector3 (0.3f, 0.3f, 0.3f), 0.25f);
		Invoke ("SetNoExitButton", 0.3f);
	}

	public void YesExitButton ()
	{
		audio_source.clip = backClip;
		audio_source.Play ();
		Application.Quit ();
	}

	void SetNoExitButton ()
	{
		exit_dialog.SetActive (false);
	}
	public void RateUsButton(){
		audio_source.clip = defualtClip;
		audio_source.Play ();
		Application.OpenURL ("");
	}
	public void MoreApps(){
		audio_source.clip = defualtClip;
		audio_source.Play ();
		Application.OpenURL ("");
	}
	public void PrivacyPolicy(){
		audio_source.clip = defualtClip;
		audio_source.Play ();
		Application.OpenURL ("");
	}
	public void FreeCash(){
		//AdsManager._instance.ShowUnityRewardedvideo ();
		PlayerPrefs.SetInt ("FreeCash",1);
	}
	void CheckIAPStatus(){
		if (PlayerPrefs.GetInt ("UnlockAllBuses") == 6676) {
			unlockAllButton.GetComponent<Button> ().enabled = false;
			unlockAllButton.GetComponent<Button> ().interactable = false;
			unlockAllButton.GetComponent<Image> ().color = Color.gray;
			unlockAllButton2.GetComponent<Button> ().enabled = false;
			unlockAllButton2.GetComponent<Button> ().interactable = false;
			unlockAllButton2.GetComponent<Image> ().color = Color.gray;
		}
		if (PlayerPrefs.GetInt ("RemveAds") == 6676) {
			removeAdsButton.GetComponent<Button> ().enabled = false;
			removeAdsButton.GetComponent<Button> ().interactable = false;
			removeAdsButton.GetComponent<Image> ().color = Color.gray;
			removeAdsButton2.GetComponent<Button> ().enabled = false;
			removeAdsButton2.GetComponent<Button> ().interactable = false;
			removeAdsButton2.GetComponent<Image> ().color = Color.gray;

		}
	}
    public void UnlockAll()
    {
//		Purchaser.Instance.UnlockAll();
		if (PlayerPrefs.GetInt ("UnlockAllBuses") == 6676) {
			audio_source.clip = defualtClip;
			audio_source.Play ();
			UnlockAllBuses ();
			UnlockAllLevels ();
		}
    }
	 void UnlockAllBuses(){
		for (int i = 1; i < menu_buses.Length; i++) {
			PlayerPrefs.SetInt (menu_buses [i].name, 0);
		}
	}

    public void UnlockAllLevels()
    {
		audio_source.clip = defualtClip;
		audio_source.Play ();
        PlayerPrefs.SetInt("LevelLocked", 14);
    }

	//If running first time set the game to defualt state
	void IsRunningFirstTime ()
	{
			PlayerPrefs.SetInt ("UserEarning", 500);
			PlayerPrefs.SetInt ("IAmRunningFirstTime", 6676);
			PlayerPrefs.SetInt ("LevelLocked", 0);
			for (int i = 1; i < menu_buses.Length; i++) {
				PlayerPrefs.SetInt (menu_buses [i].name, 1);
			}
			GameManager.Instance.mission_no = 0;
			GameManager.Instance.bus_number = 0;
			GameManager.Instance.current_bus_index = 0;

	}

	void Start ()
	{
//		PlayerPrefs.DeleteAll ();

		Time.timeScale = 1;
		CheckIAPStatus ();
		//PlayerPrefs.SetInt ("IAmRunningFirstTime", 0);
		if (PlayerPrefs.GetInt ("UserEarning") < 500) {
			PlayerPrefs.SetInt ("UserEarning", 500);
		}
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if (PlayerPrefs.GetInt ("IAmRunningFirstTime") == 0) {
			PlayerPrefs.SetInt ("IAmRunningFirstTime", 6676);
			IsRunningFirstTime ();
		}
		//bus_index = GameManager.Instance.current_bus_index;

		if (PlayerPrefs.GetInt (menu_buses [bus_index].name) == 1) {
			select_bus_button.SetActive (false);
			locked_label.SetActive (true);
		}
		earning_text.text = "" + PlayerPrefs.GetInt ("UserEarning");
		ShowUnlockedLevels ();
		if (GameManager.Instance.isToBusUnLock) {
			bus_index = GameManager.Instance.unlockBusIndex;
			PlayGameButton ();
		} else {
			bus_index=PlayerPrefs.GetInt("SelectedBus");
		}
		IsLocked();
		if(GameManager.Instance.isForGarage)
		IsForGrage ();
		//AdsManager.instance.ShowInterstitial ();
		//		Adds.instance.LogGoogleAnalytics ();

		userEarningText = GameObject.Find("UserEarningText").GetComponent<Text>();
	}

	void Update ()
	{
		earning_Text_Menu.text = "" + PlayerPrefs.GetInt ("UserEarning");
		userEarningText.text = "" + PlayerPrefs.GetInt ("UserEarning");
		if (Input.GetKeyDown (KeyCode.Escape)) {
			exit_dialog.SetActive (true);
			LeanTween.scale (exit_dialog, new Vector3 (1, 1, 1), 0.25f);
		}

		if(PlayerPrefs.GetInt ("FreeCash", 1)==1){
			//Give Reward
			if (PlayerPrefs.GetInt("rewardvdo")==1) {
				PlayerPrefs.SetInt ("UserEarning", PlayerPrefs.GetInt ("UserEarning") + 200);
				earning_Text_Menu.text = "" + PlayerPrefs.GetInt ("UserEarning");
				PlayerPrefs.SetInt ("rewardvdo", 0);
			}
			PlayerPrefs.SetInt ("FreeCash", 0);
		}
		else if(PlayerPrefs.GetInt ("rewardvdo", 1)==2){
			//Video Failed
			PlayerPrefs.SetInt ("rewardvdo", 0);
		}
	}
}
