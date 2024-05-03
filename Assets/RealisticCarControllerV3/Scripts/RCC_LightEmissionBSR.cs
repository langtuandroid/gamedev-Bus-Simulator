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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/Light Emission")]
public class RCC_LightEmissionBSR : MonoBehaviour {

	private Light sharedLightBSR;
	[FormerlySerializedAs("lightRenderer")] public Renderer lightRendererBSR;
	[FormerlySerializedAs("materialIndex")] public int materialIndexBSR = 0;
	[FormerlySerializedAs("noTexture")] public bool noTextureBSR = false;

	private void Start () {

		sharedLightBSR = GetComponent<Light>();
		Material m = lightRendererBSR.materials[materialIndexBSR];
		m.EnableKeyword("_EMISSION");
	 
	}

	private void Update () {

		if(!sharedLightBSR.enabled){
			lightRendererBSR.materials[materialIndexBSR].SetColor("_EmissionColor", Color.white * 0f);
			return;
		}

		if(!noTextureBSR)
			lightRendererBSR.materials[materialIndexBSR].SetColor("_EmissionColor", Color.white * sharedLightBSR.intensity);
		else
			lightRendererBSR.materials[materialIndexBSR].SetColor("_EmissionColor", Color.red * sharedLightBSR.intensity);
	
	}

}
