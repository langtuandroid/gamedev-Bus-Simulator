//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/AI Controller")]
public class RCC_AICarControllerBSR : MonoBehaviour {

	private RCC_CarControllerV3 carControllerBSR;
	private Rigidbody rigidBSR;
	
	// Waypoint Container.
	private RCC_AIWaypointsContainerBSR waypointsContainerBSR;
	[FormerlySerializedAs("currentWaypoint")] public int currentWaypointBSR = 0;

	// AI Type
	[FormerlySerializedAs("_AIType")] public AIType _AITypeBSR;
	public enum AIType {FollowWaypoints, ChasePlayer}
	
	// Raycast distances.
	[FormerlySerializedAs("obstacleLayers")] public LayerMask obstacleLayersBSR = -1;
	[FormerlySerializedAs("wideRayLength")] public int wideRayLengthBSR = 20;
	[FormerlySerializedAs("tightRayLength")] public int tightRayLengthBSR = 20;
	[FormerlySerializedAs("sideRayLength")] public int sideRayLengthBSR = 3;
	private float rayInputBSR = 0f;
	private bool  raycastingBSR = false;
	private float resetTimeBSR = 0f; 
	
	// Steer, motor, and brake inputs.
	private float steerInputBSR = 0f;
	private float gasInputBSR = 0f;
	private float brakeInputBSR = 0f;

	[FormerlySerializedAs("limitSpeed")] public bool limitSpeedBSR = false;
	[FormerlySerializedAs("maximumSpeed")] public float maximumSpeedBSR = 100f;

	[FormerlySerializedAs("smoothedSteer")] public bool smoothedSteerBSR = true;
	
	// Brake Zone.
	private float maximumSpeedInBrakeZoneBSR = 0f;
	private bool inBrakeZoneBSR = false;
	
	// Counts laps and how many waypoints passed.
	[FormerlySerializedAs("lap")] public int lapBSR = 0;
	[FormerlySerializedAs("totalWaypointPassed")] public int totalWaypointPassedBSR = 0;
	[FormerlySerializedAs("nextWaypointPassRadius")] public int nextWaypointPassRadiusBSR = 40;
	[FormerlySerializedAs("ignoreWaypointNow")] public bool ignoreWaypointNowBSR = false;
	
	// Unity's Navigator.
	private UnityEngine.AI.NavMeshAgent navigatorBSR;
	private GameObject navigatorObjectBSR;

	private void Awake() {

		carControllerBSR = GetComponent<RCC_CarControllerV3>();
		rigidBSR = GetComponent<Rigidbody>();
		carControllerBSR.AIController = true;
		waypointsContainerBSR = FindObjectOfType(typeof(RCC_AIWaypointsContainerBSR)) as RCC_AIWaypointsContainerBSR;

		navigatorObjectBSR = new GameObject("Navigator");
		navigatorObjectBSR.transform.parent = transform;
		navigatorObjectBSR.transform.localPosition = Vector3.zero;
		navigatorObjectBSR.AddComponent<UnityEngine.AI.NavMeshAgent>();
		navigatorObjectBSR.GetComponent<UnityEngine.AI.NavMeshAgent>().radius = 1;
		navigatorObjectBSR.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 1;
		navigatorObjectBSR.GetComponent<UnityEngine.AI.NavMeshAgent>().angularSpeed = 1000f;
		navigatorObjectBSR.GetComponent<UnityEngine.AI.NavMeshAgent>().height = 1;
		navigatorObjectBSR.GetComponent<UnityEngine.AI.NavMeshAgent>().avoidancePriority = 50;
		navigatorBSR = navigatorObjectBSR.GetComponent<UnityEngine.AI.NavMeshAgent>();

	}
	
	private void Update(){
		
		navigatorBSR.transform.localPosition = new Vector3(0, carControllerBSR.FrontLeftWheelCollider.transform.localPosition.y, carControllerBSR.FrontLeftWheelCollider.transform.localPosition.z);
		
	}
	
	private void  FixedUpdate (){

		if(!carControllerBSR.canControl)
			return;

		NavigationBSR();
		FixedRaycastsBSR();
		ApplyTorquesBSR();
		ResettingBSR();

	}
	
