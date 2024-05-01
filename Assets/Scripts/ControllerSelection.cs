using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ControllerSelection : MonoBehaviour {
	public Image tilt;
	public Image steer;
	public Image buttons;
	public Sprite tiltAlter;
	public Sprite steerAlter;
	public Sprite buttonsAlter;
	public Sprite tiltNoSelected;
	public Sprite steerNoSelected;
	public Sprite buttonsNoSelected;
//	RCCCarControllerV2 rccV2;	
	void Start(){
//		rccV2 = GameObject.FindGameObjectWithTag ("Bus").transform.root.gameObject.GetComponent<RCCCarControllerV2> ();
		Debug.Log (GameObject.FindGameObjectWithTag ("Bus"));
		GameManager.Instance.controllerType = PlayerPrefs.GetInt ("ControllerType");
		SetController (GameManager.Instance.controllerType);

	}
	void SetController(int n){
//		Debug.Log ("N\t"+n);
		if (n == 2) {
//			rccV2.useAccelerometerForSteer = true;
//			rccV2.steeringWheelControl= false;
			tilt.sprite = tiltAlter;
			buttons.sprite = buttonsNoSelected;
			steer.sprite = steerNoSelected;

		}
		else if (n == 1) {
//			rccV2.useAccelerometerForSteer = false;
//			rccV2.steeringWheelControl= true;
			steer.sprite = steerAlter;
			buttons.sprite = buttonsNoSelected;
			tilt.sprite = tiltNoSelected;

		} else {
		//	Debug.Log ("Here");

//			rccV2.useAccelerometerForSteer = false;
//			rccV2.steeringWheelControl= false;
			buttons.sprite = buttonsAlter;
			tilt.sprite = tiltNoSelected;
			steer.sprite = steerNoSelected;

		}

	}
	public void ControllerButtons(string name){
		switch(name){
		case "Tilt":
			GameManager.Instance.controllerType = 2;
			SetController (2);
			break;
		case "Steer":
			GameManager.Instance.controllerType = 1;
			//rccV2.MobileGUI ();
			SetController (1);
			break;
		case "Buttons":
			GameManager.Instance.controllerType = 0;
			SetController (0);
			break;
		case "OK":
			gameObject.SetActive (false);
			PlayerPrefs.SetInt ("ControllerType",GameManager.Instance.controllerType);
			break;
		}
	}
}
