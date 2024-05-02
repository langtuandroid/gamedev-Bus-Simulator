using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//using AppAdvisory.Ads;
public class GameController : MonoBehaviour
{
	public GameObject adspanel;
	public Transform trafficSpwaner;
	public GameObject blackImage;
	public GameObject[] buses = new GameObject[0];
	public GameObject[] levels = new GameObject[0];
	public Transform[] busStartingPosition = new Transform[0];
	public Transform carCamera;
	public GameObject missionCompleteDialog;
	public GameObject missionFailedDialog;

	public MapCanvasController mapCanvasController;
	public RCC_Camera rccCam;
	[HideInInspector] public bool is_time_start = true;
	[HideInInspector] public bool pick_current_time = true;
	[HideInInspector] public int current_time = 0;
	[HideInInspector] public int minutes;
	[HideInInspector] public int seconds;
	string minutes_prefix = "";
	string seconds_prefix = "";
	public Text mission_time_text;
	public AudioSource beepAudioSource;
	public GameObject openDoorButton;
	MissionHandler missionHandler;
	[HideInInspector] public Transform bus;
	bool isDoorOpen = true;
	BusController busController;
	bool isMissionFailed = false;
	[HideInInspector] public bool isMissionComplete = false;
	float health = 100;
	public Image healthBar;
	public AudioSource audioSource;
	public AudioClip[] audioClips = new AudioClip[0];
	bool isHeadLightOn = false;
	Rigidbody rigid;
	public Image fuel_bar;
	public float fuel = 100;
	bool isBlinkFuel = true;
	[HideInInspector] public bool isFuelEmpty = true;
	int fuel_cusmption_rate = 2;
	public AudioSource fuelFillingAudioSourc;
	public AudioClip fuelEnd;
	public AudioClip fuelWarning;
	public GameObject fuelDialog;
	public Animator levelNumberAnimator;
	public Text levelNumberText;
	bool is_allow = true;
	public HowToPlay howToPlay;
	public GameObject watchAdToRefillTankDialog;
	public GameObject repairBusDialog;
	public Text missionCompleteTimeText;
	string tempTime = "";
	public GameObject objectiveDialog;

	public void Buttons (string name)
	{
		switch (name) {
		case "OpenDoor":
			if (isDoorOpen) {
				isDoorOpen = false;
				busController.OpenDoor ();
				audioSource.clip = audioClips [10];
				openDoorButton.SetActive (false);
			} else {
				busController.CloseDoor ();
				audioSource.clip = audioClips [11];
				isDoorOpen = true;
				openDoorButton.SetActive (false);
			}
			audioSource.Play ();
			if (GameManager.Instance.mission_no == 0)
				howToPlay.Next ();
			break;

		case "HeadLights":
			if (!isHeadLightOn) {
				busController.HeadLights (true);
				isHeadLightOn = true;
			} else {
				busController.HeadLights (false);
				isHeadLightOn = false;
			}
			break;

		}
	}

	public void FillTank (float tfuel)
	{
		isBlinkFuel = true;
		fuel = fuel + tfuel;
		fuel_bar.GetComponent<Image> ().color = Color.white;
		fuel_bar.fillAmount = fuel / 100;
		is_allow = true;

		if (fuel <= 0) {
			if (Application.internetReachability != NetworkReachability.NotReachable) {
			//	AdsManager._instance.ShowInterstitial ();
			//	adspanel.SetActive (true);
				Invoke ("fuelAds", 0.1f);
			} else {
				print ("level failed");
				isMissionFailed = true;
				missionFailedDialog.SetActive (true);
			}


		} else {
			isFuelEmpty = true;
		}
	}
	void fuelAds(){
		adspanel.SetActive (false);
		print ("level failed");
		isMissionFailed = true;
		missionFailedDialog.SetActive (true);
	}
	public void PlayPedestrainVoices (string name)
	{
		if (name == "Male") {
			audioSource.clip = audioClips [Random.Range (0, 5)];
		} else {
			audioSource.clip = audioClips [Random.Range (5, 10)];
		}
		audioSource.Play ();
	}

	public void ReduceBusHealth (float healthReduction)
	{
		health -= healthReduction;
		healthBar.fillAmount = health / 100;
		Debug.Log ("Reduction");
		if (healthBar.fillAmount <= 0.4f) {
			//healthBar.color = Color.red;
			LeanTween.alpha (healthBar.GetComponent<RectTransform> (), 0f, 0.35f).setLoopPingPong ();
		}
		if (healthBar.fillAmount <= 0) {
			repairBusDialog.SetActive (true);
			//isMissionFailed = true;
			//missionFailedDialog.SetActive (true);
		}
	}

