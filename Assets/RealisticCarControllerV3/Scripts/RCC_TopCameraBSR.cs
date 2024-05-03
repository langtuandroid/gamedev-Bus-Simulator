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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Top Camera")]
public class RCC_TopCameraBSR : MonoBehaviour{
	
	// The target we are following
	[FormerlySerializedAs("playerCar")] public Transform playerCarBSR;
	public Transform _playerCarBSR{get{return playerCarBSR;}set{playerCarBSR = value;	GetPlayerCarBSR();}}
	private Rigidbody playerRigidBSR;

	private Camera camBSR;
	[FormerlySerializedAs("pivot")] public GameObject pivotBSR;

	// The distance in the x-z plane to the target
	[FormerlySerializedAs("distance")] public float distanceBSR = 20f;
	private float distanceOffsetBSR = 0f;
	[FormerlySerializedAs("maximumDistanceOffset")] public float maximumDistanceOffsetBSR = 10f;

	private float targetFieldOfViewBSR = 60f;
	[FormerlySerializedAs("minimumOrtSize")] public float minimumOrtSizeBSR =7.5f;
	[FormerlySerializedAs("maximumOrtSize")] public float maximumOrtSizeBSR = 12.5f;

	private Vector3 targetPositionBSR, pastFollowerPositionBSR = Vector3.zero;
	private Vector3 pastTargetPositionBSR = Vector3.zero;

	private float speedBSR = 0f;

	private void Awake(){

		camBSR = GetComponentInChildren<Camera>();

	}
	
	private void GetPlayerCarBSR(){

		if(!playerCarBSR)
			return;

		playerRigidBSR = playerCarBSR.GetComponent<Rigidbody>();

		transform.position = playerCarBSR.transform.position;

		if(playerCarBSR.GetComponent<RCC_CameraConfigBSR>())
			playerCarBSR.GetComponent<RCC_CameraConfigBSR>().SetCameraSettingsBSR();

	}

	public void SetPlayerCarBSR(GameObject player){

		_playerCarBSR = player.transform;

	}
	
	private void Update(){
		
		// Early out if we don't have a player
		if (!playerCarBSR || !playerRigidBSR){
			GetPlayerCarBSR();
			return;
		}

		// Speed of the vehicle.
		speedBSR = Mathf.Lerp(speedBSR, playerRigidBSR.velocity.magnitude * 3.6f, Time.deltaTime * .5f);

		distanceOffsetBSR = Mathf.Lerp (0f, maximumDistanceOffsetBSR, speedBSR / 100f);
		targetFieldOfViewBSR = Mathf.Lerp (minimumOrtSizeBSR, maximumOrtSizeBSR, speedBSR / 100f);
		camBSR.orthographicSize = targetFieldOfViewBSR;

		targetPositionBSR = playerCarBSR.position;
		targetPositionBSR += playerCarBSR.rotation * Vector3.forward * distanceOffsetBSR;

		transform.position = SmoothApproachBSR(pastFollowerPositionBSR, pastTargetPositionBSR, targetPositionBSR, Mathf.Clamp(.5f, speedBSR, Mathf.Infinity));

		pastFollowerPositionBSR = transform.position;
		pastTargetPositionBSR = targetPositionBSR;

		pivotBSR.transform.localPosition = new Vector3 (0f, 0f, -distanceBSR);

	}

	private Vector3 SmoothApproachBSR( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float delta){

		if(float.IsNaN(delta) || float.IsInfinity(delta) || pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
			return transform.position;

		float t = Time.deltaTime * delta;
		Vector3 v = ( targetPosition - pastTargetPosition ) / t;
		Vector3 f = pastPosition - pastTargetPosition + v;
		return targetPosition - v + f * Mathf.Exp( -t );

	}

}