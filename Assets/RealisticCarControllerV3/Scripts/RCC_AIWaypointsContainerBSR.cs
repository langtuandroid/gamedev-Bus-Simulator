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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/Waypoints Container")]
public class RCC_AIWaypointsContainerBSR : MonoBehaviour {

	[FormerlySerializedAs("waypoints")] public List<Transform> waypointsBSR = new List<Transform>();
	[FormerlySerializedAs("target")] public Transform targetBSR;
	
	private void OnDrawGizmos() {
		
		for(int i = 0; i < waypointsBSR.Count; i ++){
			
			Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.3f);
			Gizmos.DrawSphere (waypointsBSR[i].transform.position, 2);
			Gizmos.DrawWireSphere (waypointsBSR[i].transform.position, 20f);
			
			if(i < waypointsBSR.Count - 1){
				if(waypointsBSR[i] && waypointsBSR[i+1]){
					if (waypointsBSR.Count > 0) {
						Gizmos.color = Color.green;
						if(i < waypointsBSR.Count - 1)
							Gizmos.DrawLine(waypointsBSR[i].position, waypointsBSR[i+1].position); 
						if(i < waypointsBSR.Count - 2)
							Gizmos.DrawLine(waypointsBSR[waypointsBSR.Count - 1].position, waypointsBSR[0].position); 
					}
				}
			}
		}
		
	}
	
}
