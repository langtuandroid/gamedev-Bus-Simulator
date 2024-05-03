//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Mobile Buttons")]
public class RCC_MobileButtonsBSR : MonoBehaviour {

	[FormerlySerializedAs("carControllers")] public RCC_CarControllerV3[] carControllersBSR;

	[FormerlySerializedAs("gasButton")] public RCC_UIControllerBSR gasButtonBSR;
	[FormerlySerializedAs("brakeButton")] public RCC_UIControllerBSR brakeButtonBSR;
	[FormerlySerializedAs("leftButton")] public RCC_UIControllerBSR leftButtonBSR;
	[FormerlySerializedAs("rightButton")] public RCC_UIControllerBSR rightButtonBSR;
	[FormerlySerializedAs("steeringWheel")] public RCC_UISteeringWheelControllerBSR steeringWheelBSR;
	[FormerlySerializedAs("handbrakeButton")] public RCC_UIControllerBSR handbrakeButtonBSR;
	[FormerlySerializedAs("NOSButton")] public RCC_UIControllerBSR NOSButtonBSR;
	[FormerlySerializedAs("gearButton")] public GameObject gearButtonBSR;

	private float gasInputBSR = 0f;
	private float brakeInputBSR = 0f;
	private float leftInputBSR = 0f;
	private float rightInputBSR = 0f;
	private float steeringWheelInputBSR = 0f;
	private float handbrakeInputBSR = 0f;
	private float NOSInputBSR = 1f;
	private float gyroInputBSR = 0f;

	private Vector3 orgBrakeButtonPosBSR;

	private void Start(){

		if(RCC_SettingsBSR.InstanceBSR.controllerTypeBSR != RCC_SettingsBSR.ControllerType.Mobile){
			
			if(gasButtonBSR)
				gasButtonBSR.gameObject.SetActive(false);
			if(leftButtonBSR)
				leftButtonBSR.gameObject.SetActive(false);
			if(rightButtonBSR)
				rightButtonBSR.gameObject.SetActive(false);
			if(brakeButtonBSR)
				brakeButtonBSR.gameObject.SetActive(false);
			if(steeringWheelBSR)
				steeringWheelBSR.gameObject.SetActive(false);
			if(handbrakeButtonBSR)
				handbrakeButtonBSR.gameObject.SetActive(false);
			if(NOSButtonBSR)
				NOSButtonBSR.gameObject.SetActive(false);
			if(gearButtonBSR)
				gearButtonBSR.gameObject.SetActive(false);
			
			enabled = false;
			return;

		}

		orgBrakeButtonPosBSR = brakeButtonBSR.transform.position;
		GetVehiclesBSR();

	}

	public void GetVehiclesBSR(){

		carControllersBSR = GameObject.FindObjectsOfType<RCC_CarControllerV3>();

	}

	private void Update(){

		if(RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR){

			if(RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR)
				RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR = false;
			
			if(!steeringWheelBSR.gameObject.activeInHierarchy){
				steeringWheelBSR.gameObject.SetActive(true);
				brakeButtonBSR.transform.position = orgBrakeButtonPosBSR;
			}

			if(leftButtonBSR.gameObject.activeInHierarchy)
				leftButtonBSR.gameObject.SetActive(false);
			if(rightButtonBSR.gameObject.activeInHierarchy)
				rightButtonBSR.gameObject.SetActive(false);
			
		}

		if(RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR){

			if(RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR)
				RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR = false;

			brakeButtonBSR.transform.position = leftButtonBSR.transform.position;
			if(steeringWheelBSR.gameObject.activeInHierarchy)
				steeringWheelBSR.gameObject.SetActive(false);
			if(leftButtonBSR.gameObject.activeInHierarchy)
				leftButtonBSR.gameObject.SetActive(false);
			if(rightButtonBSR.gameObject.activeInHierarchy)
				rightButtonBSR.gameObject.SetActive(false);
			
		}

		if(!RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR && !RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR){
			
			if(steeringWheelBSR && steeringWheelBSR.gameObject.activeInHierarchy)
				steeringWheelBSR.gameObject.SetActive(false);
			if(!leftButtonBSR.gameObject.activeInHierarchy){
				brakeButtonBSR.transform.position = orgBrakeButtonPosBSR;
				leftButtonBSR.gameObject.SetActive(true);
			}
			if(!rightButtonBSR.gameObject.activeInHierarchy)
				rightButtonBSR.gameObject.SetActive(true);
			
		}

		gasInputBSR = GetInputBSR(gasButtonBSR);
		brakeInputBSR = GetInputBSR(brakeButtonBSR);
		leftInputBSR = GetInputBSR(leftButtonBSR);
		rightInputBSR = GetInputBSR(rightButtonBSR);

		if(steeringWheelBSR)
			steeringWheelInputBSR = steeringWheelBSR.inputBSR;

		if(RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR)
			gyroInputBSR = Input.acceleration.x * RCC_SettingsBSR.InstanceBSR.gyroSensitivityBSR;
		else
			gyroInputBSR = 0f;
		
		handbrakeInputBSR = GetInputBSR(handbrakeButtonBSR);
		NOSInputBSR = Mathf.Clamp(GetInputBSR(NOSButtonBSR) * 2.5f, 1f, 2.5f);

		for (int i = 0; i < carControllersBSR.Length; i++) {

			if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController){

				carControllersBSR[i].gasInput = gasInputBSR;
				carControllersBSR[i].brakeInput = brakeInputBSR;
				carControllersBSR[i].steerInput = -leftInputBSR + rightInputBSR + steeringWheelInputBSR + gyroInputBSR;
				carControllersBSR[i].handbrakeInput = handbrakeInputBSR;
				carControllersBSR[i].boostInput = NOSInputBSR;

			}

		}

	}

	private float GetInputBSR(RCC_UIControllerBSR button){

		if(button == null)
			return 0f;

		return(button.inputBSR);

	}

	public void ChangeCamera () {

		if(GameObject.FindObjectOfType<RCC_CameraBSR>())
			GameObject.FindObjectOfType<RCC_CameraBSR>().ChangeCameraBSR();

	}

	public void ChangeController(int index){

		switch(index){

		case 0:
			RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR = false;
			RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR = false;
			break;
		case 1:
			RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR = true;
			RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR = false;
			break;
		case 2:
			RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR = false;
			RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR = true;
			break;

		}

	}

}
