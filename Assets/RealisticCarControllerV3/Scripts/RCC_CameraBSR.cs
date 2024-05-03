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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Main Camera")]
public class RCC_CameraBSR : MonoBehaviour{
	
	// The target we are following
	[FormerlySerializedAs("playerCar")] public Transform playerCarBSR;
	public Transform _playerCarBSR{get{return playerCarBSR;}set{playerCarBSR = value;	GetPlayerCarBSR();}}
	private Rigidbody playerRigidBSR;

	private Camera camBSR;
	[FormerlySerializedAs("pivot")] public GameObject pivotBSR;
	private GameObject boundCenterBSR;

	[FormerlySerializedAs("cameraMode")] public CameraMode cameraModeBSR;
	public enum CameraMode{TPS, FPS, WHEEL, FIXED}

	// The distance in the x-z plane to the target
	[FormerlySerializedAs("distance")] public float distanceBSR = 6f;
	
	// the height we want the camera to be above the target
	[FormerlySerializedAs("height")] public float heightBSR = 2f;

	private float heightDampingBSR = 5f;
	private float rotationDampingBSR = 3f;

	[FormerlySerializedAs("targetFieldOfView")] public float targetFieldOfViewBSR = 60f;
	[FormerlySerializedAs("minimumFOV")] public float minimumFOVBSR = 55f;
	[FormerlySerializedAs("maximumFOV")] public float maximumFOVBSR = 70f;
	[FormerlySerializedAs("hoodCameraFOV")] public float hoodCameraFOVBSR = 70f;
	[FormerlySerializedAs("wheelCameraFOV")] public float wheelCameraFOVBSR = 60f;
	
	[FormerlySerializedAs("maximumTilt")] public float maximumTiltBSR = 15f;
	private float tiltAngleBSR = 0f;

	internal int cameraSwitchCountBSR = 0;
	private RCC_HoodCameraBSR hoodCamBSR;
	private RCC_WheelCameraBSR wheelCamBSR;
	private RCC_FixedCameraBSR fixedCamBSR;

	private Vector3 targetPositionBSR = Vector3.zero;

	private float speedBSR = 0f;

	private Vector3 localVectorBSR;
	private Vector3 collisionPosBSR;
	private Quaternion collisionRotBSR;

	private float indexBSR = 0f;

	private void Awake(){

		camBSR = GetComponentInChildren<Camera>();

	}
	
	private void GetPlayerCarBSR(){

		if(!playerCarBSR)
			return;

		cameraModeBSR = CameraMode.TPS;
		playerRigidBSR = playerCarBSR.GetComponent<Rigidbody>();
		hoodCamBSR = playerCarBSR.GetComponentInChildren<RCC_HoodCameraBSR>();
		wheelCamBSR = playerCarBSR.GetComponentInChildren<RCC_WheelCameraBSR>();
		fixedCamBSR = GameObject.FindObjectOfType<RCC_FixedCameraBSR>();

		transform.position = playerCarBSR.transform.position;
		transform.rotation = playerCarBSR.transform.rotation * Quaternion.Euler(10f, 0f, 0f);

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

		if(indexBSR > 0)
			indexBSR -= Time.deltaTime * 5f;

		if(cameraModeBSR == CameraMode.TPS){
			
		}

		camBSR.fieldOfView = Mathf.Lerp (camBSR.fieldOfView, targetFieldOfViewBSR, Time.deltaTime * 3f);
			
	}
	
	private void LateUpdate (){
		
		// Early out if we don't have a target
		if (!playerCarBSR || !playerRigidBSR)
			return;

		if (!playerCarBSR.gameObject.activeSelf)
			return;

		if(Input.GetKeyDown(RCC_SettingsBSR.InstanceBSR.changeCameraKBBSR)){
			ChangeCameraBSR();
		}

		switch(cameraModeBSR){

		case CameraMode.TPS:
			TPSBSR();
			break;

		case CameraMode.FPS:
			if(hoodCamBSR){
				FPSBSR();
			}else{
				ChangeCameraBSR();
			}
			break;
		
		case CameraMode.WHEEL:
			if(wheelCamBSR){
				WHEELBSR();
			}else{
				ChangeCameraBSR();
			}
			break;

		case CameraMode.FIXED:
			if(fixedCamBSR){
				FIXEDBSR();
			}else{
				ChangeCameraBSR();
			}
			break;

		}

	}

	public void ChangeCameraBSR(){

		cameraSwitchCountBSR ++;
		if(cameraSwitchCountBSR >= 4)
			cameraSwitchCountBSR = 0;
		
		if(fixedCamBSR)
			fixedCamBSR.canTrackNowBSR = false;

		switch(cameraSwitchCountBSR){
		case 0:
			cameraModeBSR = CameraMode.TPS;
			break;
		case 1:
			cameraModeBSR = CameraMode.FPS;
			break;
		case 2:
			cameraModeBSR = CameraMode.WHEEL;
			break;
		case 3:
			cameraModeBSR = CameraMode.FIXED;
			break;
		}

	}

