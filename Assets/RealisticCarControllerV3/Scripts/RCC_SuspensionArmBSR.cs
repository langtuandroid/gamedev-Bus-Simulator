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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Suspension Distance Based Axle")]
public class RCC_SuspensionArmBSR : MonoBehaviour {

	[FormerlySerializedAs("wheelcollider")] public WheelCollider wheelcolliderBSR;

	[FormerlySerializedAs("axis")] public Axis axisBSR;
	public enum Axis {X, Y, Z}

	private Vector3 orgRotBSR;
	private float totalSuspensionDistanceBSR = 0;

	[FormerlySerializedAs("offsetAngle")] public float offsetAngleBSR = 30;
	[FormerlySerializedAs("angleFactor")] public float angleFactorBSR = 150;
	
	private void Start () {
		
		orgRotBSR = transform.localEulerAngles;
		totalSuspensionDistanceBSR = GetSuspensionDistanceBSR ();

	}

	private void FixedUpdate () {
		
		float suspensionCourse = GetSuspensionDistanceBSR () - totalSuspensionDistanceBSR;
		transform.localEulerAngles = orgRotBSR;

		switch(axisBSR){

		case Axis.X:
			transform.Rotate (Vector3.right, suspensionCourse * angleFactorBSR - offsetAngleBSR, Space.Self);
			break;
		case Axis.Y:
			transform.Rotate (Vector3.up, suspensionCourse * angleFactorBSR - offsetAngleBSR, Space.Self);
			break;
		case Axis.Z:
			transform.Rotate (Vector3.forward, suspensionCourse * angleFactorBSR - offsetAngleBSR, Space.Self);
			break;

		}

	}
		
	private float GetSuspensionDistanceBSR() {
		
		Quaternion quat;
		Vector3 position;
		wheelcolliderBSR.GetWorldPose(out position, out quat);
		Vector3 local = wheelcolliderBSR.transform.InverseTransformPoint (position);
		return local.y;

	}

}