	public void SetMissionState ()
	{
		Debug.Log ("Then\t" + mission_time_text.text);
		minutes = missionHandler.missionMinutes;
		seconds = missionHandler.missionSeconds;
		minutes_prefix = "";
		seconds_prefix = "";
		pick_current_time = true;
		Debug.Log ("Now\t" + mission_time_text.text);
		levelNumberText.text = "Level - " + (GameManager.Instance.mission_no + 1);
		levelNumberAnimator.SetTrigger ("NextLevel");
	}
	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 1f;
		GameObject tempBus = (GameObject)Instantiate (Resources.Load ("Buses/" + GameManager.Instance.bus_name) as GameObject, busStartingPosition [GameManager.Instance.mission_no].position, busStartingPosition [GameManager.Instance.mission_no].rotation);
		bus = tempBus.transform;
		rigid = bus.GetComponent<Rigidbody> ();
		busController = bus.gameObject.GetComponent<BusController> ();
		rccCam.playerCar = bus;
		mapCanvasController.playerTransform = bus;
		trafficSpwaner.position = bus.position;	
		trafficSpwaner.SetParent (bus);
		levels [GameManager.Instance.mission_no].SetActive (true);
		missionHandler = levels [GameManager.Instance.mission_no].GetComponent<MissionHandler> ();
		minutes = missionHandler.missionMinutes;
		seconds = missionHandler.missionSeconds;
		if (Application.platform == RuntimePlatform.Android) {
			RCC_Settings.instance.controllerType = RCC_Settings.ControllerType.Mobile;
		}
		if (GameManager.Instance.mission_no == 0) {
			howToPlay.gameObject.transform.parent.gameObject.SetActive (true);
		} else {
			objectiveDialog.SetActive (true);
		}
		levelNumberText.text = "Level - " + (GameManager.Instance.mission_no + 1);
		if (GameManager.Instance.mission_no > 0)
			levelNumberAnimator.SetTrigger ("NextLevel");
	}


	void Update ()
	{
		if (!isMissionFailed && isMissionComplete) {
			return;
		}
//		Debug.Log ("Calleding\t"+mission_time_text.text);
		Timer ();
		float bus_speed = rigid.velocity.magnitude / 1000;
		if (fuel > 0) {
			if (bus_speed > 0.005) {
				fuel -= (bus_speed * fuel_cusmption_rate) * (Time.deltaTime * 20);
				fuel_bar.fillAmount = fuel / 100;
			}
			if (fuel < 30 && is_allow) {
				is_allow = false;
				PlayFuelAudio ();
				fuel_bar.color = Color.red;
				if (isBlinkFuel = true) {
					isBlinkFuel = false;
					LeanTween.alpha (fuel_bar.gameObject.GetComponent<RectTransform> (), 0, 0.55f).setLoopPingPong ();
				}
			} 
		} else if (isFuelEmpty) {
			GameManager.Instance.fromFuelStation = false;
			isFuelEmpty = false;
			PlayFuelEndAudio ();
		}


		if(PlayerPrefs.GetInt("rewardvdo",0)==1){
			PlayerPrefs.SetInt ("rewardvdo", 0);
			if (PlayerPrefs.GetInt("fillfuel")==1) {
				GameObject.FindObjectOfType<GameController> ().Method (true);
				PlayerPrefs.SetInt("fillfuel",0);
			}

			if (PlayerPrefs.GetInt("repairbus")==1) {
				GameObject.FindObjectOfType<GameController> ().RepairBusSuccess (true);	
				PlayerPrefs.SetInt("repairbus",0);
			}

		}else if(PlayerPrefs.GetInt("rewardvdo",0)==2){
			PlayerPrefs.SetInt ("rewardvdo", 0);
		}

	}

	void PlayFuelAudio ()
	{
		if (!fuelFillingAudioSourc.loop) {
			fuelFillingAudioSourc.loop = true;
		}
		fuelFillingAudioSourc.clip = fuelWarning;
		fuelFillingAudioSourc.Play ();
	}

	void PlayFuelEndAudio ()
	{
		fuelFillingAudioSourc.loop = false;
		fuelFillingAudioSourc.clip = fuelEnd;
		fuelFillingAudioSourc.Play ();
		isMissionFailed = true;
		watchAdToRefillTankDialog.SetActive (true);
		//missionFailedDialog.SetActive (true);
		//fuelDialog.SetActive (true);
	}

	public void SetScreenBlack ()
	{
		blackImage.SetActive (true);
		Invoke ("SetScreenWhite", 2f);
	}

	void SetScreenWhite ()
	{
		blackImage.SetActive (false);
	}
	public void pausebtnAds(){
	//	AdsManager._instance.ShowunityAds();
		print ("pause btn ads");
	}
	public void MissionComplete ()
	{

		isMissionComplete = true;

		PlayerPrefs.SetInt ("UserEarning", (PlayerPrefs.GetInt ("UserEarning") + missionHandler.fair));
		if (GameManager.Instance.isHowToPlay == false) {
			missionCompleteDialog.SetActive (true);
			missionCompleteTimeText.text = "" + mission_time_text.text;
		} else {
			howToPlay.TutorialComplete ();
		}
		minutes_prefix = "";
		seconds_prefix = "";
		mission_time_text.text = "";

	}

	public void HowToPlayFunction ()
	{
		howToPlay.Next ();
	}
