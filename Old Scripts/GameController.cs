using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using AppAdvisory.Ads;
//*********************************************//
//This class controles the overall game//
//Bus Respawning at random position
//Picking the passangers when bus reaches at stops
//Dropping the passangers when bus reaches at destination
//Showing the dialogs when Mission Complete, Mission Failed, Objective Dialog and Pause Dialog
//********************************************//
public class GameController : MonoBehaviour
{
	public GameObject[] Missions = new GameObject[10];
	public Transform[] bus_spwan_points = new Transform[0];
	public Transform itsSpwaner;
	public GameObject[] busses;
	public GameObject black_image;
	GameObject bus;
//	public RCCCarCamera rcc_car_camera;
	public GameObject paused_dialog;
	public GameObject mission_complete_dialog;
	public GameObject mission_failed_dialog;
	public GameObject objective_dialog;
	public MapCanvasController map_canvas_controller;
	public Transform mini_map_cam;
	bool is_mission_complete = false;
	MissionHandler mission_handler;
	GameObject temp_bus_obj;
	GameObject temp_current_station;
	public Text time_text;
	BusController bus_controller;
	public AudioSource cameraAudioSource;
	public AudioClip levelCompleteClip;
	public AudioClip trafficSound;
	public AudioClip missionFailedClip;
	public AudioClip [] maleHitSounds=new AudioClip[0];
	public AudioClip [] femaleHitSounds=new AudioClip[0];
	public AudioSource hitAudioSource;
	public GameObject dialogBackground;
	public GameObject controllerSelectionDialog;
	public GameObject helpDialog;
	public GameObject unLockBusDialog;
	public GameObject fuelDialog;
	public AudioSource fuelFillingAudioSourc;
	public AudioClip fuelEnd;
	public AudioClip fuelWarning;

	void Start ()
	{	
		if (GameManager.Instance.isFirstTimeGamePlay) {
			GameManager.Instance.isFirstTimeGamePlay = false;
			controllerSelectionDialog.SetActive (true);
			helpDialog.SetActive (true);
		}
		mission_handler = Missions [GameManager.Instance.mission_no].GetComponent<MissionHandler> ();
		Missions [GameManager.Instance.mission_no].SetActive (true);

		temp_bus_obj = (GameObject)Instantiate (busses [GameManager.Instance.bus_number], bus_spwan_points [GameManager.Instance.mission_no].position,bus_spwan_points [GameManager.Instance.mission_no].rotation);
		itsSpwaner.SetParent (temp_bus_obj.transform);
		itsSpwaner.position = temp_bus_obj.transform.position;
		itsSpwaner.gameObject.SetActive (true);
		bus_controller = temp_bus_obj.GetComponent<BusController> ();
		map_canvas_controller.playerTransform = temp_bus_obj.transform;
		mini_map_cam.position = new Vector3 (temp_bus_obj.transform.position.x, mini_map_cam.position.y, temp_bus_obj.transform.position.z);
		//rcc_car_camera.playerCar = temp_bus_obj.transform;
		if (GameManager.Instance.bus_number == 3) {	
//			rcc_car_camera.distance = 12f;
//			rcc_car_camera.height = 5f;
		}
		else if (GameManager.Instance.bus_number == 4||GameManager.Instance.bus_number == 2||GameManager.Instance.bus_number == 0) {
//			rcc_car_camera.distance = 12f;
//			rcc_car_camera.height = 4f;
		}
		Invoke ("ShowObjectiveDialog", 1.5f);
		//Advertisement.Initialize ("1098773");
//		Adds.instance.HideAdmobBannerAd ();
	//	Adds.instance.LogGoogleAnalytics ();

	}
	public void PlayFuelAudio(){
		if (!fuelFillingAudioSourc.loop) {
			fuelFillingAudioSourc.loop = true;
		}
			fuelFillingAudioSourc.clip = fuelWarning;
		fuelFillingAudioSourc.Play ();
	}
	public void PlayFuelEndAudio(){
		fuelFillingAudioSourc.loop = false;
		fuelFillingAudioSourc.clip = fuelEnd;
		fuelFillingAudioSourc.Play ();
		fuelDialog.SetActive (true);
	}
	Transform bus_set_postion;

