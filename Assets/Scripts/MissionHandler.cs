using UnityEngine;
using System.Collections;
//***********************************//
//This class handles the missions of the game//
//Assigning the passanger source stop and destination stop
//Instantiating the bus stops
public class MissionHandler : MonoBehaviour
{
	public GameObject[] passanger = new GameObject[1];
	public GameObject[] busstops = new GameObject[1];
	public GameObject[] passanger_pos = new GameObject[1];
	public GameObject[] busstops_pos = new GameObject[1];
	public float [] stopTimer=new float[0]; 
	public string last_stop;
	public int fair;
	public Transform bus_init_pos;
	public int missionMinutes;
	public int missionSeconds;
	GameObject current_bus;

	void Start ()
	{
		if (GameManager.Instance.is_continuou) {
			current_bus = GameObject.Find (GameManager.Instance.bus_name);
			print (current_bus.name);
		}
        
		for (int i = 0; i < passanger_pos.Length; i++) {
			passanger [i].transform.position = passanger_pos [i].transform.position;
			passanger [i].transform.rotation = passanger_pos [i].transform.rotation;
		}
		GameManager.Instance.bus_stops.Clear ();
		switch (GameManager.Instance.mission_no) {
		case 0:												//Mission Number 1
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				pass.pickStop = "StopA(Clone)";
				pass.dropStop= "StopB(Clone)";
			}
			for (int i = 0; i < busstops_pos.Length; i++) {	
	//			Debug.Log ("Mission 1");
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
//				Debug.Log (temp_bus_stop.name);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
//				if (i == 0) {
//					temp_bus_stop.GetComponent<MapMarker> ().number_of_passangers = 1;
//				}
				if (i > 0) {
					
					temp_bus_stop.SetActive (false);
				}
			} 
			break;
		case 1:												//Mission Number 2
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				pass.pickStop = "StopA(Clone)";
				pass.dropStop= "StopB(Clone)";
			}
			for (int i = 0; i < busstops_pos.Length; i++) {	
//				Debug.Log ("Mission 1");
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
//				Debug.Log (temp_bus_stop.name);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
					if (i > 0) {

					temp_bus_stop.SetActive (false);
				}
			} 

			break;
		case 2:												//Mission Number 3
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				pass.pickStop = "StopA(Clone)";
				pass.dropStop= "StopB(Clone)";
			}
			for (int i = 0; i < busstops_pos.Length; i++) {	
//				Debug.Log ("Mission 1");
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
//				Debug.Log (temp_bus_stop.name);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {

					temp_bus_stop.SetActive (false);
				}
			} 


			break;
		case 3:												//Mission Number 4
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				pass.pickStop = "StopA(Clone)";
				pass.dropStop= "StopB(Clone)";
			}
			for (int i = 0; i < busstops_pos.Length; i++) {	
				Debug.Log ("Mission 1");
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				Debug.Log (temp_bus_stop.name);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {

					temp_bus_stop.SetActive (false);
				}
			} 


			break;
		case 4:												//Mission Number 5
			for (int i = 0; i <passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				pass.pickStop = "StopA(Clone)";
				pass.dropStop= "StopB(Clone)";
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}   

			break;
		case 5:												//Mission Number 6
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 3) {
					pass.pickStop = "StopA(Clone)";
					pass.dropStop = "StopB(Clone)";
				} else {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {
					
					temp_bus_stop.SetActive (false);
				}
			}   
			break;
		case 6:												//Mission Number 7
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);

				if (i  <2) {
					
					pass.pickStop = "StopA(Clone)";
					pass.dropStop = "StopB(Clone)";
				} else {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {
					temp_bus_stop.SetActive (false);
				}
			}
			break;
		case 7:												//Mission Number 8
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 3) {
					
					pass.pickStop = "StopA(Clone)";
					pass.dropStop = "StopB(Clone)";
				} else {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;		
		case 8:												//Mission Number 9
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				Debug.Log (passanger.Length);
				if (i < 3) {
					
					pass.pickStop = "StopA(Clone)";
					pass.dropStop = "StopB(Clone)";
				} else {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;	
		case 9:												//Mission Number 10
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 3) {
					pass.pickStop = "StopA(Clone)";
					pass.dropStop = "StopB(Clone)";
				} else {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;		
		case 10:												//Mission Number 11
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 2) {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
				else if (i < 3) {
					Debug.Log ("Called");
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "NewStopE(Clone)";
				}  
				else if (i < 6){
					pass.pickStop = "NewStopF(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;
		case 11:												//Mission Number 11
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 2) {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
				else if (i < 3) {
					Debug.Log ("Called");
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "NewStopE(Clone)";
				}  
				else if (i < 6){
					pass.pickStop = "NewStopF(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;
		case 12:												//Mission Number 11
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 2) {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
				else if (i < 3) {
					Debug.Log ("Called");
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "NewStopE(Clone)";
				}  
				else if (i < 6){
					pass.pickStop = "NewStopF(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;
		case 13:												//Mission Number 11
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 2) {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
				else if (i < 3) {
					Debug.Log ("Called");
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "NewStopE(Clone)";
				}  
				else if (i < 6){
					pass.pickStop = "NewStopF(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;
		case 14:												//Mission Number 11
			for (int i = 0; i < passanger.Length; i++) {
				passanger [i].SetActive (true);
				BusPassanger pass = passanger [i].GetComponent<BusPassanger> ();
				pass.isToWalk = false;
				pass.GetComponent<Animator> ().SetBool ("Walk", false);
				if (i < 2) {
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
				else if (i < 3) {
					Debug.Log ("Called");
					pass.pickStop = "NewStopD(Clone)";
					pass.dropStop = "NewStopE(Clone)";
				}  
				else if (i < 6){
					pass.pickStop = "NewStopF(Clone)";
					pass.dropStop = "StopB(Clone)";
				}
			}
			for (int i = 0; i < busstops_pos.Length; i++) {
				GameObject temp_bus_stop = (GameObject)Instantiate (busstops [i], busstops_pos [i].transform.position, busstops_pos [i].transform.rotation);
				GameManager.Instance.bus_stops.Add (temp_bus_stop);
				if (i > 0) {					
					temp_bus_stop.SetActive (false);
				}
			}
			break;
		}

	}
}
