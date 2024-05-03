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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Orbit Camera")]
public class RCC_CameraOrbitBSR : MonoBehaviour
{

	[FormerlySerializedAs("target")] public Transform targetBSR;
	[FormerlySerializedAs("distance")] public float distanceBSR= 10.0f;
	
	[FormerlySerializedAs("xSpeed")] public float xSpeedBSR= 250f;
	[FormerlySerializedAs("ySpeed")] public float  ySpeedBSR= 120f;
	
	[FormerlySerializedAs("yMinLimit")] public float yMinLimitBSR= -20f;
	[FormerlySerializedAs("yMaxLimit")] public float yMaxLimitBSR= 80f;
	
	private float xBSR= 0f;
	private float yBSR= 0f;
		
	private void  Start (){

		Vector3 angles= transform.eulerAngles;
		xBSR = angles.y;
		yBSR = angles.x;

	}
	
	private void  LateUpdate (){

		if (targetBSR) {

			xBSR += Input.GetAxis("Mouse X") * xSpeedBSR * 0.02f;
			yBSR -= Input.GetAxis("Mouse Y") * ySpeedBSR * 0.02f;
			
			yBSR = ClampAngleBSR(yBSR, yMinLimitBSR, yMaxLimitBSR);
			
			Quaternion rotation= Quaternion.Euler(yBSR, xBSR, 0);
			Vector3 position= rotation * new Vector3(0f, 0f, -distanceBSR) + targetBSR.position;
			
			transform.rotation = rotation;
			transform.position = position;
		}

	}
	
	static float ClampAngleBSR ( float angle ,   float min ,   float max  ){

		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);

	}
	
}