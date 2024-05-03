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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Fixed Camera")]
public class RCC_FixedCameraBSR : MonoBehaviour {

	[FormerlySerializedAs("currentCar")] public Transform currentCarBSR;
	private RCC_CameraBSR rccCameraBSR;

	[FormerlySerializedAs("maxDistance")] public float maxDistanceBSR = 50f;
	private float distanceBSR;

	[FormerlySerializedAs("minimumFOV")] public float minimumFOVBSR = 20f;
	[FormerlySerializedAs("maximumFOV")] public float maximumFOVBSR = 60f;
	[FormerlySerializedAs("canTrackNow")] public bool canTrackNowBSR = false;

	private float timerBSR = 0f;
	[FormerlySerializedAs("updateInSeconds")] public float updateInSecondsBSR = .05f;

	private void Start(){

		rccCameraBSR = GameObject.FindObjectOfType<RCC_CameraBSR> ();

	}

	private void Update(){

		if (!canTrackNowBSR)
			return;

		if (!rccCameraBSR) {
			rccCameraBSR = GameObject.FindObjectOfType<RCC_CameraBSR> ();
			return;
		}

		if (!currentCarBSR) {
			currentCarBSR = rccCameraBSR.playerCarBSR;
			return;
		}

		CheckCullingBSR ();
			
		distanceBSR = Vector3.Distance (transform.position, currentCarBSR.position);
		rccCameraBSR.targetFieldOfViewBSR = Mathf.Lerp (maximumFOVBSR, minimumFOVBSR, distanceBSR / maxDistanceBSR);
		transform.LookAt (currentCarBSR.position);

	}

	private void CheckCullingBSR(){

		timerBSR += Time.deltaTime;

		if (timerBSR < updateInSecondsBSR) {
			return;
		} else {
			timerBSR = 0f;
		}
			
		RaycastHit hit;

		if ((Physics.Linecast (currentCarBSR.position, transform.position, out hit) && !hit.transform.IsChildOf (currentCarBSR) && !hit.collider.isTrigger) || distanceBSR >= maxDistanceBSR) {
			ChangePositionBSR ();
		}

	}

	private void ChangePositionBSR(){

		float randomizedAngle = Random.Range (-15f, 15f);
		RaycastHit hit;

		if (Physics.Raycast (currentCarBSR.position, Quaternion.AngleAxis (randomizedAngle, currentCarBSR.up) * currentCarBSR.forward, out hit, maxDistanceBSR) && !hit.transform.IsChildOf(currentCarBSR) && !hit.collider.isTrigger) {
			transform.position = hit.point;
			transform.LookAt (currentCarBSR.position + new Vector3(0f, Mathf.Clamp(randomizedAngle, 0f, 10f), 0f));
			transform.position += transform.rotation * Vector3.forward * 3f;
		} else {
			transform.position = currentCarBSR.position + new Vector3(0f, Mathf.Clamp(randomizedAngle, 0f, 5f), 0f);
			transform.position += Quaternion.AngleAxis (randomizedAngle, currentCarBSR.up) * currentCarBSR.forward * (maxDistanceBSR * .9f);
		}

	}
	
}
