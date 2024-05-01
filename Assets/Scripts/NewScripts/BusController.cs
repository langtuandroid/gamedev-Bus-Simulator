using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BusController : MonoBehaviour {

	public Transform [] seats=new Transform[0];
	public Animator anim;
	GameController gameController;
	MissionHandler missionHandler;
	Transform parkingPos;
	List <Transform> tempSeats=new List<Transform>();
	int countBusStop=0;
	Rigidbody rigid;
	public GameObject headsLight;
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
		for (int i = 0; i < seats.Length; i++) {
			tempSeats.Add (seats [i]);
		}
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		missionHandler = gameController.levels [GameManager.Instance.mission_no].GetComponent<MissionHandler> ();
        	
 	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "BusStation") {
			parkingPos = col.gameObject.transform;
			StartCoroutine ("ParkBus");
		} else if (col.gameObject.tag == "Water") {
			gameController.missionFailedDialog.SetActive (true);
		} else if (col.gameObject.tag == "FuelStation") {
			StartCoroutine ("EnableFuelStationDialog");
		}

	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "BusStation") {
			StopCoroutine ("ParkBus");
		}
		else if (col.gameObject.tag == "FuelStation") {
			StopCoroutine ("EnableFuelStationDialog");
		}
	}
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Pedestrain") {
			gameController.PlayPedestrainVoices ("Male");
			if(rigid.velocity.magnitude>10)
				gameController.ReduceBusHealth (10);
			col.gameObject.transform.position = new Vector3 (50000, 50000, 50000);//GameObject.Find ("PedestrainOutPostion").transform.position;
		} else if (col.gameObject.tag == "FemalePedestrain") {
			gameController.PlayPedestrainVoices ("Female");
			gameController.ReduceBusHealth (10);
			col.gameObject.transform.position = new Vector3 (50000, 50000, 50000);
		}  else if (col.gameObject.tag == "TrafficCar") {	
//			Debug.Log (rigid.velocity.magnitude);
			if(rigid.velocity.magnitude>10)
			gameController.ReduceBusHealth (rigid.velocity.magnitude);
		}
	}
	IEnumerator ParkBus(){	
		yield return new WaitForSeconds (3);
		parkingPos.gameObject.GetComponent<MapMarker> ().isActive = false;
		gameController.SetScreenBlack ();
		Invoke ("ParkNow",1);
	}
	IEnumerator EnableFuelStationDialog(){	
		yield return new WaitForSeconds (3);
		gameController.fuelDialog.SetActive (true);
	}
	void ParkNow(){
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		GetComponent<RCC_CarControllerV3> ().canControl = false;		
		Transform tempParkingPos=parkingPos.Find ("BusPos"+GameManager.Instance.bus_name.Substring(GameManager.Instance.bus_name.Length-1)).transform;
		transform.position = new Vector3(tempParkingPos.position.x,transform.position.y,tempParkingPos.position.z);
		transform.rotation = tempParkingPos.rotation;
		Transform tempCam= parkingPos.Find ("CameraPosition").transform;
		gameController.carCamera.gameObject.GetComponent<RCC_Camera> ().enabled = false;
		gameController.carCamera.position = tempCam.position;
		gameController.carCamera.rotation = tempCam.rotation;
		parkingPos.gameObject.SetActive (false);
		gameController.openDoorButton.SetActive (true);
		if (GameManager.Instance.isHowToPlay) {
			Debug.Log ("Door Open Enable");
			gameController.HowToPlayFunction ();
		}
	}
	public void OpenDoor(){
		anim.SetBool ("DoorOpen",true);
		for (int i = 0; i < missionHandler.passanger.Length; i++) {
			BusPassanger busPassanger = missionHandler.passanger [i].GetComponent<BusPassanger> ();
			int rand = Random.Range (0, tempSeats.Count);
			busPassanger.MovePassanger (tempSeats [rand], gameObject.transform, parkingPos.name, missionHandler.busstops_pos [countBusStop].transform.GetChild (Random.Range (0, 5)).transform);
			if (busPassanger.pickStop == parkingPos.name) {				
				tempSeats.RemoveAt (rand);
			}
		}
		Invoke ("ActivateOpenDoorButton", missionHandler.stopTimer[countBusStop]);
	}
	 void ActivateOpenDoorButton(){
		gameController.openDoorButton.SetActive (true);
		if (GameManager.Instance.isHowToPlay) {
			Debug.Log ("Door Open Enable");
			gameController.HowToPlayFunction ();
		}
	}
	public void HeadLights(bool isOn){
		headsLight.SetActive (isOn);
	}
	public void CloseDoor(){
		anim.SetBool ("DoorOpen",false);		
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		GetComponent<RCC_CarControllerV3> ().canControl = true;		
		gameController.carCamera.gameObject.GetComponent<RCC_Camera> ().enabled = true;
		countBusStop += 1;
		if (missionHandler.last_stop != parkingPos.gameObject.name) {
			GameManager.Instance.bus_stops [countBusStop].SetActive (true);
		} else {
			GameManager.Instance.mission_reward = missionHandler.fair;
			gameController.MissionComplete ();
			tempSeats.Clear ();
			for (int i = 0; i < seats.Length; i++) {
				tempSeats.Add (seats [i]);
			}
			missionHandler= gameController.levels [GameManager.Instance.mission_no+1].GetComponent<MissionHandler> ();
			countBusStop = 0;
		}
	}
}
