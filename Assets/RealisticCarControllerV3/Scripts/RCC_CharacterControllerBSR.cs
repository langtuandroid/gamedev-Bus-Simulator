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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Animator Controller")]
public class RCC_CharacterControllerBSR : MonoBehaviour {

	private RCC_CarControllerV3 carControllerBSR;
	private Rigidbody carRigidBSR;
	[FormerlySerializedAs("animator")] public Animator animatorBSR;

	[FormerlySerializedAs("driverSteeringParameter")] public string driverSteeringParameterBSR;
	[FormerlySerializedAs("driverShiftingGearParameter")] public string driverShiftingGearParameterBSR;
	[FormerlySerializedAs("driverDangerParameter")] public string driverDangerParameterBSR;
	[FormerlySerializedAs("driverReversingParameter")] public string driverReversingParameterBSR;

	[FormerlySerializedAs("steerInput")] public float steerInputBSR = 0f;
	[FormerlySerializedAs("directionInput")] public float directionInputBSR = 0f;
	[FormerlySerializedAs("reversing")] public bool reversingBSR = false;
	[FormerlySerializedAs("impactInput")] public float impactInputBSR = 0f;
	[FormerlySerializedAs("gearInput")] public float gearInputBSR = 0f;

	private void Start () {

		if(!animatorBSR)
			animatorBSR = GetComponentInChildren<Animator>();
		carControllerBSR = GetComponent<RCC_CarControllerV3>();
		carRigidBSR = GetComponent<Rigidbody>();
		
	}

	private void Update () {

		steerInputBSR = Mathf.Lerp(steerInputBSR, carControllerBSR.steerInput, Time.deltaTime * 5f);
		directionInputBSR = carRigidBSR.transform.InverseTransformDirection(carRigidBSR.velocity).z;
		impactInputBSR -= Time.deltaTime * 5f;

		if(impactInputBSR < 0)
			impactInputBSR = 0f;
		if(impactInputBSR > 1)
			impactInputBSR = 1f;

		if(directionInputBSR <= -2f)
			reversingBSR = true;
		else if(directionInputBSR > -1f)
			reversingBSR = false;

		if(carControllerBSR.changingGear)
			gearInputBSR = 1f;
		else
			gearInputBSR -= Time.deltaTime * 5f;

		if(gearInputBSR < 0)
			gearInputBSR = 0f;
		if(gearInputBSR > 1)
			gearInputBSR = 1f;

		if(!reversingBSR){
			animatorBSR.SetBool(driverReversingParameterBSR, false);
		}else{
			animatorBSR.SetBool(driverReversingParameterBSR, true);
		}

		if(impactInputBSR > .5f){
			animatorBSR.SetBool(driverDangerParameterBSR, true);
		}else{
			animatorBSR.SetBool(driverDangerParameterBSR, false);
		}

		if(gearInputBSR > .5f){
			animatorBSR.SetBool(driverShiftingGearParameterBSR, true);
		}else{
			animatorBSR.SetBool(driverShiftingGearParameterBSR, false);
		}

		animatorBSR.SetFloat(driverSteeringParameterBSR, steerInputBSR);
		
	}

	private void OnCollisionEnter(Collision col){

		if(col.relativeVelocity.magnitude < 2.5f)
			return;

		impactInputBSR = 1f;

	}

}
