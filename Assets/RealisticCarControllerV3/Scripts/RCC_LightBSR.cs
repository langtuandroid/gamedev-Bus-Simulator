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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/Light")]
public class RCC_LightBSR : MonoBehaviour {

	private RCC_CarControllerV3 carControllerBSR;
	private Light _lightBSR;
	private Projector projectorBSR;

	[FormerlySerializedAs("lightType")] public LightType lightTypeBSR;
	public enum LightType{HeadLight, BrakeLight, ReverseLight, Indicator};
	[FormerlySerializedAs("inertia")] public float inertiaBSR = 1f;

	// For Indicators.
	private RCC_CarControllerV3.IndicatorsOn indicatorsOnBSR;
	private AudioSource indicatorSoundBSR;
	public AudioClip indicatorClipBSR{get{return RCC_SettingsBSR.InstanceBSR.indicatorClipBSR;}}

	private void Start () {
		
		carControllerBSR = GetComponentInParent<RCC_CarControllerV3>();
		_lightBSR = GetComponent<Light>();
		_lightBSR.enabled = true;

		if(RCC_SettingsBSR.InstanceBSR.useLightProjectorForLightingEffectBSR){
			
			projectorBSR = GetComponent<Projector>();
			if(projectorBSR == null){
				projectorBSR = ((GameObject)Instantiate(RCC_SettingsBSR.InstanceBSR.projectorBSR, transform.position, transform.rotation)).GetComponent<Projector>();
				projectorBSR.transform.SetParent(transform, true);
			}
			projectorBSR.ignoreLayers = RCC_SettingsBSR.InstanceBSR.projectorIgnoreLayerBSR;
			if(lightTypeBSR != LightType.HeadLight)
				projectorBSR.transform.localRotation = Quaternion.Euler(20f, transform.localPosition.z > 0f ? 0f : 180f, 0f);
			Material newMaterial = new Material(projectorBSR.material);
			projectorBSR.material = newMaterial ;

		}

		if(RCC_SettingsBSR.InstanceBSR.useLightsAsVertexLightsBSR){
			_lightBSR.renderMode = LightRenderMode.ForceVertex;
			_lightBSR.cullingMask = 0;
		}else{
			_lightBSR.renderMode = LightRenderMode.ForcePixel;
		}

		if(lightTypeBSR == LightType.Indicator){
			
			if(!carControllerBSR.transform.Find("All Audio Sources/Indicator Sound AudioSource"))
				indicatorSoundBSR = RCC_CreateAudioSourceBSR.NewAudioSourceBSR(carControllerBSR.gameObject, "Indicator Sound AudioSource", 3, 10, 1, indicatorClipBSR, false, false, false);
			else
				indicatorSoundBSR = carControllerBSR.transform.Find("All Audio Sources/Indicator Sound AudioSource").GetComponent<AudioSource>();
			
		}

	}

	private void Update () {

		if(RCC_SettingsBSR.InstanceBSR.useLightProjectorForLightingEffectBSR)
			ProjectorsBSR();

		switch(lightTypeBSR){

		case LightType.HeadLight:
			if(!carControllerBSR.lowBeamHeadLightsOn && !carControllerBSR.highBeamHeadLightsOn)
				LightingBSR(0f);
			if(carControllerBSR.lowBeamHeadLightsOn && !carControllerBSR.highBeamHeadLightsOn){
				LightingBSR(.6f, 50f, 90f);
				transform.localEulerAngles = new Vector3(10f, 0f, 0f);
			}else if(carControllerBSR.highBeamHeadLightsOn){
				LightingBSR(1f, 200f, 45f);
				transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			break;

		case LightType.BrakeLight:
			LightingBSR((!carControllerBSR.lowBeamHeadLightsOn ? (carControllerBSR._brakeInput >= .1f ? 1f : 0f)  : (carControllerBSR._brakeInput >= .1f ? 1f : .3f)));
			break;

		case LightType.ReverseLight:
			LightingBSR(carControllerBSR.direction == -1 ? 1f : 0f);
			break;

		case LightType.Indicator:
			indicatorsOnBSR = carControllerBSR.indicatorsOn;
			IndicatorsBSR();
			break;

		}
		
	}

	private void LightingBSR(float input){

		_lightBSR.intensity = Mathf.Lerp(_lightBSR.intensity, input, Time.deltaTime * inertiaBSR * 20f);

	}

	private void LightingBSR(float input, float range, float spotAngle){

		_lightBSR.intensity = Mathf.Lerp(_lightBSR.intensity, input, Time.deltaTime * inertiaBSR * 20f);
		_lightBSR.range = range;
		_lightBSR.spotAngle = spotAngle;

	}

	private void IndicatorsBSR(){

		switch(indicatorsOnBSR){

		case RCC_CarControllerV3.IndicatorsOn.Left:

			if(transform.localPosition.x > 0f){
				LightingBSR (0);
				break;
			}

			if(carControllerBSR.indicatorTimer >= .5f){
				LightingBSR (0);
				if(indicatorSoundBSR.isPlaying)
					indicatorSoundBSR.Stop();
			}else{
				LightingBSR (1);
				if(!indicatorSoundBSR.isPlaying && carControllerBSR.indicatorTimer <= .05f)
					indicatorSoundBSR.Play();
			}
			if(carControllerBSR.indicatorTimer >= 1f)
				carControllerBSR.indicatorTimer = 0f;
			break;

		case RCC_CarControllerV3.IndicatorsOn.Right:

			if(transform.localPosition.x < 0f){
				LightingBSR (0);
				break;
			}

			if(carControllerBSR.indicatorTimer >= .5f){
				LightingBSR (0);
			if(indicatorSoundBSR.isPlaying)
				indicatorSoundBSR.Stop();
			}else{
				LightingBSR (1);
				if(!indicatorSoundBSR.isPlaying && carControllerBSR.indicatorTimer <= .05f)
					indicatorSoundBSR.Play();
			}
			if(carControllerBSR.indicatorTimer >= 1f)
				carControllerBSR.indicatorTimer = 0f;
			break;

		case RCC_CarControllerV3.IndicatorsOn.All:
			
			if(carControllerBSR.indicatorTimer >= .5f){
				LightingBSR (0);
				if(indicatorSoundBSR.isPlaying)
					indicatorSoundBSR.Stop();
			}else{
				LightingBSR (1);
				if(!indicatorSoundBSR.isPlaying && carControllerBSR.indicatorTimer <= .05f)
					indicatorSoundBSR.Play();
			}
			if(carControllerBSR.indicatorTimer >= 1f)
				carControllerBSR.indicatorTimer = 0f;
			break;

		case RCC_CarControllerV3.IndicatorsOn.Off:
			
			LightingBSR (0);
			carControllerBSR.indicatorTimer = 0f;
			break;
			
		}

	}

	private void ProjectorsBSR(){

		if(!_lightBSR.enabled){
			projectorBSR.enabled = false;
			return;
		}else{
			projectorBSR.enabled = true;
		}

		projectorBSR.material.color = _lightBSR.color * (_lightBSR.intensity / 2f);

		projectorBSR.farClipPlane = Mathf.Lerp(10f, 40f, (_lightBSR.range - 50) / 150f);
		projectorBSR.fieldOfView = Mathf.Lerp(40f, 30f, (_lightBSR.range - 50) / 150f);

	}
		
}
