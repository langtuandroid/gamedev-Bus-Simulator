//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Shadow Rotation Const")]
public class RCC_ShadowRotConstBSR : MonoBehaviour {

	private Transform rootBSR;

	private void Start () {

		rootBSR = GetComponentInParent<RCC_CarControllerV3>().transform;
	
	}

	private void Update () {

		transform.rotation = Quaternion.Euler(90, rootBSR.eulerAngles.y, 0);
	
	}

}
