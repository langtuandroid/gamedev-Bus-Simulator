using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BusController : MonoBehaviour
{
	public Rigidbody rigid;
	//RCCCarControllerV2 rcc_car_controller_v2;
	GameObject player;
	GameObject current_station;
	Transform dest_target;
	[HideInInspector] public Transform passanger_holder;
	public bool is_stay = false;
	bool is_passanger_picking = false;
	public float fuel = 100;
	int fuel_cusmption_rate = 2;
	public float damage_divider = 5;
	//public float bus_speed = 100;
	//public float bus_steer = 50;
	public float bus_health = 100;
	[HideInInspector] public int picked_passanger = 0;
	int passanger_at_stop = 0;
	public int damageThreshold = 10;

	Image fuel_bar;
	Image damageBar;
	Text damageText;
	Text earning_text;
	bool is_allow = true;
	bool is_allow2 = true;
	[HideInInspector] public float damage_deduction = 0f;
	//int earnings = 100;
	GameObject[] passangers;
	bool is_killed = false;
	GameController game_controller;
	[HideInInspector] public Text mission_time_text;
	int minutes;
	int seconds;
	string minutes_prefix = "";
	string seconds_prefix = "";
	[HideInInspector] public bool pick_current_time = true;
	[HideInInspector] public int current_time = 0;
	[HideInInspector] public bool is_time_start = false;
	[HideInInspector] public bool is_mission_faild_shown = true;
	[HideInInspector] public bool isFuelEmpty = true;
	public AudioClip door_open_clip;
	public AudioClip door_close_clip;
	public AudioSource audio_source;
	bool isBlinkFuel=true;
	public AudioSource beepAudioSource;
	void Start ()
	{
		picked_passanger = 0;
		fuel_bar = GameObject.Find ("FuelBar").GetComponent<Image> ();
		damageBar = GameObject.Find ("DamageBar").GetComponent<Image> ();
		damageText = GameObject.Find ("DamageText").GetComponent<Text> ();
		//  earning_text = GameObject.Find("EarningText").GetComponent<Text>();
      	
	//	rcc_car_controller_v2 = GetComponent<RCCCarControllerV2> ();
		passanger_holder = transform.FindChild ("PassangerHolder").transform;
		GameObject mission = game_controller.Missions [GameManager.Instance.mission_no].gameObject;
		MissionHandler mission_handler = mission.GetComponent<MissionHandler> ();
		passangers = new GameObject[mission_handler.passanger.Length];
		for (int i = 0; i < passangers.Length; i++) {
			passangers [i] = mission_handler.passanger [i];

		}
	}

	void Awake ()
	{
		game_controller = GameObject.Find ("GameManager").GetComponent<GameController> ();
		mission_time_text = game_controller.time_text;
		minutes = GameManager.Instance.mission_minutes;
		seconds = GameManager.Instance.mission_seconds;
		if (seconds < 10) {
			seconds_prefix = "0";
		}
		if (minutes < 10) {
			minutes_prefix = "0";
		}
		mission_time_text.text = minutes_prefix + minutes + ":" + seconds_prefix + seconds;
	}

	public void FillTank (float tfuel)
	{
		isBlinkFuel = true;
		fuel = fuel+tfuel;
		fuel_bar.GetComponent<Image> ().color = Color.white;
		fuel_bar.fillAmount = fuel / 100;
		is_allow = true;

		if (fuel <= 0) {
			game_controller.ShowMissionFailedDialog ();
		} else {
			isFuelEmpty = true;
		}
	}

	void Timer ()
	{
		if (is_time_start) {
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
				mission_time_text.text = minutes_prefix + minutes + ":" + seconds_prefix + temp_seconds;
				if (temp_seconds == 10&&minutes==0) {
					LeanTween.alpha (mission_time_text.gameObject.GetComponent<RectTransform> (), 0f, 1f).setLoopPingPong ();
					mission_time_text.color = Color.red;
					beepAudioSource.Play ();
				}

			} else {
				if (is_mission_faild_shown) {
					is_mission_faild_shown = false;
					game_controller.ShowMissionFailedDialog ();
				}
			}
		}
	}

	void Update ()
	{
		Timer ();
		float bus_speed = rigid.velocity.magnitude / 1000;
		if (fuel > 0) {
			if (bus_speed > 0.005) {
				fuel -= (bus_speed * fuel_cusmption_rate);
//ZZ				Debug.Log ("Fuel\t" + fuel);
				fuel_bar.fillAmount = fuel / 100;
			}
//		        if (is_stay)
//		        {
//		            rigid.velocity = Vector3.zero;
//		        }
			if (fuel < 30 && is_allow) {
				is_allow = false;
				game_controller.PlayFuelAudio ();
				fuel_bar.color = Color.red;
				if (isBlinkFuel = true) {
					isBlinkFuel = false;
					LeanTween.alpha (fuel_bar.gameObject.GetComponent<RectTransform> (), 0, 1f).setLoopPingPong ();
				}			}
		} else if (isFuelEmpty) {
			GameManager.Instance.fromFuelStation = false;
			isFuelEmpty = false;
			//game_controller.fuelDialog.SetActive (true);
			game_controller.PlayFuelEndAudio ();
		}
//		if (is_stay) {
//			rigid.velocity = Vector3.zero;
//		}
		

	}

	//    void Update()
	//    {
	//        if ((fuel < 0 || bus_health < 0 || is_killed))
	//        {
	//            if (is_allow2)
	//            {
	//                is_allow2 = false;
	//                rcc_car_controller_v2.enabled = false;
	//            }
	//
	//            rigid.velocity = Vector3.zero;
	//
	//        }
	//        float bus_speed = rigid.velocity.magnitude / 1000;
	//        if (bus_speed > 0.005)
	//        {
	//            fuel -= (bus_speed / fuel_cusmption_rate);
	//            fuel_bar.fillAmount = fuel / 100;
	//        }
	//        if (is_stay)
	//        {
	//            rigid.velocity = Vector3.zero;
	//        }
	//        if (fuel < 30 && is_allow)
	//        {
	//            is_allow = false;
	//            fuel_bar.color = Color.red;
	//        }
	//
	//    }

	public void SetNextMissionTime ()
	{
		minutes = GameManager.Instance.mission_minutes;
		seconds = GameManager.Instance.mission_seconds;
		if (seconds < 10) {
			seconds_prefix = "0";
		}
		if (minutes < 10) {
			minutes_prefix = "0";
		}
		mission_time_text.text = minutes_prefix + minutes + ":" + seconds_prefix + seconds;
	}

	IEnumerator StayTime ()
	{
		yield return new WaitForSeconds (3);
		game_controller.StartCoroutine (game_controller.PickUpPassanger (current_station, gameObject));
		current_station.GetComponent<MapMarker> ().isActive = false;
		MissionHandler m_h = game_controller.Missions [GameManager.Instance.mission_no].GetComponent<MissionHandler> ();
		for (int i = 0; i < m_h.busstops.Length - 1; i++) {
			
			if (m_h.busstops [i].name + "(Clone)" == current_station.name) {
				GameManager.Instance.bus_stops [i].SetActive (true);
				break;
			}
		}
		Invoke ("OpenDoor", 2.95f);
	}

	void OpenDoor ()
	{
		audio_source.clip = door_open_clip;
		audio_source.Play ();
		if (GameManager.Instance.bus_number != 4 || GameManager.Instance.bus_number != 2 || GameManager.Instance.bus_number != 1) {			
			GetComponent<Animator> ().SetBool ("DoorOpen", true);
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Pedestrain") {
			game_controller.PlayMalePassangerHitByBusSound ();
			col.gameObject.transform.position = GameObject.Find ("PedestrainOutPostion").transform.position;
		} else if (col.gameObject.tag == "FemalePedestrain") {
			game_controller.PlayFeMalePassangerHitByBusSound ();
			col.gameObject.transform.position = GameObject.Find ("PedestrainOutPostion").transform.position;
		}

		if (col.gameObject.tag == "Water") {
			Debug.Log (col.gameObject.name);
			game_controller.ShowMissionFailedDialog ();
		} else if (col.gameObject != null && col.gameObject.tag != "Roads" && rigid.velocity.magnitude > damageThreshold) {
			damage_deduction +=	rigid.velocity.magnitude * damage_divider; 
			bus_health -= (rigid.velocity.magnitude * damage_divider);
			if (bus_health <= 30) {
				LeanTween.color (damageBar.GetComponent<RectTransform> (), Color.red, 0.3f).setLoopPingPong ();
			}
			if (bus_health <= 0) {
				damageText.text = "0" + "/100";
				game_controller.ShowMissionFailedDialog ();
			} else {
				damageText.text = (int)bus_health + "/100";
			}
			damageBar.fillAmount = bus_health / 100;
		}


	}
	IEnumerator StayFuelStation(){
		yield return new WaitForSeconds (3);
		GameManager.Instance.fromFuelStation = true;
		game_controller.fuelDialog.SetActive (true);
	}
	void OnTriggerEnter (Collider col)
	{
		
		if (col.gameObject.tag == "BusStation") {
			current_station = col.gameObject;
			string stop_name = current_station.name.Substring (0, current_station.name.Length - 7) + "Pos";
			GameObject temp_stop = game_controller.Missions [GameManager.Instance.mission_no].transform.FindChild (stop_name).gameObject;
			passanger_at_stop = current_station.GetComponent<MapMarker> ().number_of_passangers;
			dest_target = col.gameObject.transform;
			is_passanger_picking = false;
			StartCoroutine ("StayTime");
		} else if (col.gameObject.tag == "FuelStation") {
			StartCoroutine ("StayFuelStation");

		}
		if (col.gameObject.tag == "Passanger" && (int)rigid.velocity.magnitude == 0) {
			picked_passanger += 1;
			player = col.gameObject;
			player.transform.position = passanger_holder.gameObject.transform.position;
			player.transform.SetParent (passanger_holder);
			player.SetActive (false);
			if (picked_passanger == passanger_at_stop) {
				
				audio_source.clip = door_close_clip;
				audio_source.Play ();
				if (GameManager.Instance.bus_number != 4 || GameManager.Instance.bus_number != 2 || GameManager.Instance.bus_number != 1) {
					GetComponent<Animator> ().SetBool ("DoorOpen", false);
				}
				if (is_time_start == false) {
					is_time_start = true;
				}
				picked_passanger = 0;
		//		rcc_car_controller_v2.enabled = true;
				gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
				//is_stay = false;
				Destroy (current_station);
		//		Camera.main.GetComponent<RCCCarCamera> ().playerCar = gameObject.transform;
				GameObject.Find ("BlackImage").SetActive (false);
			}

		} 

//			else if (col.gameObject.tag == "Passanger") {
//			is_killed = true;
//			col.gameObject.SetActive (false);
//		}

	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "BusStation") {
			StopCoroutine ("StayTime");
		}
		else if (col.gameObject.tag == "FuelStation") {
			StopCoroutine ("StayFuelStation");
		}

	}

}
