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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Exhaust")]
public class RCC_ExhaustBSR : MonoBehaviour {

	private RCC_CarControllerV3 carControllerBSR;
	private ParticleSystem particleBSR;
	private ParticleSystem.EmissionModule emissionBSR;
	private ParticleSystem.MinMaxCurve emissionRateBSR;
	[FormerlySerializedAs("flame")] public ParticleSystem flameBSR;
	private ParticleSystem.EmissionModule subEmissionBSR;
	private ParticleSystem.MinMaxCurve subEmissionRateBSR;
	private Light flameLightBSR;

	[FormerlySerializedAs("flameTime")] public float flameTimeBSR = 0f;
	private AudioSource flameSourceBSR;

	[FormerlySerializedAs("flameColor")] public Color flameColorBSR = Color.red;
	[FormerlySerializedAs("boostFlameColor")] public Color boostFlameColorBSR = Color.blue;

	private void Start () {

		if (RCC_SettingsBSR.InstanceBSR.dontUseAnyParticleEffectsBSR) {
			Destroy (gameObject);
			return;
		}

		carControllerBSR = GetComponentInParent<RCC_CarControllerV3>();
		particleBSR = GetComponent<ParticleSystem>();
		emissionBSR = particleBSR.emission;

		if(flameBSR){
			
			subEmissionBSR = flameBSR.emission;
			flameLightBSR = flameBSR.GetComponentInChildren<Light>();
			flameSourceBSR = RCC_CreateAudioSourceBSR.NewAudioSourceBSR(gameObject, "Exhaust Flame AudioSource", 10f, 50f, 10f, RCC_SettingsBSR.InstanceBSR.exhaustFlameClipsBSR[0], false, false, false);
			flameLightBSR.renderMode = RCC_SettingsBSR.InstanceBSR.useLightsAsVertexLightsBSR ? LightRenderMode.ForceVertex : LightRenderMode.ForcePixel;

		}
	
	}

	private void Update () {

		if(!carControllerBSR || !particleBSR)
			return;

		if(carControllerBSR.engineRunning){

			if(carControllerBSR.speed < 150){
				if(!emissionBSR.enabled)
					emissionBSR.enabled = true;
			if(carControllerBSR._gasInput > .05f){
				emissionRateBSR.constantMax = 50f;
				emissionBSR.rate = emissionRateBSR;
				particleBSR.startSpeed = 5f;
				particleBSR.startSize = 8;
			}else{
				emissionRateBSR.constantMax = 5;
				emissionBSR.rate = emissionRateBSR;
				particleBSR.startSpeed = .5f;
				particleBSR.startSize = 4;
				}
			}else{
				if(emissionBSR.enabled)
					emissionBSR.enabled = false;
			}

			if(carControllerBSR._gasInput >= .25f)
				flameTimeBSR = 0f;

			if((carControllerBSR.useExhaustFlame && carControllerBSR.engineRPM >= 5000 && carControllerBSR.engineRPM <= 5500 && carControllerBSR._gasInput <= .25f && flameTimeBSR <= .5f) || carControllerBSR._boostInput >= 1.5f){
				
				flameTimeBSR += Time.deltaTime;
				subEmissionBSR.enabled = true;

				if(flameLightBSR)
					flameLightBSR.intensity = flameSourceBSR.pitch * 3f * Random.Range(.25f, 1f) ;
				
				if(carControllerBSR._boostInput >= 1.5f && flameBSR){
					flameBSR.startColor = boostFlameColorBSR;
					flameLightBSR.color = flameBSR.startColor;
				}else{
					flameBSR.startColor = flameColorBSR;
					flameLightBSR.color = flameBSR.startColor;
				}

				if(!flameSourceBSR.isPlaying){
					flameSourceBSR.clip = RCC_SettingsBSR.InstanceBSR.exhaustFlameClipsBSR[Random.Range(0, RCC_SettingsBSR.InstanceBSR.exhaustFlameClipsBSR.Length)];
					flameSourceBSR.Play();
				}

			}else{
				
				subEmissionBSR.enabled = false;

				if(flameLightBSR)
					flameLightBSR.intensity = 0f;
				if(flameSourceBSR.isPlaying)
					flameSourceBSR.Stop();
				
			}
				
		}else{

			if(emissionBSR.enabled)
				emissionBSR.enabled = false;

			subEmissionBSR.enabled = false;

			if(flameLightBSR)
				flameLightBSR.intensity = 0f;
			if(flameSourceBSR.isPlaying)
				flameSourceBSR.Stop();
			
		}

	}

}
