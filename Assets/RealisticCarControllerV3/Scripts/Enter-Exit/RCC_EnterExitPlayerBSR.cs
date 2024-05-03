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

public class RCC_EnterExitPlayerBSR : MonoBehaviour {

	[FormerlySerializedAs("playerType")] public PlayerType playerTypeBSR;
	public enum PlayerType{FPS, TPS}

	[FormerlySerializedAs("rootOfPlayer")] public GameObject rootOfPlayerBSR;
	[FormerlySerializedAs("maxRayDistance")] public float maxRayDistanceBSR= 2f;
	[FormerlySerializedAs("rayHeight")] public float rayHeightBSR = 1f;
	[FormerlySerializedAs("TPSCamera")] public GameObject TPSCameraBSR;
	private bool showGuiBSR = false;

	private void Start(){

		if (!rootOfPlayerBSR)
			rootOfPlayerBSR = transform.root.gameObject;

		GameObject carCamera = GameObject.FindObjectOfType<RCC_CameraBSR>().gameObject;
		carCamera.SetActive(false);

	}
	
	private void Update (){
		
		Vector3 direction= transform.TransformDirection(Vector3.forward);
		RaycastHit hit;

		if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + (playerTypeBSR == PlayerType.TPS ? rayHeightBSR : 0f), transform.position.z), direction, out hit, maxRayDistanceBSR)){

			if(hit.transform.GetComponentInParent<RCC_CarControllerV3>()){

				showGuiBSR = true;

				if ((RCC_SettingsBSR.InstanceBSR.controllerTypeBSR == RCC_SettingsBSR.ControllerType.Keyboard && Input.GetKeyDown (RCC_SettingsBSR.InstanceBSR.enterExitVehicleKBBSR))) {

					hit.transform.GetComponentInParent<RCC_CarControllerV3> ().SendMessage ("Act", rootOfPlayerBSR, SendMessageOptions.DontRequireReceiver);
					
				}

			}else{

				showGuiBSR = false;

			}
			
		}else{

			showGuiBSR = false;

		}
		
	}
	
	private void OnGUI (){
		
		if(showGuiBSR){
			if(RCC_SettingsBSR.InstanceBSR.controllerTypeBSR == RCC_SettingsBSR.ControllerType.Keyboard)
				GUI.Label( new Rect(Screen.width - (Screen.width/1.7f),Screen.height - (Screen.height/1.2f),800,100),"Press ''" + RCC_SettingsBSR.InstanceBSR.enterExitVehicleKBBSR.ToString() + "'' key to Get In");
		}
		
	}

	private void OnDrawGizmos(){

		Gizmos.color = Color.red;
		Gizmos.DrawRay (new Vector3(transform.position.x, transform.position.y + (playerTypeBSR == PlayerType.TPS ? rayHeightBSR : 0f), transform.position.z), transform.forward * maxRayDistanceBSR);
		
	}

	private void OnEnable(){

		if (TPSCameraBSR)
			TPSCameraBSR.SetActive (true);

	}

	private void OnDisable(){

		if (TPSCameraBSR)
			TPSCameraBSR.SetActive (false);

	}
	
}