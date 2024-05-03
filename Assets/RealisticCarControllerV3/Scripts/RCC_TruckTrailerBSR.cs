//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Truck Trailer")]
[RequireComponent (typeof(Rigidbody))]
public class RCC_TruckTrailerBSR : MonoBehaviour {

	private RCC_CarControllerV3 carControllerBSR;
	private Rigidbody rigidBSR;
	[FormerlySerializedAs("COM")] public Transform COMBSR;

	//Extra Wheels.
	[FormerlySerializedAs("wheelColliders")] public WheelCollider[] wheelCollidersBSR;
	private List<WheelCollider> leftWheelCollidersBSR = new List<WheelCollider>();
	private List<WheelCollider> rightWheelCollidersBSR = new List<WheelCollider>();

	[FormerlySerializedAs("antiRoll")] public float antiRollBSR = 50000f;

	private void Start () {

		rigidBSR = GetComponent<Rigidbody>();
		carControllerBSR = transform.GetComponentInParent<RCC_CarControllerV3>();

		rigidBSR.interpolation = RigidbodyInterpolation.None;
		rigidBSR.interpolation = RigidbodyInterpolation.Interpolate;


		antiRollBSR = carControllerBSR.antiRollFrontHorizontal;

		for (int i = 0; i < wheelCollidersBSR.Length; i++) {

			if(wheelCollidersBSR[i].transform.localPosition.x < 0f)
				leftWheelCollidersBSR.Add(wheelCollidersBSR[i]);
			else
				rightWheelCollidersBSR.Add(wheelCollidersBSR[i]);

		}

		gameObject.SetActive (false);
		gameObject.SetActive (true);

	}

	private void FixedUpdate(){
		rigidBSR.centerOfMass = transform.InverseTransformPoint(COMBSR.transform.position);
		AntiRollBarsBSR();

		//Applying Small Torque For Preventing Stuck Issue. Unity 5 WheelColliders Are Weird
		foreach(WheelCollider wc in wheelCollidersBSR){
			wc.motorTorque = carControllerBSR._gasInput * (carControllerBSR.engineTorque / 10f);
		}

	}

	public void AntiRollBarsBSR (){

		for (int i = 0; i < leftWheelCollidersBSR.Count; i++) {

			WheelHit hit;

			float travelL = 1.0f;
			float travelR = 1.0f;

			bool groundedL= leftWheelCollidersBSR[i].GetGroundHit(out hit);

			if (groundedL)
				travelL = (-leftWheelCollidersBSR[i].transform.InverseTransformPoint(hit.point).y - leftWheelCollidersBSR[i].radius) / leftWheelCollidersBSR[i].suspensionDistance;

			bool groundedR= rightWheelCollidersBSR[i].GetGroundHit(out hit);

			if (groundedR)
				travelR = (-rightWheelCollidersBSR[i].transform.InverseTransformPoint(hit.point).y - rightWheelCollidersBSR[i].radius) / rightWheelCollidersBSR[i].suspensionDistance;

			float antiRollForce= (travelL - travelR) * antiRollBSR;

			if (groundedL)
				rigidBSR.AddForceAtPosition(leftWheelCollidersBSR[i].transform.up * -antiRollForce, leftWheelCollidersBSR[i].transform.position); 
			if (groundedR)
				rigidBSR.AddForceAtPosition(rightWheelCollidersBSR[i].transform.up * antiRollForce, rightWheelCollidersBSR[i].transform.position); 

		}

	}

}
