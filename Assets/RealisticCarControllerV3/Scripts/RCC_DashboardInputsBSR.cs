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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Inputs")]
public class RCC_DashboardInputsBSR : MonoBehaviour {

	[FormerlySerializedAs("currentCarController")] public RCC_CarControllerV3 currentCarControllerBSR;

	[FormerlySerializedAs("RPMNeedle")] public GameObject RPMNeedleBSR;
	[FormerlySerializedAs("KMHNeedle")] public GameObject KMHNeedleBSR;
	[FormerlySerializedAs("turboGauge")] public GameObject turboGaugeBSR;
	[FormerlySerializedAs("NOSGauge")] public GameObject NOSGaugeBSR;
	[FormerlySerializedAs("BoostNeedle")] public GameObject BoostNeedleBSR;
	[FormerlySerializedAs("NoSNeedle")] public GameObject NoSNeedleBSR;

	private float RPMNeedleRotationBSR = 0f;
	private float KMHNeedleRotationBSR = 0f;
	private float BoostNeedleRotationBSR = 0f;
	private float NoSNeedleRotationBSR = 0f;

	[FormerlySerializedAs("RPM")] public float RPMBSR;
	[FormerlySerializedAs("KMH")] public float KMHBSR;
	internal int directionBSR = 1;
	internal float GearBSR;
	internal bool NGearBSR = false;

	internal bool ABSBSR = false;
	internal bool ESPBSR = false;
	internal bool ParkBSR = false;
	internal bool HeadlightsBSR = false;
	internal RCC_CarControllerV3.IndicatorsOn indicatorsBSR;

	private void Update(){

		if(RCC_SettingsBSR.InstanceBSR.uiTypeBSR == RCC_SettingsBSR.UIType.None){
			gameObject.SetActive(false);
			enabled = false;
			return;
		}

		GetValuesBSR();

	}
	
	public void GetVehicleBSR(RCC_CarControllerV3 rcc){

		currentCarControllerBSR = rcc;
		RCC_UIDashboardButtonBSR[] buttons = GameObject.FindObjectsOfType<RCC_UIDashboardButtonBSR>();

		foreach(RCC_UIDashboardButtonBSR button in buttons)
			button.CheckBSR();

	}

	private void GetValuesBSR(){

		if(!currentCarControllerBSR)
			return;

		if(!currentCarControllerBSR.canControl || currentCarControllerBSR.AIController){
			return;
		}

		if(NOSGaugeBSR){
			if(currentCarControllerBSR.useNOS){
				if(!NOSGaugeBSR.activeSelf)
					NOSGaugeBSR.SetActive(true);
			}else{
				if(NOSGaugeBSR.activeSelf)
					NOSGaugeBSR.SetActive(false);
			}
		}

		if(turboGaugeBSR){
			if(currentCarControllerBSR.useTurbo){
				if(!turboGaugeBSR.activeSelf)
					turboGaugeBSR.SetActive(true);
			}else{
				if(turboGaugeBSR.activeSelf)
					turboGaugeBSR.SetActive(false);
			}
		}
		
		RPMBSR = currentCarControllerBSR.engineRPM;
		KMHBSR = currentCarControllerBSR.speed;
		directionBSR = currentCarControllerBSR.direction;
		GearBSR = currentCarControllerBSR.currentGear;

		NGearBSR = currentCarControllerBSR.changingGear;
		
		ABSBSR = currentCarControllerBSR.ABSAct;
		ESPBSR = currentCarControllerBSR.ESPAct;
		ParkBSR = currentCarControllerBSR.handbrakeInput > .1f ? true : false;
		HeadlightsBSR = currentCarControllerBSR.lowBeamHeadLightsOn || currentCarControllerBSR.highBeamHeadLightsOn;
		indicatorsBSR = currentCarControllerBSR.indicatorsOn;

		if(RPMNeedleBSR){
			RPMNeedleRotationBSR = (currentCarControllerBSR.engineRPM / 50f);
			RPMNeedleBSR.transform.eulerAngles = new Vector3(RPMNeedleBSR.transform.eulerAngles.x ,RPMNeedleBSR.transform.eulerAngles.y, -RPMNeedleRotationBSR);
		}
		if(KMHNeedleBSR){
			if(RCC_SettingsBSR.InstanceBSR.unitsBSR == RCC_SettingsBSR.Units.KMH)
				KMHNeedleRotationBSR = (currentCarControllerBSR.speed);
			else
				KMHNeedleRotationBSR = (currentCarControllerBSR.speed * 0.62f);
			KMHNeedleBSR.transform.eulerAngles = new Vector3(KMHNeedleBSR.transform.eulerAngles.x ,KMHNeedleBSR.transform.eulerAngles.y, -KMHNeedleRotationBSR);
		}
		if(BoostNeedleBSR){
			BoostNeedleRotationBSR = (currentCarControllerBSR.turboBoost / 30f) * 270f;
			BoostNeedleBSR.transform.eulerAngles = new Vector3(BoostNeedleBSR.transform.eulerAngles.x ,BoostNeedleBSR.transform.eulerAngles.y, -BoostNeedleRotationBSR);
		}
		if(NoSNeedleBSR){
			NoSNeedleRotationBSR = (currentCarControllerBSR.NoS / 100f) * 270f;
			NoSNeedleBSR.transform.eulerAngles = new Vector3(NoSNeedleBSR.transform.eulerAngles.x ,NoSNeedleBSR.transform.eulerAngles.y, -NoSNeedleRotationBSR);
		}
			
	}

}