	private void NavigationBSR (){
		
		if(!waypointsContainerBSR){
			Debug.LogError("Waypoints Container Couldn't Found!");
			enabled = false;
			return;
		}
		if(_AITypeBSR == AIType.FollowWaypoints && waypointsContainerBSR && waypointsContainerBSR.waypointsBSR.Count < 1){
			Debug.LogError("Waypoints Container Doesn't Have Any Waypoints!");
			enabled = false;
			return;
		}
		
		// Next waypoint's position.
		Vector3 nextWaypointPosition = transform.InverseTransformPoint( new Vector3(waypointsContainerBSR.waypointsBSR[currentWaypointBSR].position.x, transform.position.y, waypointsContainerBSR.waypointsBSR[currentWaypointBSR].position.z));
		float navigatorInput = Mathf.Clamp(transform.InverseTransformDirection(navigatorBSR.desiredVelocity).x * 1.5f, -1f, 1f);

		if (_AITypeBSR == AIType.FollowWaypoints) {
			if(navigatorBSR.isOnNavMesh)
				navigatorBSR.SetDestination (waypointsContainerBSR.waypointsBSR [currentWaypointBSR].position);
		} else {
			if(navigatorBSR.isOnNavMesh)
				navigatorBSR.SetDestination (waypointsContainerBSR.targetBSR.position);
		}
		//Steering Input.
		if(carControllerBSR.direction == 1){
			if(!ignoreWaypointNowBSR)
				steerInputBSR = Mathf.Clamp((navigatorInput + rayInputBSR), -1f, 1f);
			else
				steerInputBSR = Mathf.Clamp(rayInputBSR, -1f, 1f);
		}else{
			steerInputBSR = Mathf.Clamp((-navigatorInput - rayInputBSR), -1f, 1f);
		}
		
		if(!inBrakeZoneBSR){
			if(carControllerBSR.speed >= 25){
				brakeInputBSR = Mathf.Lerp(0f, .85f, (Mathf.Abs(steerInputBSR)));
			}else{
				brakeInputBSR = 0f;
			}
		}else{
			brakeInputBSR = Mathf.Lerp(0f, 1f, (carControllerBSR.speed - maximumSpeedInBrakeZoneBSR) / maximumSpeedInBrakeZoneBSR);
		}

		if(!inBrakeZoneBSR){
			
			if(carControllerBSR.speed >= 10){
				if(!carControllerBSR.changingGear)
					gasInputBSR = Mathf.Clamp(1f - (Mathf.Abs(navigatorInput / 10f)  - Mathf.Abs(rayInputBSR / 10f)), .75f, 1f);
				else
					gasInputBSR = 0f;
			}else{
				if(!carControllerBSR.changingGear)
					gasInputBSR = 1f;
				else
					gasInputBSR = 0f;
			}

		}else{
			
			if(!carControllerBSR.changingGear)
				gasInputBSR = Mathf.Lerp(1f, 0f, (carControllerBSR.speed) / maximumSpeedInBrakeZoneBSR);
			else
				gasInputBSR = 0f;

		}

		if (_AITypeBSR == AIType.FollowWaypoints) {
		
			// Checks for the distance to next waypoint. If it is less than written value, then pass to next waypoint.
			if (nextWaypointPosition.magnitude < nextWaypointPassRadiusBSR) {
				
				currentWaypointBSR++;
				totalWaypointPassedBSR++;
			
				// If all waypoints are passed, sets the current waypoint to first waypoint and increase lap.
				if (currentWaypointBSR >= waypointsContainerBSR.waypointsBSR.Count) {
					currentWaypointBSR = 0;
					lapBSR++;
				}

			}

		}
		
	}
	
	private void ResettingBSR (){
		
		if(carControllerBSR.speed <= 5 && transform.InverseTransformDirection(rigidBSR.velocity).z < 1f)
			resetTimeBSR += Time.deltaTime;
		
		if(resetTimeBSR >= 2)
			carControllerBSR.direction = -1;

		if(resetTimeBSR >= 4 || carControllerBSR.speed >= 25){
			carControllerBSR.direction = 1;
			resetTimeBSR = 0;
		}
		
	}
	