	private void FPSBSR(){

		if(transform.parent != hoodCamBSR.transform){
			transform.SetParent(hoodCamBSR.transform, false);
			transform.position = hoodCamBSR.transform.position;
			transform.rotation = hoodCamBSR.transform.rotation;
			targetFieldOfViewBSR = hoodCameraFOVBSR;
			hoodCamBSR.FixShakeBSR ();
		}

	}

	private void WHEELBSR(){

		if(transform.parent != wheelCamBSR.transform){
			transform.SetParent(wheelCamBSR.transform, false);
			transform.position = wheelCamBSR.transform.position;
			transform.rotation = wheelCamBSR.transform.rotation;
			targetFieldOfViewBSR = wheelCameraFOVBSR;
		}

	}

	private void TPSBSR(){

		if(transform.parent != null)
			transform.SetParent(null);

		if(targetPositionBSR == Vector3.zero){
			targetPositionBSR = _playerCarBSR.position;
			targetPositionBSR -= transform.rotation * Vector3.forward * distanceBSR;
			transform.position = targetPositionBSR;
		}

		speedBSR = (playerRigidBSR.transform.InverseTransformDirection(playerRigidBSR.velocity).z) * 3.6f;
		targetFieldOfViewBSR = Mathf.Lerp(minimumFOVBSR, maximumFOVBSR, speedBSR / 150f) + (5f * Mathf.Cos (1f * indexBSR));
		tiltAngleBSR = Mathf.Lerp(0f, maximumTiltBSR * (int)Mathf.Clamp(-playerCarBSR.InverseTransformDirection(playerRigidBSR.velocity).x, -1, 1), Mathf.Abs(playerCarBSR.InverseTransformDirection(playerRigidBSR.velocity).x) / 50f);

		// Calculate the current rotation angles.
		float wantedRotationAngle = playerCarBSR.eulerAngles.y;
		float wantedHeight = playerCarBSR.position.y + heightBSR;
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		rotationDampingBSR = Mathf.Lerp(0f, 3f, (playerRigidBSR.velocity.magnitude * 3f) / 40f);

		if(speedBSR < -10)
			wantedRotationAngle = playerCarBSR.eulerAngles.y + 180;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDampingBSR * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight + Mathf.Lerp(-.5f, 0f, (speedBSR) / 20f), heightDampingBSR * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = playerCarBSR.position;
		transform.position -= currentRotation * Vector3.forward * distanceBSR;

		// Set the height of the camera
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

		// Always look at the target
		transform.LookAt (new Vector3(playerCarBSR.position.x, playerCarBSR.position.y + 1f, playerCarBSR.position.z));
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, Mathf.Clamp(tiltAngleBSR, -10f, 10f));

		//pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, (new Vector3(Random.insideUnitSphere.x / 2f, Random.insideUnitSphere.y, Random.insideUnitSphere.z) * speed * maxShakeAmount), Time.deltaTime * 1f);
		collisionPosBSR = Vector3.Lerp(collisionPosBSR, Vector3.zero, Time.deltaTime * 5f);
		collisionRotBSR = Quaternion.Lerp(collisionRotBSR, Quaternion.identity, Time.deltaTime * 5f);
		pivotBSR.transform.localPosition = Vector3.Lerp(pivotBSR.transform.localPosition, collisionPosBSR, Time.deltaTime * 5f);
		pivotBSR.transform.localRotation = Quaternion.Lerp(pivotBSR.transform.localRotation, collisionRotBSR, Time.deltaTime * 5f);

	}

	private void FIXEDBSR(){

		if(transform.parent != fixedCamBSR.transform){
			transform.SetParent(fixedCamBSR.transform, false);
			transform.position = fixedCamBSR.transform.position;
			transform.rotation = fixedCamBSR.transform.rotation;
			targetFieldOfViewBSR = 60;
			fixedCamBSR.currentCarBSR = playerCarBSR;
			fixedCamBSR.canTrackNowBSR = true;
		}

		if(fixedCamBSR.transform.parent != null)
			fixedCamBSR.transform.SetParent(null);

	}

	public void CollisionBSR(Collision collision){

		if(!enabled || cameraModeBSR != CameraMode.TPS)
			return;
		
		Vector3 colRelVel = collision.relativeVelocity;
		colRelVel *= 1f - Mathf.Abs(Vector3.Dot(transform.up,collision.contacts[0].normal));

		float cos = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, colRelVel.normalized));

		if (colRelVel.magnitude * cos >= 5f){

			localVectorBSR = transform.InverseTransformDirection(colRelVel) / (30f);

			collisionPosBSR -= localVectorBSR * 3f;
			collisionRotBSR = Quaternion.Euler(new Vector3(-localVectorBSR.z * 30f, -localVectorBSR.y * 30f, -localVectorBSR.x * 30f));
			targetFieldOfViewBSR = camBSR.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 15f);
			indexBSR = Mathf.Clamp(collision.relativeVelocity.magnitude / 5f, 0f, 10f);

		}

	}

}