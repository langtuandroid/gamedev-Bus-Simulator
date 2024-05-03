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

public class RCC_UselessBSR : MonoBehaviour {

	[FormerlySerializedAs("useless")] public Useless uselessBSR;
	public enum Useless{Controller, Behavior}

	// Use this for initialization
	private void Awake () {

		int type = 0;

		if(uselessBSR == Useless.Behavior){

			RCC_SettingsBSR.BehaviorType behavior = RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR;

			switch(behavior){
			case(RCC_SettingsBSR.BehaviorType.Simulator):
				type = 0;
				break;
			case(RCC_SettingsBSR.BehaviorType.Racing):
				type = 1;
				break;
			case(RCC_SettingsBSR.BehaviorType.SemiArcade):
				type = 2;
				break;
			case(RCC_SettingsBSR.BehaviorType.Drift):
				type = 3;
				break;
			case(RCC_SettingsBSR.BehaviorType.Fun):
				type = 4;
				break;
			case(RCC_SettingsBSR.BehaviorType.Custom):
				type = 5;
				break;
			}

		}else{

			if(!RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR && !RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR)
				type = 0;
			if(RCC_SettingsBSR.InstanceBSR.useAccelerometerForSteeringBSR)
				type = 1;
			if(RCC_SettingsBSR.InstanceBSR.useSteeringWheelForSteeringBSR)
				type = 2;

		}

		GetComponent<Dropdown>().value = type;
		GetComponent<Dropdown>().RefreshShownValue();
	
	}

}