	private void FixedRaycastsBSR(){
		
		Vector3 forward = transform.TransformDirection ( new Vector3(0, 0, 1));
		Vector3 pivotPos = new Vector3(transform.localPosition.x, carControllerBSR.FrontLeftWheelCollider.transform.position.y, transform.localPosition.z);
		RaycastHit hit;
		
		// New bools effected by fixed raycasts.
		bool  tightTurn = false;
		bool  wideTurn = false;
		bool  sideTurn = false;
		bool  tightTurn1 = false;
		bool  wideTurn1 = false;
		bool  sideTurn1 = false;
		
		// New input steers effected by fixed raycasts.
		float newinputSteer1 = 0f;
		float newinputSteer2 = 0f;
		float newinputSteer3 = 0f;
		float newinputSteer4 = 0f;
		float newinputSteer5 = 0f;
		float newinputSteer6 = 0f;
		
		// Drawing Rays.
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(25, transform.up) * forward * wideRayLengthBSR, Color.white);
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-25, transform.up) * forward * wideRayLengthBSR, Color.white);
		
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(7, transform.up) * forward * tightRayLengthBSR, Color.white);
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-7, transform.up) * forward * tightRayLengthBSR, Color.white);

		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(90, transform.up) * forward * sideRayLengthBSR, Color.white);
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-90, transform.up) * forward * sideRayLengthBSR, Color.white);
		
		// Wide Raycasts.
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(25, transform.up) * forward, out hit, wideRayLengthBSR, obstacleLayersBSR) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(25, transform.up) * forward * wideRayLengthBSR, Color.red);
			newinputSteer1 = Mathf.Lerp (-.5f, 0f, (hit.distance / wideRayLengthBSR));
			wideTurn = true;
		}
		
		else{
			newinputSteer1 = 0f;
			wideTurn = false;
		}
		
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(-25, transform.up) * forward, out hit, wideRayLengthBSR, obstacleLayersBSR) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-25, transform.up) * forward * wideRayLengthBSR, Color.red);
			newinputSteer4 = Mathf.Lerp (.5f, 0f, (hit.distance / wideRayLengthBSR));
			wideTurn1 = true;
		}else{
			newinputSteer4 = 0f;
			wideTurn1 = false;
		}
		
		// Tight Raycasts.
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(7, transform.up) * forward, out hit, tightRayLengthBSR, obstacleLayersBSR) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(7, transform.up) * forward * tightRayLengthBSR , Color.red);
			newinputSteer3 = Mathf.Lerp (-1f, 0f, (hit.distance / tightRayLengthBSR));
			tightTurn = true;
		}else{
			newinputSteer3 = 0f;
			tightTurn = false;
		}
		
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(-7, transform.up) * forward, out hit, tightRayLengthBSR, obstacleLayersBSR) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-7, transform.up) * forward * tightRayLengthBSR, Color.red);
			newinputSteer2 = Mathf.Lerp (1f, 0f, (hit.distance / tightRayLengthBSR));
			tightTurn1 = true;
		}else{
			newinputSteer2 = 0f;
			tightTurn1 = false;
		}

		// Side Raycasts.
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(90, transform.up) * forward, out hit, sideRayLengthBSR, obstacleLayersBSR) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(90, transform.up) * forward * sideRayLengthBSR , Color.red);
			newinputSteer5 = Mathf.Lerp (-1f, 0f, (hit.distance / sideRayLengthBSR));
			sideTurn = true;
		}else{
			newinputSteer5 = 0f;
			sideTurn = false;
		}
		
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(-90, transform.up) * forward, out hit, sideRayLengthBSR, obstacleLayersBSR) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-90, transform.up) * forward * sideRayLengthBSR, Color.red);
			newinputSteer6 = Mathf.Lerp (1f, 0f, (hit.distance / sideRayLengthBSR));
			sideTurn1 = true;
		}else{
			newinputSteer6 = 0f;
			sideTurn1 = false;
		}
		
		if(wideTurn || wideTurn1 || tightTurn || tightTurn1 || sideTurn || sideTurn1)
			raycastingBSR = true;
		else
			raycastingBSR = false;
		
		if(raycastingBSR)
			rayInputBSR = (newinputSteer1 + newinputSteer2 + newinputSteer3 + newinputSteer4 + newinputSteer5 + newinputSteer6);
		else
			rayInputBSR = 0f;
		
		if(raycastingBSR && Mathf.Abs(rayInputBSR) > .5f)
			ignoreWaypointNowBSR = true;
		else
			ignoreWaypointNowBSR = false;
		
	}

	private void ApplyTorquesBSR(){

		if(carControllerBSR.direction == 1){
			if(!limitSpeedBSR){
				carControllerBSR.gasInput = gasInputBSR;
			}else{
				carControllerBSR.gasInput = gasInputBSR * Mathf.Clamp01(Mathf.Lerp(10f, 0f, (carControllerBSR.speed) / maximumSpeedBSR));
			}
		}else{
			carControllerBSR.gasInput = 0f;
		}

		if(smoothedSteerBSR)
			carControllerBSR.steerInput = Mathf.Lerp(carControllerBSR.steerInput, steerInputBSR, Time.deltaTime * 20f);
		else
			carControllerBSR.steerInput = steerInputBSR;

		if(carControllerBSR.direction == 1)
			carControllerBSR.brakeInput = brakeInputBSR;
		else
			carControllerBSR.brakeInput = gasInputBSR;

	}
	
	private void OnTriggerEnter (Collider col){
		
		if(col.gameObject.GetComponent<RCC_AIBrakeZoneBSR>()){
			inBrakeZoneBSR = true;
			maximumSpeedInBrakeZoneBSR = col.gameObject.GetComponent<RCC_AIBrakeZoneBSR>().targetSpeedBSR;
		}
		
	}
	
	private void OnTriggerExit (Collider col){
		
		if(col.gameObject.GetComponent<RCC_AIBrakeZoneBSR>()){
			inBrakeZoneBSR = false;
			maximumSpeedInBrakeZoneBSR = 0;
		}
		
	}
	
}