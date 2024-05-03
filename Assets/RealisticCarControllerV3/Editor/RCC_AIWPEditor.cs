//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RCC_AIWaypointsContainerBSR))]
public class RCC_AIWPEditor : Editor {
	
	RCC_AIWaypointsContainerBSR wpScript;
	
	public override void  OnInspectorGUI () {
		
		serializedObject.Update();
		
		wpScript = (RCC_AIWaypointsContainerBSR)target;
			
		if (GUILayout.Button ("Delete Waypoints")) {
			foreach (Transform t in wpScript.waypointsBSR) {
				DestroyImmediate (t.gameObject);
			}
			wpScript.waypointsBSR.Clear ();
		}
			
		EditorGUILayout.PropertyField(serializedObject.FindProperty("target"), new GUIContent("Target", "Target"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("waypoints"), new GUIContent("Waypoints", "Waypoints"), true);

		EditorGUILayout.HelpBox("Create Waypoints By Shift + Left Mouse Button On Your Road", MessageType.Info);

		serializedObject.ApplyModifiedProperties();
		
	}

	void OnSceneGUI(){

		Event e = Event.current;
		wpScript = (RCC_AIWaypointsContainerBSR)target;

		if(e != null){

			if(e.isMouse && e.shift && e.type == EventType.MouseDown){

				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(ray, out hit, 5000.0f)) {

					Vector3 newTilePosition = hit.point;

					GameObject wp = new GameObject("Waypoint " + wpScript.waypointsBSR.Count.ToString());

					wp.transform.position = newTilePosition;
					wp.transform.SetParent(wpScript.transform);

					GetWaypoints();

				}

			}

			if(wpScript)
				Selection.activeGameObject = wpScript.gameObject;

		}

		GetWaypoints();

	}
	
	public void GetWaypoints(){
		
		wpScript.waypointsBSR = new List<Transform>();
		
		Transform[] allTransforms = wpScript.transform.GetComponentsInChildren<Transform>();
		
		foreach(Transform t in allTransforms){
			
			if(t != wpScript.transform)
				wpScript.waypointsBSR.Add(t);
			
		}
		
	}
	
}
