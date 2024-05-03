//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RCC_GroundMaterialsBSR))]
public class RCC_PhysicMaterialsEditor : Editor {

	RCC_GroundMaterialsBSR physicMats;
	Color originalGUIColor;
	string[] physicMatsNames;
	Vector2 scrollPos;

	public override void OnInspectorGUI () {

		serializedObject.Update();

		physicMats = (RCC_GroundMaterialsBSR)target;

		originalGUIColor = GUI.backgroundColor;

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false );


		/////////////////////////


		GUILayout.Label("Ground Physic Materials", EditorStyles.boldLabel);

		EditorGUI.indentLevel++;

		EditorGUILayout.BeginVertical(GUI.skin.box);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("frictions"), new GUIContent("Ground Physic Materials"), true);

		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical(GUI.skin.box);

		for (int i = 0; i < physicMats.frictionsBSR.Length; i++) {

			EditorGUILayout.BeginVertical(GUI.skin.box);

			if(physicMats.frictionsBSR[i].groundMaterialBSR != null){
				GUILayout.Label(physicMats.frictionsBSR[i].groundMaterialBSR.name, EditorStyles.boldLabel);
				EditorGUILayout.Space(); 
				physicMats.frictionsBSR[i].groundMaterialBSR.staticFriction = physicMats.frictionsBSR[i].groundMaterialBSR.dynamicFriction = EditorGUILayout.FloatField("Forward And Sideways Stiffness", physicMats.frictionsBSR[i].groundMaterialBSR.staticFriction);
				physicMats.frictionsBSR[i].groundParticlesBSR = (GameObject)EditorGUILayout.ObjectField("Wheel Particles", physicMats.frictionsBSR[i].groundParticlesBSR, typeof(GameObject), false);
				physicMats.frictionsBSR[i].groundSoundBSR = (AudioClip)EditorGUILayout.ObjectField("Wheel Sound", physicMats.frictionsBSR[i].groundSoundBSR, typeof(AudioClip), false);
			}else{
				GUI.color = Color.red;
				GUILayout.Label("Null. Select One Material!", EditorStyles.boldLabel);
				GUI.color  = originalGUIColor;
			}
			 
			EditorGUILayout.EndVertical();
			
		}

		EditorGUILayout.EndVertical();

		EditorGUILayout.EndScrollView();

		GUI.color = new Color(.5f, 1f, 1f, 1f);

		if(GUILayout.Button(" <-- Return To RCC Settings")){
			Selection.activeObject = Resources.Load("RCCAssets/RCCAssetSettings") as RCC_SettingsBSR;
		}

		GUI.color = originalGUIColor;


		/////////////////////////


		serializedObject.ApplyModifiedProperties();

		if(GUI.changed)
			EditorUtility.SetDirty(physicMats);
	
	}

}
