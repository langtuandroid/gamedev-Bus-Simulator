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

[CustomEditor(typeof(RCC_AIBrakeZonesContainerBSR))]
public class RCC_AIBZEditor : Editor {
	
	RCC_AIBrakeZonesContainerBSR bzScript;
	
	public override void  OnInspectorGUI () {
		
		serializedObject.Update();
		
		bzScript = (RCC_AIBrakeZonesContainerBSR)target;

		if(GUILayout.Button("Delete Brake Zones")){
			foreach(Transform t in bzScript.brakeZonesBSR){
				DestroyImmediate(t.gameObject);
			}
			bzScript.brakeZonesBSR.Clear();
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeZones"), new GUIContent("Brake Zones", "Brake Zones"), true);

		EditorGUILayout.HelpBox("Create BrakeZones By Shift + Left Mouse Button On Your Road", MessageType.Info);

		serializedObject.ApplyModifiedProperties();
		
	}

	void OnSceneGUI (){

		Event e = Event.current;
		bzScript = (RCC_AIBrakeZonesContainerBSR)target;

		if(e != null){

			if(e.isMouse && e.shift && e.type == EventType.MouseDown){

				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(ray, out hit, 5000.0f)) {

					Vector3 newTilePosition = hit.point;

					GameObject wp = new GameObject("Brake Zone " + bzScript.brakeZonesBSR.Count.ToString());

					wp.transform.position = newTilePosition;
					wp.AddComponent<RCC_AIBrakeZoneBSR>();
					wp.AddComponent<BoxCollider>();
					wp.GetComponent<BoxCollider>().isTrigger = true;
					wp.GetComponent<BoxCollider>().size = new Vector3(25, 10, 50);
					wp.transform.SetParent(bzScript.transform);
					GetBrakeZones();
					Event.current.Use();

				}

			}

			if(bzScript)
				Selection.activeGameObject = bzScript.gameObject;

		}

		GetBrakeZones();

	}
	
	public void GetBrakeZones(){
		
		bzScript.brakeZonesBSR = new List<Transform>();
		
		Transform[] allTransforms = bzScript.transform.GetComponentsInChildren<Transform>();
		
		foreach(Transform t in allTransforms){
			
			if(t != bzScript.transform)
				bzScript.brakeZonesBSR.Add(t);
			
		}
		
	}
	
}
