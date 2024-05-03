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

[System.Serializable]
public class RCC_GroundMaterialsBSR : ScriptableObject {
	
	#region singleton
	public static RCC_GroundMaterialsBSR instanceBSR;
	public static RCC_GroundMaterialsBSR InstanceBsr{	get{if(instanceBSR == null) instanceBSR = Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterialsBSR; return instanceBSR;}}
	#endregion

	[System.Serializable]
	public class GroundMaterialFrictionsBSR{
		
		[FormerlySerializedAs("groundMaterial")] public PhysicMaterial groundMaterialBSR;
		[FormerlySerializedAs("forwardStiffness")] public float forwardStiffnessBSR = 1f;
		[FormerlySerializedAs("sidewaysStiffness")] public float sidewaysStiffnessBSR = 1f;
		[FormerlySerializedAs("slip")] public float slipBSR = .25f;
		[FormerlySerializedAs("damp")] public float dampBSR = 1f;
		[FormerlySerializedAs("groundParticles")] public GameObject groundParticlesBSR;
		[FormerlySerializedAs("groundSound")] public AudioClip groundSoundBSR;

	}
		
	[FormerlySerializedAs("frictions")] public GroundMaterialFrictionsBSR[] frictionsBSR;

	[FormerlySerializedAs("useTerrainSplatMapForGroundFrictions")] public bool useTerrainSplatMapForGroundFrictionsBSR = false;
	[FormerlySerializedAs("terrainPhysicMaterial")] public PhysicMaterial terrainPhysicMaterialBSR;
	[FormerlySerializedAs("terrainSplatMapIndex")] public int[] terrainSplatMapIndexBSR;

}


