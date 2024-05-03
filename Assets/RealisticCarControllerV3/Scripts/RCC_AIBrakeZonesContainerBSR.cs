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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/Brake Zones Container")]
public class RCC_AIBrakeZonesContainerBSR : MonoBehaviour {
	
	[FormerlySerializedAs("brakeZones")] public List<Transform> brakeZonesBSR = new List<Transform>();
	
	private void OnDrawGizmos() {
		
		for(int i = 0; i < brakeZonesBSR.Count; i ++){

			Gizmos.matrix = brakeZonesBSR[i].transform.localToWorldMatrix;
			Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.25f);
			Vector3 colliderBounds = brakeZonesBSR[i].GetComponent<BoxCollider>().size;

			Gizmos.DrawCube(Vector3.zero, colliderBounds);

		}
		
	}
	
}
