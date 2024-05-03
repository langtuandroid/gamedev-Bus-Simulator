//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Steering Wheel")]
public class RCC_UISteeringWheelControllerBSR : MonoBehaviour {

	private GameObject steeringWheelGameObjectBSR;
	private Image steeringWheelTextureBSR;

	[FormerlySerializedAs("input")] public float inputBSR = 0f;
	[FormerlySerializedAs("steeringWheelAngle")] public float steeringWheelAngleBSR = 0f;
	[FormerlySerializedAs("steeringWheelMaximumsteerAngle")] public float steeringWheelMaximumsteerAngleBSR = 270f;
	[FormerlySerializedAs("steeringWheelResetPosSpeed")] public float steeringWheelResetPosSpeedBSR = 20f;	
	[FormerlySerializedAs("steeringWheelCenterDeadZoneRadius")] public float steeringWheelCenterDeadZoneRadiusBSR = 5f;

	private RectTransform steeringWheelRectBSR;
	private CanvasGroup steeringWheelCanvasGroupBSR;

	private float steeringWheelTempAngleBSR, steeringWheelNewAngleBSR;
	private bool steeringWheelPressedBSR;

	private Vector2 steeringWheelCenterBSR, steeringWheelTouchPosBSR;

	private EventTrigger eventTriggerBSR;

	private void Awake(){

		steeringWheelTextureBSR = GetComponent<Image>();

	}

	private void Update () {

		if(!RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR)
			return;

		if(!steeringWheelRectBSR && steeringWheelTextureBSR){
			SteeringWheelInitBSR();
		}

		SteeringWheelControllingBSR();

		inputBSR = GetSteeringWheelInputBSR();

	}

	private void SteeringWheelInitBSR(){

		steeringWheelGameObjectBSR = steeringWheelTextureBSR.gameObject;
		steeringWheelRectBSR = steeringWheelTextureBSR.rectTransform;
		steeringWheelCanvasGroupBSR = steeringWheelTextureBSR.GetComponent<CanvasGroup> ();
		steeringWheelCenterBSR = steeringWheelRectBSR.position;
		
		SteeringWheelEventsInitBSR ();

	}

	//Events Initialization For Steering Wheel.
	private void SteeringWheelEventsInitBSR(){

		eventTriggerBSR = steeringWheelGameObjectBSR.GetComponent<EventTrigger>();
		
		var a = new EventTrigger.TriggerEvent();
		a.AddListener( data => 
		              {
			var evData = (PointerEventData)data;
			data.Use();
			
			steeringWheelPressedBSR = true;
			steeringWheelTouchPosBSR = evData.position;
			steeringWheelTempAngleBSR = Vector2.Angle(Vector2.up, evData.position - steeringWheelCenterBSR);
		});
		
		eventTriggerBSR.triggers.Add(new EventTrigger.Entry{callback = a, eventID = EventTriggerType.PointerDown});
		
		
		var b = new EventTrigger.TriggerEvent();
		b.AddListener( data => 
		              {
			var evData = (PointerEventData)data;
			data.Use();
			steeringWheelTouchPosBSR = evData.position;
		});
		
		eventTriggerBSR.triggers.Add(new EventTrigger.Entry{callback = b, eventID = EventTriggerType.Drag});
		
		
		var c = new EventTrigger.TriggerEvent();
		c.AddListener( data => 
		              {
			steeringWheelPressedBSR = false;
		});
		
		eventTriggerBSR.triggers.Add(new EventTrigger.Entry{callback = c, eventID = EventTriggerType.EndDrag});

	}

	public float GetSteeringWheelInputBSR(){

		return Mathf.Round(steeringWheelAngleBSR / steeringWheelMaximumsteerAngleBSR * 100) / 100;

	}

	public bool isSteeringWheelPressedBSR(){

		return steeringWheelPressedBSR;

	}

	public void SteeringWheelControllingBSR (){

		if(!steeringWheelCanvasGroupBSR || !steeringWheelRectBSR || !RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR){
			if(steeringWheelGameObjectBSR)
				steeringWheelGameObjectBSR.SetActive(false);
			return;
		}

		if(!steeringWheelGameObjectBSR.activeSelf)
			steeringWheelGameObjectBSR.SetActive(true);

		if(steeringWheelPressedBSR){

			steeringWheelNewAngleBSR = Vector2.Angle(Vector2.up, steeringWheelTouchPosBSR - steeringWheelCenterBSR);

			if(Vector2.Distance( steeringWheelTouchPosBSR, steeringWheelCenterBSR ) > steeringWheelCenterDeadZoneRadiusBSR){

				if(steeringWheelTouchPosBSR.x > steeringWheelCenterBSR.x)
					steeringWheelAngleBSR += steeringWheelNewAngleBSR - steeringWheelTempAngleBSR;
				else
					steeringWheelAngleBSR -= steeringWheelNewAngleBSR - steeringWheelTempAngleBSR;

			}

			if(steeringWheelAngleBSR > steeringWheelMaximumsteerAngleBSR)
				steeringWheelAngleBSR = steeringWheelMaximumsteerAngleBSR;
			else if(steeringWheelAngleBSR < -steeringWheelMaximumsteerAngleBSR)
				steeringWheelAngleBSR = -steeringWheelMaximumsteerAngleBSR;
			
			steeringWheelTempAngleBSR = steeringWheelNewAngleBSR;

		}else{

			if(!Mathf.Approximately(0f, steeringWheelAngleBSR)){

				float deltaAngle = steeringWheelResetPosSpeedBSR;
				
				if(Mathf.Abs(deltaAngle) > Mathf.Abs(steeringWheelAngleBSR)){
					steeringWheelAngleBSR = 0f;
					return;
				}
				
				if(steeringWheelAngleBSR > 0f)
					steeringWheelAngleBSR -= deltaAngle;
				else
					steeringWheelAngleBSR += deltaAngle;

			}

		}

		steeringWheelRectBSR.eulerAngles = new Vector3 (0f, 0f, -steeringWheelAngleBSR);
		
	}

	private void OnDisable(){
		
		steeringWheelPressedBSR = false;
		inputBSR = 0f;

	}

}
