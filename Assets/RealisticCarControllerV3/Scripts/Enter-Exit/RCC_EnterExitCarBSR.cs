//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class RCC_EnterExitCarBSR : MonoBehaviour {

	private RCC_CarControllerV3 carControllerBSR;
	private GameObject carCameraBSR;
	private GameObject playerBSR;
	private GameObject dashboardBSR;
	[FormerlySerializedAs("getOutPosition")] public Transform getOutPositionBSR;

	[FormerlySerializedAs("isPlayerIn")] public bool isPlayerInBSR = false;
	private bool  openedBSR = false;
	private float waitTimeBSR = 1f;
	private bool  tempBSR = false;
	
	private void Awake (){

		carControllerBSR = GetComponent<RCC_CarControllerV3>();
		carCameraBSR = GameObject.FindObjectOfType<RCC_CameraBSR>().gameObject;
	
		if(GameObject.FindObjectOfType<RCC_DashboardInputsBSR>())
			dashboardBSR = GameObject.FindObjectOfType<RCC_DashboardInputsBSR>().gameObject;

		if(!getOutPositionBSR){
			GameObject getOutPos = new GameObject("Get Out Position");
			getOutPos.transform.SetParent(transform);
			getOutPos.transform.localPosition = new Vector3(-3f, 0f, 0f);
			getOutPos.transform.localRotation = Quaternion.identity;
			getOutPositionBSR = getOutPos.transform;
		}

	}

	private void Start(){

		if(dashboardBSR)
			StartCoroutine("DisableDashboardBSR");

	}

	private IEnumerator DisableDashboardBSR(){

		yield return new WaitForEndOfFrame();
		dashboardBSR.SetActive(false);

	}
	
	private void Update (){

		if((RCC_SettingsBSR.InstanceBSR.controllerTypeBSR == RCC_SettingsBSR.ControllerType.Keyboard && Input.GetKeyDown(RCC_SettingsBSR.InstanceBSR.enterExitVehicleKBBSR)) && openedBSR && !tempBSR){
			GetOutBSR();
			openedBSR = false;
			tempBSR = false;
		}

		if(isPlayerInBSR)
			carControllerBSR.canControl = true;
		else
			carControllerBSR.canControl = false;

	}
	
	private IEnumerator ActBSR (GameObject fpsplayer){
		
		playerBSR = fpsplayer;

		if (!openedBSR && !tempBSR){
			GetInBSR();
			openedBSR = true;
			tempBSR = true;
			yield return new WaitForSeconds(waitTimeBSR);
			tempBSR = false;
		}

	}
	
	private void GetInBSR (){

		isPlayerInBSR = true;

		carCameraBSR.SetActive(true);

		if(carCameraBSR.GetComponent<RCC_CameraBSR>()){
			carCameraBSR.GetComponent<RCC_CameraBSR>().cameraSwitchCountBSR = 10;
			carCameraBSR.GetComponent<RCC_CameraBSR>().ChangeCameraBSR();
		}

		carCameraBSR.transform.GetComponent<RCC_CameraBSR>().SetPlayerCarBSR(gameObject);
		playerBSR.transform.SetParent(transform);
		playerBSR.transform.localPosition = Vector3.zero;
		playerBSR.transform.localRotation = Quaternion.identity;
		playerBSR.SetActive(false);
		GetComponent<RCC_CarControllerV3>().canControl = true;
		if(dashboardBSR){
			dashboardBSR.SetActive(true);
			dashboardBSR.GetComponent<RCC_DashboardInputsBSR>().GetVehicleBSR(GetComponent<RCC_CarControllerV3>());
		}

			if(!GetComponent<RCC_CarControllerV3>().engineRunning)
				SendMessage ("StartEngine");
		
		//Cursor.lockState = CursorLockMode.None;
	}
	
	private void GetOutBSR (){

		isPlayerInBSR = false;

		playerBSR.transform.SetParent(null);
		playerBSR.transform.position = getOutPositionBSR.position;
		playerBSR.transform.rotation = getOutPositionBSR.rotation;
		playerBSR.transform.rotation = Quaternion.Euler (0f, playerBSR.transform.eulerAngles.y, 0f);
		carCameraBSR.SetActive(false);
		playerBSR.SetActive(true);
		GetComponent<RCC_CarControllerV3>().canControl = false;
		if(!RCC_SettingsBSR.InstanceBSR.keepEnginesAliveBSR)
			GetComponent<RCC_CarControllerV3>().engineRunning = false;
		if(dashboardBSR)
			dashboardBSR.SetActive(false);
		//Cursor.lockState = CursorLockMode.Locked;

	}
	
}