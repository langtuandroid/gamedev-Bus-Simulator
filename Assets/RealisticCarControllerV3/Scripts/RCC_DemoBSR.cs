//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Demo Manager")]
public class RCC_DemoBSR : MonoBehaviour {

	[FormerlySerializedAs("selectableVehicles")] public RCC_CarControllerV3[] selectableVehiclesBSR;
	[FormerlySerializedAs("selectedCarIndex")] public int selectedCarIndexBSR = 0;
	[FormerlySerializedAs("selectedBehaviorIndex")] public int selectedBehaviorIndexBSR = 0;

	public void SelectVehicle (int index) {

		selectedCarIndexBSR = index;
	
	}

	public void Spawn () {

		RCC_CarControllerV3[] activeVehicles = GameObject.FindObjectsOfType<RCC_CarControllerV3>();
		Vector3 lastKnownPos = new Vector3();
		Quaternion lastKnownRot = new Quaternion();
		GameObject newVehicle;

		if(activeVehicles != null && activeVehicles.Length > 0){
			foreach(RCC_CarControllerV3 rcc in activeVehicles){
				if(!rcc.AIController && rcc.canControl){
					lastKnownPos = rcc.transform.position;
					lastKnownRot = rcc.transform.rotation;
					break;
				}
			}
		}

		if(lastKnownPos == Vector3.zero){
			if(	GameObject.FindObjectOfType<RCC_CameraBSR>()){
				lastKnownPos = GameObject.FindObjectOfType<RCC_CameraBSR>().transform.position;
				lastKnownRot = GameObject.FindObjectOfType<RCC_CameraBSR>().transform.rotation;
			}
		}

		lastKnownRot.x = 0f;
		lastKnownRot.z = 0f;

		for (int i = 0; i < activeVehicles.Length; i++) {

			if(activeVehicles[i].canControl && !activeVehicles[i].AIController){
				Destroy(activeVehicles[i].gameObject);
			}
			 
		}

		newVehicle = (GameObject)GameObject.Instantiate(selectableVehiclesBSR[selectedCarIndexBSR].gameObject, lastKnownPos + (Vector3.up), lastKnownRot);
		 
		newVehicle.GetComponent<RCC_CarControllerV3>().canControl = true;

		if(	GameObject.FindObjectOfType<RCC_CameraBSR>()){
			GameObject.FindObjectOfType<RCC_CameraBSR>().SetPlayerCarBSR(newVehicle);
		}

	}

	public void SelectBehavior(int index){

		selectedBehaviorIndexBSR = index;

	}

	public void InitBehavior(){

		switch(selectedBehaviorIndexBSR){
		case 0:
			RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.Simulator;
			RestartScene();
			break;
		case 1:
			RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.Racing;
			RestartScene();
			break;
		case 2:
			RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.SemiArcade;
			RestartScene();
			break;
		case 3:
			RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.Drift;
			RestartScene();
			break;
		case 4:
			RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.Fun;
			RestartScene();
			break;
		case 5:
			RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.Custom;
			RestartScene();
			break;
		}

	}

	public void RestartScene(){

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

	public void Quit(){

		Application.Quit();

	}

}
