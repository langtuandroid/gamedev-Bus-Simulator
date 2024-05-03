//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Button")]
public class RCC_UIControllerBSR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	internal float inputBSR;
	private float sensitivityBSR{get{return RCC_SettingsBSR.InstanceBSR.UIButtonSensitivityBSR;}}
	private float gravityBSR{get{return RCC_SettingsBSR.InstanceBSR.UIButtonGravityBSR;}}
	[FormerlySerializedAs("pressing")] public bool pressingBSR;

	public void OnPointerDown(PointerEventData eventData){

		pressingBSR = true;

	}

	public void OnPointerUp(PointerEventData eventData){
		 
		pressingBSR = false;
		
	}

	void OnPress (bool isPressed){

		if(isPressed)
			pressingBSR = true;
		else
			pressingBSR = false;

	}

	private void FixedUpdate(){
		
		if(pressingBSR)
			inputBSR += Time.fixedDeltaTime * sensitivityBSR;
		else
			inputBSR -= Time.fixedDeltaTime * gravityBSR;
		
		if(inputBSR < 0f)
			inputBSR = 0f;
		if(inputBSR > 1f)
			inputBSR = 1f;
		
	}

	private void OnDisable(){

		inputBSR = 0f;
		pressingBSR = false;

	}

}
