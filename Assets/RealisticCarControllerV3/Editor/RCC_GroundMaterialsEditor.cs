//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RCC_GroundMaterialsBSR))]
public class RCC_GroundMaterialsEditor : Editor {

	RCC_GroundMaterialsBSR prop;

	Vector2 scrollPos;
	List<RCC_GroundMaterialsBSR.GroundMaterialFrictionsBSR> groundMaterials = new List<RCC_GroundMaterialsBSR.GroundMaterialFrictionsBSR>();

	Color orgColor;

	public override void OnInspectorGUI (){

		serializedObject.Update();
		prop = (RCC_GroundMaterialsBSR)target;
		orgColor = GUI.color;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Wheels Editor", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("This editor will keep update necessary .asset files in your project. Don't change directory of the ''Resources/RCCAssets''.", EditorStyles.helpBox);
		EditorGUILayout.Space();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false );

		EditorGUIUtility.labelWidth = 110f;
		//EditorGUIUtility.fieldWidth = 10f;

		GUILayout.Label("Ground Materials", EditorStyles.boldLabel);

		for (int i = 0; i < prop.frictionsBSR.Length; i++) {

			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.Space();

			if(prop.frictionsBSR[i].groundMaterialBSR)
				EditorGUILayout.LabelField(prop.frictionsBSR[i].groundMaterialBSR.name + (i == 0 ? " (Default)" : ""), EditorStyles.boldLabel);

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();

			prop.frictionsBSR[i].groundMaterialBSR = (PhysicMaterial)EditorGUILayout.ObjectField("Physic Material", prop.frictionsBSR[i].groundMaterialBSR, typeof(PhysicMaterial), false, GUILayout.Width(250f));
			prop.frictionsBSR[i].forwardStiffnessBSR = EditorGUILayout.FloatField("Forward Stiffness", prop.frictionsBSR[i].forwardStiffnessBSR, GUILayout.Width(250f));

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			prop.frictionsBSR[i].groundSoundBSR = (AudioClip)EditorGUILayout.ObjectField("Wheel Sound", prop.frictionsBSR[i].groundSoundBSR, typeof(AudioClip), false, GUILayout.Width(250f));
			prop.frictionsBSR[i].sidewaysStiffnessBSR = EditorGUILayout.FloatField("Sideways Stiffness", prop.frictionsBSR[i].sidewaysStiffnessBSR, GUILayout.Width(250f));

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			prop.frictionsBSR[i].groundParticlesBSR = (GameObject)EditorGUILayout.ObjectField("Wheel Particles", prop.frictionsBSR[i].groundParticlesBSR, typeof(GameObject), false, GUILayout.Width(250f));
			prop.frictionsBSR[i].slipBSR = EditorGUILayout.FloatField("Slip", prop.frictionsBSR[i].slipBSR, GUILayout.Width(250f));

			EditorGUILayout.Space();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			prop.frictionsBSR[i].dampBSR = EditorGUILayout.FloatField("Damp", prop.frictionsBSR[i].dampBSR, GUILayout.Width(250f));
			GUI.color = Color.red;		if(GUILayout.Button("Remove", GUILayout.Width(75f))){RemoveGroundMaterial(i);}	GUI.color = orgColor;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();

		}

		EditorGUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label("Terrain Ground Materials", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("useTerrainSplatMapForGroundFrictions"), new GUIContent("Use Terrain SplatMap For Ground Physics"), false);
		if(prop.useTerrainSplatMapForGroundFrictionsBSR){
			EditorGUILayout.PropertyField(serializedObject.FindProperty("terrainPhysicMaterial"), new GUIContent("Terrain Physic Material"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("terrainSplatMapIndex"), new GUIContent("Terrain Splat Map Index"), true);
		}
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();

		GUI.color = Color.cyan;

		if(GUILayout.Button("Create New Ground Material")){

			AddNewWheel();

		}

		if(GUILayout.Button("--< Return To Asset Settings")){

			OpenGeneralSettings();

		}

		GUI.color = orgColor;

		EditorGUILayout.EndScrollView();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Created by Buğra Özdoğanlar\nBoneCrackerGames", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		serializedObject.ApplyModifiedProperties();

		if(GUI.changed)
			EditorUtility.SetDirty(prop);

	}

	void AddNewWheel(){

		groundMaterials.Clear();
		groundMaterials.AddRange(prop.frictionsBSR);
		RCC_GroundMaterialsBSR.GroundMaterialFrictionsBSR newGroundMaterial = new RCC_GroundMaterialsBSR.GroundMaterialFrictionsBSR();
		groundMaterials.Add(newGroundMaterial);
		prop.frictionsBSR = groundMaterials.ToArray();

	}

	void RemoveGroundMaterial(int index){

		groundMaterials.Clear();
		groundMaterials.AddRange(prop.frictionsBSR);
		groundMaterials.RemoveAt(index);
		prop.frictionsBSR = groundMaterials.ToArray();

	}

	void OpenGeneralSettings(){

		Selection.activeObject =RCC_SettingsBSR.InstanceBSR;

	}

}