	void SetBusPosition ()
	{
		
		bus.transform.position = new Vector3 (bus_set_postion.position.x, temp_bus_obj.transform.position.y, bus_set_postion.position.z);
		bus.transform.rotation = bus_set_postion.rotation;
		StartCoroutine (DropPassanger (temp_current_station, bus_controller.passanger_holder.gameObject));

	}
	public void PlayMalePassangerHitByBusSound(){
		int randSound = UnityEngine.Random.Range (0,maleHitSounds.Length);
		hitAudioSource.clip = maleHitSounds [randSound];
		hitAudioSource.Play ();
	}
	public void PlayFeMalePassangerHitByBusSound(){
		int randSound = UnityEngine.Random.Range (0,femaleHitSounds.Length);
		hitAudioSource.clip = femaleHitSounds [randSound];
		hitAudioSource.Play ();
	}
	public IEnumerator PickUpPassanger (GameObject bus_stop, GameObject temp_bus)
	{
		temp_current_station = bus_stop;
		bus = temp_bus;
		bus.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		//bus_controller.is_stay = true;
		//bus.GetComponent<RCCCarControllerV2> ().enabled = false;
		black_image.SetActive (true);
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 1f, 1f);
		bus_set_postion = bus_stop.transform;
		Invoke ("SetBusPosition", 1.1f);
		yield return new WaitForSeconds (2);
		//Camera.main.GetComponent<RCCCarCamera> ().playerCar = null;
		Transform cam_pos = bus_stop.transform.FindChild ("CameraPosition").transform;
		Camera.main.transform.position = cam_pos.position;
		Camera.main.transform.rotation = cam_pos.rotation;
		LeanTween.alpha (black_image.GetComponent<RectTransform> (), 0f, 1f).setDelay (1f);
		temp_current_station = bus_stop;
		Invoke ("MovePassangers",1.8f);

	}
	void MovePassangers(){
		int mission_number = GameManager.Instance.mission_no;	
		for (int i = 0; i < mission_handler.passanger.Length; i++) {
			Passanger psng = mission_handler.passanger [i].GetComponent<Passanger> ();
			if (psng.source_stop_tag + "(Clone)" == temp_current_station.name) {
				psng.is_destination = false;

				psng.RunAnimaiton (temp_bus_obj.transform.FindChild ("BusDoor").gameObject.transform, true);
			}
		}

	}

	public IEnumerator DropPassanger (GameObject bus_stop, GameObject temp_holder)
	{
		yield return new WaitForSeconds (1);
//		temp_holder.transform.parent.GetComponent<RCCCarControllerV2> ().enabled = false;
		int count = temp_holder.transform.childCount;
		string stop_name = bus_stop.name.Substring (0, bus_stop.name.Length - 7) + "Pos";
		GameObject t_target = Missions [GameManager.Instance.mission_no].transform.FindChild (stop_name).gameObject;
		for (int i = count - 1; i >= 0; i--) {
			GameObject obj = temp_holder.transform.GetChild (i).gameObject;
			Passanger psng = mission_handler.passanger [i].GetComponent<Passanger> ();
			if (psng.dest_stop_tag + "(Clone)" == bus_stop.name) {
				obj.transform.SetParent (null);
				obj.SetActive (true);
				int target_number = UnityEngine.Random.Range (0, t_target.transform.childCount);
				Passanger passanger = obj.GetComponent<Passanger> ();
				passanger.target = t_target.transform.GetChild (target_number);
				passanger.is_destination = true;
				passanger.RunAnimaiton (t_target.transform.GetChild (target_number).gameObject.transform, true);

			}
		}

		if (bus_stop.name == mission_handler.last_stop + "(Clone)") {
			is_mission_complete = true;
			if (GameManager.Instance.mission_no != 19) {
				bus_controller.is_time_start = false;
				if (PlayerPrefs.GetInt ("LevelLocked") <= GameManager.Instance.mission_no) {
					PlayerPrefs.SetInt ("LevelLocked", (GameManager.Instance.mission_no + 1));
				}
			}
			PlayerPrefs.SetInt ("UserEarning", (PlayerPrefs.GetInt ("UserEarning") + mission_handler.fair));
			Invoke ("ShowCompleteDialog", 5);
			temp_current_station = bus_stop;
			Invoke ("CloseBusDoor",2f);
		}

	}
	void CloseBusDoor(){
		//if (GameManager.Instance.bus_number != 4) {
			bus_controller.audio_source.clip = bus_controller.door_close_clip;
			bus_controller.audio_source.Play ();
		//}
		temp_bus_obj.GetComponent<Animator> ().SetBool ("DoorOpen",false);
	}
	void ShowCompleteDialog ()
	{
		if (PlayerPrefs.GetInt ("UserEarning") >= 50000 && PlayerPrefs.GetInt ("BusNumber5") != 0) {
			GameManager.Instance.isToBusUnLock = true;
			GameManager.Instance.unlockBusIndex = 4;
			unLockBusDialog.SetActive (true);
		}
		else if (PlayerPrefs.GetInt ("UserEarning") >= 40000 && PlayerPrefs.GetInt ("BusNumber4") != 0) {
			GameManager.Instance.isToBusUnLock = true;
			GameManager.Instance.unlockBusIndex = 3;
			unLockBusDialog.SetActive (true);
		}
		else if (PlayerPrefs.GetInt ("UserEarning") >= 30000 && PlayerPrefs.GetInt ("BusNumber3") != 0) {
			GameManager.Instance.isToBusUnLock = true;
			GameManager.Instance.unlockBusIndex = 2;
			unLockBusDialog.SetActive (true);
		}
		else if (PlayerPrefs.GetInt ("UserEarning") >= 20000 && PlayerPrefs.GetInt ("BusNumber2") != 0) {
			GameManager.Instance.isToBusUnLock = true;
			GameManager.Instance.unlockBusIndex = 1;
			unLockBusDialog.SetActive (true);
		}
		dialogBackground.SetActive (true);
		cameraAudioSource.loop = false;
		cameraAudioSource.clip = levelCompleteClip;
		cameraAudioSource.Play ();
		mission_handler.fair = mission_handler.fair - (int)bus_controller.damage_deduction;
		mission_complete_dialog.transform.FindChild ("EarningText").GetComponent<Text> ().text = mission_handler.fair + "$";
		mission_complete_dialog.transform.FindChild ("TimeText").GetComponent<Text> ().text = bus_controller.mission_time_text.text;
		mission_complete_dialog.SetActive (true);
		LeanTween.scale (mission_complete_dialog, new Vector3 (1, 1, 1), 0.25f);
		Invoke ("SetGamePlayAudio",6f);

	}
	void SetGamePlayAudio(){
		cameraAudioSource.loop = true;
		cameraAudioSource.clip = trafficSound;
		cameraAudioSource.Play ();
	}
	public void ShowMissionFailedDialog ()
	{
		dialogBackground.SetActive (true);
		cameraAudioSource.loop = false;
		cameraAudioSource.clip = missionFailedClip;
		cameraAudioSource.Play ();
		if(AdsManager.instance.IsReadyInterstitial())
		AdsManager.instance.ShowInterstitial();
		mission_failed_dialog.transform.FindChild ("EarningText").GetComponent<Text> ().text = "0$";
		mission_failed_dialog.transform.FindChild ("TimeText").GetComponent<Text> ().text = bus_controller.mission_time_text.text;	//.text;
		mission_failed_dialog.SetActive (true);
		LeanTween.scale (mission_failed_dialog, new Vector3 (1, 1, 1), 0.25f);
		Invoke ("PauseGame",0.5f);
		Invoke ("SetGamePlayAudio",6f);
	}

	void ShowObjectiveDialog ()
	{
		dialogBackground.SetActive (true);
		objective_dialog.SetActive (true);
		LeanTween.alpha (objective_dialog.GetComponent<RectTransform> (), 1f, 0.5f);
	}

	public void ObjectiveDialogOkButton ()
	{
		LeanTween.alpha (objective_dialog.GetComponent<RectTransform> (), 0f, 0.5f);
		Invoke ("SetObjectiveDialogFalse", 0.6f);
	}

	void SetObjectiveDialogFalse ()
	{
		dialogBackground.SetActive (false);
		objective_dialog.SetActive (false);
	}

	public void NextMission ()
	{
		if(AdsManager.instance.IsReadyInterstitial())
			AdsManager.instance.ShowInterstitial ();	
		if(GameManager.Instance.bus_number!=4||GameManager.Instance.bus_number!=2||GameManager.Instance.bus_number!=0)
		temp_bus_obj.GetComponent<Animator> ().SetBool ("DoorOpen",false);
		Destroy (temp_current_station);
		GameManager.Instance.bus_stops.Clear ();
		if (GameManager.Instance.mission_no != 19 && is_mission_complete) {
			bus_controller.is_time_start = false;
			bus_controller.pick_current_time = true;
			bus_controller.picked_passanger = 0;
			is_mission_complete = false;
			LeanTween.scale (mission_complete_dialog, new Vector3 (0.3f, 0.3f, 0.3f), 0.25f);
			Invoke ("ExitCompletDialog", 0.25f);
			black_image.SetActive (true);
			LeanTween.alpha (black_image.GetComponent<RectTransform> (), 1f, 1f);
			LeanTween.alpha (black_image.GetComponent<RectTransform> (), 0f, 1f).setDelay (1.5f);
			Invoke ("SetNextMission", 1.5f);
		}
	}

	void ExitCompletDialog ()
	{
		dialogBackground.SetActive (false);
		mission_complete_dialog.SetActive (false);
	}

	void SetNextMission ()
	{
	//	temp_bus_obj.GetComponent<RCCCarControllerV2> ().enabled = true;
		bus.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		//bus_controller.is_stay = false;
	//	rcc_car_camera.playerCar = temp_bus_obj.transform;
		black_image.SetActive (false);
		Missions [GameManager.Instance.mission_no].SetActive (false);
		GameManager.Instance.mission_no += 1;
		int level_number = GameManager.Instance.mission_no;
		if (level_number == 1) {
			GameManager.Instance.mission_minutes = 3;
			GameManager.Instance.mission_seconds = 45;
		} else if (level_number == 2) {
			GameManager.Instance.mission_minutes = 4;
			GameManager.Instance.mission_seconds = 15;
		} else if (level_number == 3) {
			GameManager.Instance.mission_minutes = 4;
			GameManager.Instance.mission_seconds = 45;
		} else if (level_number == 4) {
			GameManager.Instance.mission_minutes = 5;
			GameManager.Instance.mission_seconds = 20;
		} else if (level_number == 5) {
			GameManager.Instance.mission_minutes = 7;
			GameManager.Instance.mission_seconds = 0;
		} else if (level_number == 6) {
			GameManager.Instance.mission_minutes = 8;
			GameManager.Instance.mission_seconds = 45;
		} else if (level_number == 7) {
			GameManager.Instance.mission_minutes = 9;
			GameManager.Instance.mission_seconds = 15;
		} else if (level_number == 8) {
			GameManager.Instance.mission_minutes = 10;
			GameManager.Instance.mission_seconds = 0;
		} else if (level_number == 9) {
			GameManager.Instance.mission_minutes = 11;
			GameManager.Instance.mission_seconds = 0;
		}
		else if (level_number == 10) {
			GameManager.Instance.mission_minutes = 12;
			GameManager.Instance.mission_seconds = 15;
		}
		else if (level_number == 11) {
			GameManager.Instance.mission_minutes = 12;
			GameManager.Instance.mission_seconds = 30;
		}
		else if (level_number == 12) {
			GameManager.Instance.mission_minutes = 13;
			GameManager.Instance.mission_seconds = 15;
		}
		else if (level_number == 13) {
			GameManager.Instance.mission_minutes = 13;
			GameManager.Instance.mission_seconds = 45;
		}
		else if (level_number == 14) {
			GameManager.Instance.mission_minutes = 14;
			GameManager.Instance.mission_seconds = 0;
		}
		else if (level_number == 15) {
			GameManager.Instance.mission_minutes = 15;
			GameManager.Instance.mission_seconds = 30;
		}
		else if (level_number == 16) {
			GameManager.Instance.mission_minutes = 16;
			GameManager.Instance.mission_seconds = 15;
		}
		else if (level_number == 17) {
			GameManager.Instance.mission_minutes = 17;
			GameManager.Instance.mission_seconds = 0;
		}
		else if (level_number == 18) {
			GameManager.Instance.mission_minutes = 17;
			GameManager.Instance.mission_seconds = 30;
		}
		else if (level_number == 19) {
			GameManager.Instance.mission_minutes = 18;
			GameManager.Instance.mission_seconds = 0;
		}
		mission_handler = Missions [GameManager.Instance.mission_no].GetComponent<MissionHandler> ();
		Missions [GameManager.Instance.mission_no].SetActive (true);
		bus_controller.SetNextMissionTime ();
	}

	public void RestartButton ()
	{
		Time.timeScale = 1f;
		dialogBackground.SetActive (false);
		if(AdsManager.instance.IsReadyInterstitial())
		AdsManager.instance.ShowInterstitial ();
		bus_controller.is_time_start = false;
		GameManager.Instance.bus_stops.Clear ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	public void SettingButton(){
		controllerSelectionDialog.SetActive (true);
	}
	public void FromPauseToMainMenu()
	{
		if(AdsManager.instance.IsReadyInterstitial())
		AdsManager.instance.ShowInterstitial ();
		Time.timeScale = 1f;
		dialogBackground.SetActive (false);
		GameManager.Instance.bus_stops.Clear ();
		SceneManager.LoadScene ("MainMenu");
	
	}

	public void GoToMainMenu ()
	{
		if (AdsManager.instance.IsReadyVideoAds ())
			AdsManager.instance.ShowVideoAds ();
		else {
			if(AdsManager.instance.IsReadyInterstitial())
			AdsManager.instance.ShowInterstitial ();
		}
//		Adds.instance.ShowUnityAd ();
		Time.timeScale = 1f;
		dialogBackground.SetActive (false);
		GameManager.Instance.bus_stops.Clear ();
		SceneManager.LoadScene ("MainMenu");
	}

	public void ResumeButton ()
	{
		Time.timeScale = 1f;
		LeanTween.scale (paused_dialog, new Vector3 (0.3f, 0.3f, 0.3f), 0.25f);
		Invoke ("SetResumeButton", 0.3f);
	}

	void SetResumeButton ()
	{
		dialogBackground.SetActive (false);
		paused_dialog.SetActive (false);
	}

	public void ShowPauseDialog ()
	{
		dialogBackground.SetActive (true);
		paused_dialog.SetActive (true);
		LeanTween.scale (paused_dialog, new Vector3 (1, 1, 1), 0.25f);
		Invoke ("PauseGame", 0.35f);
		if (AdsManager.instance.IsReadyVideoAds ())
			AdsManager.instance.ShowVideoAds ();
		else {
			if(AdsManager.instance.IsReadyInterstitial())
			AdsManager.instance.ShowInterstitial ();
		}
//		Adds.instance.ShowUnityAd ();

	}
	void PauseGame(){
		Time.timeScale = 0;
	}

	void Update ()
	{
		mini_map_cam.position = new Vector3 (temp_bus_obj.transform.position.x, mini_map_cam.position.y, temp_bus_obj.transform.position.z);
	}


}