//	string referName="";
//	public void HandleRewardBasedVideoRewarded (object sender, Reward args)
//	{
//		if (referName == "UnityRewardedAd") {
//			GameObject.FindObjectOfType<UnityRewardedAd> ().OnAdCompleted ();	
//		} 
//	
//		print ("User rewarded with: " + amount.ToString () + " " + type);
//	}
	public void WatchAdToRefillFuelTank ()
	{
//mm		AdManager.instance.ShowRewardedAd ("WatchAdToRefillFuelTank");
		PlayerPrefs.SetInt("fillfuel",1);
		//AdsManager._instance.ShowUnityRewardedvideo();

//		if (AdsManager.instance.IsReadyRewardedVideo ())
//			AdsManager.instance.ShowRewardedVideo (Method);
	}

	public void RepairBus ()
	{
//mm		AdManager.instance.ShowRewardedAd ("RepairBus");
		PlayerPrefs.SetInt("repairbus",1);
		//AdsManager._instance.ShowUnityRewardedvideo();



//		if (AdsManager.instance.IsReadyRewardedVideo ())
//			AdsManager.instance.ShowRewardedVideo (RepairBusSuccess);
		
	}

	void CalculatePrefix (int temp_seconds)
	{
		if (temp_seconds > 9) {
			seconds_prefix = "";
		} else {
			seconds_prefix = "0";
		}
		if (minutes > 9) {
			minutes_prefix = "";
		} else {
			minutes_prefix = "0";
		}
		//	Debug.Log ("Then\t"+mission_time_text.text);
	}

	void Timer ()
	{
//		Debug.Log ("Then\t"+mission_time_text.text);
		if (pick_current_time) {
			current_time = (int)Time.time;
			pick_current_time = false;
		}
		int temp_seconds = seconds - ((int)Time.time - current_time);
		//seconds = temp_seconds;
		if (minutes > 0 || temp_seconds > 0) {
			if (temp_seconds == 0) {
				current_time = (int)Time.time;
				temp_seconds = 59;
				seconds = 59;
				minutes -= 1;
			}	
			CalculatePrefix (temp_seconds);
			mission_time_text.text = minutes_prefix + minutes + ":" + seconds_prefix + temp_seconds;

			if (temp_seconds == 10 && minutes == 0) {
				LeanTween.alpha (mission_time_text.gameObject.GetComponent<RectTransform> (), 0f, 1f).setLoopPingPong ();
				mission_time_text.color = Color.red;
				//beepAudioSource.Play ();
			}

		} else {
			if (!isMissionFailed) {
				if (Application.internetReachability != NetworkReachability.NotReachable) {
				//	adspanel.SetActive (true);
				//	AdsManager._instance.ShowInterstitial ();
					Invoke ("missionads",0.1f);

				} else {
					print ("level failed");
					isMissionFailed = true;
					missionFailedDialog.SetActive (true);
				}


			}
		}
//		Debug.Log ("Then\t"+mission_time_text.text);
	}
	void missionads(){
		adspanel.SetActive (false);
		print ("level failed");
		isMissionFailed = true;
		missionFailedDialog.SetActive (true);
	}
	public void RepairBusSuccess (bool success)
	{	
		if (success) {
			health = 100;
			healthBar.fillAmount = health / 100;

			LeanTween.cancel (healthBar.gameObject);
			LeanTween.alpha (healthBar.GetComponent<RectTransform> (), 1f, 0f);
			repairBusDialog.SetActive (false);
		} else {	
			repairBusDialog.SetActive (false);
		}	

	}

	public void Method (bool success)
	{	
		if (success) {
			fuel = 100;
			fuel_bar.GetComponent<Image> ().color = Color.white;
			fuel_bar.fillAmount = fuel / 100;
			is_allow = true;
			isFuelEmpty = true;
			LeanTween.cancel (fuel_bar.gameObject);
			watchAdToRefillTankDialog.SetActive (false);
		} else {	
			watchAdToRefillTankDialog.SetActive (false);
		}	

	}
}
