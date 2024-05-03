//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------


using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Main/Wheel Collider")]
[RequireComponent (typeof(WheelCollider))]
public class RCC_WheelColliderBSR : MonoBehaviour {
	
	private RCC_CarControllerV3 carControllerBSR;
	private Rigidbody rigidBSR;

	private WheelCollider _wheelColliderBSR;
	public WheelCollider wheelColliderBSR{
		get{
			if(_wheelColliderBSR == null)
				_wheelColliderBSR = GetComponent<WheelCollider>();
			return _wheelColliderBSR;
		}set{
			_wheelColliderBSR = value;
		}
	}

	private List <RCC_WheelColliderBSR> allWheelCollidersBSR = new List<RCC_WheelColliderBSR>() ;
	[FormerlySerializedAs("wheelModel")] public Transform wheelModelBSR;

	private float wheelRotationBSR = 0f;
	private float camberBSR = 0f;
	private PhysicMaterial groundMaterialBSR;

	internal float steerAngleBSR = 0f;
	internal bool isGroundedBSR = false;
	internal float totalSlipBSR = 0f;
	internal float rpmBSR = 0f;
	internal float wheelRPMToSpeedBSR = 0f;
	internal float wheelTemparatureBSR = 0f;

	private RCC_GroundMaterialsBSR physicsMaterialsBSR{get{return RCC_GroundMaterialsBSR.InstanceBsr;}}
	private RCC_GroundMaterialsBSR.GroundMaterialFrictionsBSR[] physicsFrictionsBSR{get{return RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR;}}

	private RCC_SkidmarksBSR skidmarksBSR;
	private float startSlipValueBSR = .25f;
	private int lastSkidmarkBSR = -1;
	
	private float wheelSlipAmountSidewaysBSR;
	private float wheelSlipAmountForwardBSR;

	private float orgSidewaysStiffnessBSR = 1f;
	private float orgForwardStiffnessBSR = 1f;

	//WheelFriction Curves and Stiffness.
	[FormerlySerializedAs("forwardFrictionCurve")] public WheelFrictionCurve forwardFrictionCurveBSR;
	[FormerlySerializedAs("sidewaysFrictionCurve")] public WheelFrictionCurve sidewaysFrictionCurveBSR;

	private AudioSource audioSourceBSR;
	private AudioClip audioClipBSR;

	private List<ParticleSystem> allWheelParticlesBSR = new List<ParticleSystem>();
	private ParticleSystem.EmissionModule emissionBSR;

	internal float tractionHelpedSidewaysStiffnessBSR = 1f;

	private float minForwardStiffnessBSR = .75f;
	private float maxForwardStiffnessBSR  = 1f;

	private float minSidewaysStiffnessBSR = .75f;
	private float maxSidewaysStiffnessBSR = 1f;
	
	private void Awake (){

		carControllerBSR = GetComponentInParent<RCC_CarControllerV3>();
		rigidBSR = carControllerBSR.GetComponent<Rigidbody> ();
		wheelColliderBSR = GetComponent<WheelCollider>();

		if (!RCC_SettingsBSR.InstanceBSR.dontUseSkidmarksBSR) {
			if (FindObjectOfType (typeof(RCC_SkidmarksBSR))) {
				skidmarksBSR = FindObjectOfType (typeof(RCC_SkidmarksBSR)) as RCC_SkidmarksBSR;
			} else {
				Debug.Log ("No skidmarks object found. Creating new one...");
				skidmarksBSR = (RCC_SkidmarksBSR)Instantiate (RCC_SettingsBSR.InstanceBSR.skidmarksManagerBSR, Vector3.zero, Quaternion.identity);
			}
		}

		wheelColliderBSR.mass = rigidBSR.mass / 15f;
		forwardFrictionCurveBSR = wheelColliderBSR.forwardFriction;
		sidewaysFrictionCurveBSR = wheelColliderBSR.sidewaysFriction;

		switch(RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR){

		case RCC_SettingsBSR.BehaviorType.SemiArcade:
			forwardFrictionCurveBSR = SetFrictionCurvesBSR(forwardFrictionCurveBSR, .2f, 2f, 2f, 2f);
			sidewaysFrictionCurveBSR = SetFrictionCurvesBSR(sidewaysFrictionCurveBSR, .25f, 2f, 2f, 2f);
			wheelColliderBSR.forceAppPointDistance = Mathf.Clamp(wheelColliderBSR.forceAppPointDistance, .35f, 1f);
			break;

		case RCC_SettingsBSR.BehaviorType.Drift:
			forwardFrictionCurveBSR = SetFrictionCurvesBSR(forwardFrictionCurveBSR, .25f, 1f, .8f, .5f);
			sidewaysFrictionCurveBSR = SetFrictionCurvesBSR(sidewaysFrictionCurveBSR, .4f, 1f, .5f, .75f);
			wheelColliderBSR.forceAppPointDistance = Mathf.Clamp(wheelColliderBSR.forceAppPointDistance, .1f, 1f);
			if(carControllerBSR._wheelTypeChoise == RCC_CarControllerV3.WheelType.FWD){
				Debug.LogError("Current behavior mode is ''Drift'', but your vehicle named " + carControllerBSR.name + " was FWD. You have to use RWD, AWD, or BIASED to rear wheels. Setting it to *RWD* now. ");
				carControllerBSR._wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;
			}
			break;

		case RCC_SettingsBSR.BehaviorType.Fun:
			forwardFrictionCurveBSR = SetFrictionCurvesBSR(forwardFrictionCurveBSR, .2f, 2f, 2f, 2f);
			sidewaysFrictionCurveBSR = SetFrictionCurvesBSR(sidewaysFrictionCurveBSR, .25f, 2f, 2f, 2f);
			wheelColliderBSR.forceAppPointDistance = Mathf.Clamp(wheelColliderBSR.forceAppPointDistance, .75f, 2f);
			break;

		case RCC_SettingsBSR.BehaviorType.Racing:
			forwardFrictionCurveBSR = SetFrictionCurvesBSR(forwardFrictionCurveBSR, .2f, 1f, .8f, .75f);
			sidewaysFrictionCurveBSR = SetFrictionCurvesBSR(sidewaysFrictionCurveBSR, .3f, 1f, .25f, .75f);
			wheelColliderBSR.forceAppPointDistance = Mathf.Clamp(wheelColliderBSR.forceAppPointDistance, .25f, 1f);
			break;

		case RCC_SettingsBSR.BehaviorType.Simulator:
			forwardFrictionCurveBSR = SetFrictionCurvesBSR(forwardFrictionCurveBSR, .2f, 1f, .8f, .75f);
			sidewaysFrictionCurveBSR = SetFrictionCurvesBSR(sidewaysFrictionCurveBSR, .25f, 1f, .5f, .75f);
			wheelColliderBSR.forceAppPointDistance = Mathf.Clamp(wheelColliderBSR.forceAppPointDistance, .1f, 1f);
			break;

		}

		orgForwardStiffnessBSR = forwardFrictionCurveBSR.stiffness;
		orgSidewaysStiffnessBSR = sidewaysFrictionCurveBSR.stiffness;
		wheelColliderBSR.forwardFriction = forwardFrictionCurveBSR;
		wheelColliderBSR.sidewaysFriction = sidewaysFrictionCurveBSR;

		if(RCC_SettingsBSR.InstanceBSR.useSharedAudioSourcesBSR){
			if(!carControllerBSR.transform.Find("All Audio Sources/Skid Sound AudioSource"))
				audioSourceBSR = RCC_CreateAudioSourceBSR.NewAudioSourceBSR(carControllerBSR.gameObject, "Skid Sound AudioSource", 5, 50, 0, audioClipBSR, true, true, false);
			else
				audioSourceBSR = carControllerBSR.transform.Find("All Audio Sources/Skid Sound AudioSource").GetComponent<AudioSource>();
		}else{
			audioSourceBSR = RCC_CreateAudioSourceBSR.NewAudioSourceBSR(carControllerBSR.gameObject, "Skid Sound AudioSource", 5, 50, 0, audioClipBSR, true, true, false);
			audioSourceBSR.transform.position = transform.position;
		}

		if (!RCC_SettingsBSR.InstanceBSR.dontUseAnyParticleEffectsBSR) {

			for (int i = 0; i < RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR.Length; i++) {

				GameObject ps = (GameObject)Instantiate (RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR [i].groundParticlesBSR, transform.position, transform.rotation) as GameObject;
				emissionBSR = ps.GetComponent<ParticleSystem> ().emission;
				emissionBSR.enabled = false;
				ps.transform.SetParent (transform, false);
				ps.transform.localPosition = Vector3.zero;
				ps.transform.localRotation = Quaternion.identity;
				allWheelParticlesBSR.Add (ps.GetComponent<ParticleSystem> ());

			}

		}
			
	}

	private void Start(){

		allWheelCollidersBSR = carControllerBSR.allWheelColliders.ToList();
		allWheelCollidersBSR.Remove(this);

	}

	WheelFrictionCurve SetFrictionCurvesBSR(WheelFrictionCurve curve, float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue){

		WheelFrictionCurve newCurve = curve;

		newCurve.extremumSlip = extremumSlip;
		newCurve.extremumValue = extremumValue;
		newCurve.asymptoteSlip = asymptoteSlip;
		newCurve.asymptoteValue = asymptoteValue;

		return newCurve;

	}

	private void Update(){

		if (!carControllerBSR.enabled)
			return;

		if(!carControllerBSR.sleepingRigid){

			WheelAlignBSR();
			WheelCamberBSR();

		}

	}
	
	private void  FixedUpdate (){

		if (!carControllerBSR.enabled)
			return;

		WheelHit hit;
		isGroundedBSR = wheelColliderBSR.GetGroundHit(out hit);

		steerAngleBSR = wheelColliderBSR.steerAngle;
		rpmBSR = wheelColliderBSR.rpm;
		wheelRPMToSpeedBSR = (((wheelColliderBSR.rpm * wheelColliderBSR.radius) / 2.8f) * Mathf.Lerp(1f, .75f, hit.forwardSlip)) * rigidBSR.transform.lossyScale.y;
		camberBSR = this == carControllerBSR.FrontLeftWheelCollider || this == carControllerBSR.FrontRightWheelCollider ? carControllerBSR.frontCamber : carControllerBSR.rearCamber;

		SkidMarksBSR();
		FrictionsBSR();
		AudioBSR();

	}

	public void WheelAlignBSR (){

		if(!wheelModelBSR){
			Debug.LogError(transform.name + " wheel of the " + carControllerBSR.transform.name + " is missing wheel model. This wheel is disabled");
			enabled = false;
			return;
		}

		RaycastHit hit;
		WheelHit CorrespondingGroundHit;

		Vector3 ColliderCenterPoint = wheelColliderBSR.transform.TransformPoint(wheelColliderBSR.center);
		wheelColliderBSR.GetGroundHit(out CorrespondingGroundHit);

		if(Physics.Raycast(ColliderCenterPoint, -wheelColliderBSR.transform.up, out hit, (wheelColliderBSR.suspensionDistance + wheelColliderBSR.radius) * transform.localScale.y) && !hit.transform.IsChildOf(carControllerBSR.transform) && !hit.collider.isTrigger){
			wheelModelBSR.transform.position = hit.point + (wheelColliderBSR.transform.up * wheelColliderBSR.radius) * transform.localScale.y;
			float extension = (-wheelColliderBSR.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - wheelColliderBSR.radius) / wheelColliderBSR.suspensionDistance;
			Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + wheelColliderBSR.transform.up * (CorrespondingGroundHit.force / rigidBSR.mass), extension <= 0.0 ? Color.magenta : Color.white);
			Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - wheelColliderBSR.transform.forward * CorrespondingGroundHit.forwardSlip * 2f, Color.green);
			Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - wheelColliderBSR.transform.right * CorrespondingGroundHit.sidewaysSlip * 2f, Color.red);
		}else{
			wheelModelBSR.transform.position = Vector3.Lerp(wheelModelBSR.transform.position, ColliderCenterPoint - (wheelColliderBSR.transform.up * wheelColliderBSR.suspensionDistance) * transform.localScale.y, Time.deltaTime * 10f);
		}

		wheelRotationBSR += wheelColliderBSR.rpm * 6 * Time.deltaTime;
		wheelModelBSR.transform.rotation = wheelColliderBSR.transform.rotation * Quaternion.Euler(wheelRotationBSR, wheelColliderBSR.steerAngle, wheelColliderBSR.transform.rotation.z);

	}

	public void WheelCamberBSR (){

		Vector3 wheelLocalEuler;

		if(wheelColliderBSR.transform.localPosition.x < 0)
			wheelLocalEuler = new Vector3(wheelColliderBSR.transform.localEulerAngles.x, wheelColliderBSR.transform.localEulerAngles.y, (-camberBSR));
		else
			wheelLocalEuler = new Vector3(wheelColliderBSR.transform.localEulerAngles.x, wheelColliderBSR.transform.localEulerAngles.y, (camberBSR));

		Quaternion wheelCamber = Quaternion.Euler(wheelLocalEuler);
		wheelColliderBSR.transform.localRotation = wheelCamber;

	}

	private void SkidMarksBSR(){

		WheelHit GroundHit;
		wheelColliderBSR.GetGroundHit(out GroundHit);

		wheelSlipAmountSidewaysBSR = Mathf.Abs(GroundHit.sidewaysSlip);
		wheelSlipAmountForwardBSR = Mathf.Abs(GroundHit.forwardSlip);
		totalSlipBSR = wheelSlipAmountSidewaysBSR + (wheelSlipAmountForwardBSR / 2f);

		if(skidmarksBSR){

			if (wheelSlipAmountSidewaysBSR > startSlipValueBSR || wheelSlipAmountForwardBSR > startSlipValueBSR * 2f){

				Vector3 skidPoint = GroundHit.point + 2f * (rigidBSR.velocity) * Time.deltaTime;

				if(rigidBSR.velocity.magnitude > 1f){
					lastSkidmarkBSR = skidmarksBSR.AddSkidMarkBSR(skidPoint, GroundHit.normal, (wheelSlipAmountSidewaysBSR / 2f) + (wheelSlipAmountForwardBSR / 2f), lastSkidmarkBSR);
					wheelTemparatureBSR += ((wheelSlipAmountSidewaysBSR / 2f) + (wheelSlipAmountForwardBSR / 2f)) / ((Time.fixedDeltaTime * 100f) * Mathf.Lerp(1f, 5f, wheelTemparatureBSR / 150f));
				}else{
					lastSkidmarkBSR = -1;
					wheelTemparatureBSR -= Time.fixedDeltaTime * 5f;
				}

			}else{
				
				lastSkidmarkBSR = -1;
				wheelTemparatureBSR -= Time.fixedDeltaTime * 5f;

			}

			wheelTemparatureBSR = Mathf.Clamp(wheelTemparatureBSR, 0f, 150f);

		}

	}

	private void FrictionsBSR(){

		WheelHit GroundHit;
		wheelColliderBSR.GetGroundHit(out GroundHit);
		bool contacted = false;

		for (int i = 0; i < physicsFrictionsBSR.Length; i++) {

			if(GroundHit.point != Vector3.zero && GroundHit.collider.sharedMaterial == physicsFrictionsBSR[i].groundMaterialBSR){

				contacted = true;
				
				forwardFrictionCurveBSR.stiffness = physicsFrictionsBSR[i].forwardStiffnessBSR;
				sidewaysFrictionCurveBSR.stiffness = (physicsFrictionsBSR[i].sidewaysStiffnessBSR * tractionHelpedSidewaysStiffnessBSR);

				if(RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR == RCC_SettingsBSR.BehaviorType.Drift){
					DriftBSR(Mathf.Abs(GroundHit.forwardSlip));
				}

				wheelColliderBSR.forwardFriction = forwardFrictionCurveBSR;
				wheelColliderBSR.sidewaysFriction = sidewaysFrictionCurveBSR;

				wheelColliderBSR.wheelDampingRate = physicsFrictionsBSR[i].dampBSR;

				if (!RCC_SettingsBSR.InstanceBSR.dontUseAnyParticleEffectsBSR) 
					emissionBSR = allWheelParticlesBSR[i].emission;
				
				audioClipBSR = physicsFrictionsBSR[i].groundSoundBSR;

				if (wheelSlipAmountSidewaysBSR > physicsFrictionsBSR[i].slipBSR || wheelSlipAmountForwardBSR > physicsFrictionsBSR[i].slipBSR){
					emissionBSR.enabled = true;
				}else{
					emissionBSR.enabled = false;
				}

			}

		}

		if(!contacted && physicsMaterialsBSR.useTerrainSplatMapForGroundFrictionsBSR){

			for (int k = 0; k < physicsMaterialsBSR.terrainSplatMapIndexBSR.Length; k++) {

				if(GroundHit.point != Vector3.zero && GroundHit.collider.sharedMaterial == physicsMaterialsBSR.terrainPhysicMaterialBSR){

					if(TerrainSurfaceBSR.GetTextureMixBSR(transform.position) != null && TerrainSurfaceBSR.GetTextureMixBSR(transform.position)[k] > .5f){

						contacted = true;
						
						forwardFrictionCurveBSR.stiffness = physicsFrictionsBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].forwardStiffnessBSR;
						sidewaysFrictionCurveBSR.stiffness = (physicsFrictionsBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].sidewaysStiffnessBSR * tractionHelpedSidewaysStiffnessBSR);

						if(RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR == RCC_SettingsBSR.BehaviorType.Drift){
							DriftBSR(Mathf.Abs(GroundHit.forwardSlip));
						}

						wheelColliderBSR.forwardFriction = forwardFrictionCurveBSR;
						wheelColliderBSR.sidewaysFriction = sidewaysFrictionCurveBSR;

						wheelColliderBSR.wheelDampingRate = physicsFrictionsBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].dampBSR;

						if (!RCC_SettingsBSR.InstanceBSR.dontUseAnyParticleEffectsBSR)
							emissionBSR = allWheelParticlesBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].emission;

						audioClipBSR = physicsFrictionsBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].groundSoundBSR;

						if (wheelSlipAmountSidewaysBSR > physicsFrictionsBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].slipBSR || wheelSlipAmountForwardBSR > physicsFrictionsBSR[physicsMaterialsBSR.terrainSplatMapIndexBSR[k]].slipBSR){
							emissionBSR.enabled = true;
						}else{
							emissionBSR.enabled = false;
						}
							 
					}

				}
				
			}

		}

		if(!contacted){

			forwardFrictionCurveBSR.stiffness = orgForwardStiffnessBSR;
			sidewaysFrictionCurveBSR.stiffness = orgSidewaysStiffnessBSR * tractionHelpedSidewaysStiffnessBSR;

			if(RCC_SettingsBSR.InstanceBSR.behaviorTypeBSR == RCC_SettingsBSR.BehaviorType.Drift){
				DriftBSR(Mathf.Abs(GroundHit.forwardSlip));
			}

			wheelColliderBSR.forwardFriction = forwardFrictionCurveBSR;
			wheelColliderBSR.sidewaysFriction = sidewaysFrictionCurveBSR;

			wheelColliderBSR.wheelDampingRate = physicsFrictionsBSR[0].dampBSR;

			if (!RCC_SettingsBSR.InstanceBSR.dontUseAnyParticleEffectsBSR)
				emissionBSR = allWheelParticlesBSR[0].emission;
			
			audioClipBSR = physicsFrictionsBSR[0].groundSoundBSR;

			if (wheelSlipAmountSidewaysBSR > physicsFrictionsBSR[0].slipBSR || wheelSlipAmountForwardBSR > physicsFrictionsBSR[0].slipBSR){
				emissionBSR.enabled = true;
			}else{
				emissionBSR.enabled = false;
			}

		}

		if (!RCC_SettingsBSR.InstanceBSR.dontUseAnyParticleEffectsBSR) {

			for (int i = 0; i < allWheelParticlesBSR.Count; i++) {

				if (wheelSlipAmountSidewaysBSR > startSlipValueBSR || wheelSlipAmountForwardBSR > startSlipValueBSR) {
				
				} else {
					emissionBSR = allWheelParticlesBSR [i].emission;
					emissionBSR.enabled = false;
				}
			
			}

		}

	}

	private void DriftBSR(float forwardSlip){
		
		Vector3 relativeVelocity = transform.InverseTransformDirection(rigidBSR.velocity);
		float sqrVel = ((relativeVelocity.x * relativeVelocity.x)) / 15f;

		// Forward
		if(wheelColliderBSR == carControllerBSR.FrontLeftWheelCollider.wheelColliderBSR || wheelColliderBSR == carControllerBSR.FrontRightWheelCollider.wheelColliderBSR){
			forwardFrictionCurveBSR.extremumValue = Mathf.Clamp(1f - sqrVel, .1f, maxForwardStiffnessBSR);
			forwardFrictionCurveBSR.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .1f, minForwardStiffnessBSR);
		}else{
			forwardFrictionCurveBSR.extremumValue = Mathf.Clamp(1f - sqrVel, .75f, maxForwardStiffnessBSR);
			forwardFrictionCurveBSR.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .75f,  minForwardStiffnessBSR);
		}

		// Sideways
		if(wheelColliderBSR == carControllerBSR.FrontLeftWheelCollider.wheelColliderBSR || wheelColliderBSR == carControllerBSR.FrontRightWheelCollider.wheelColliderBSR){
			sidewaysFrictionCurveBSR.extremumValue = Mathf.Clamp(1f - sqrVel / 1f, .6f, maxSidewaysStiffnessBSR);
			sidewaysFrictionCurveBSR.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .6f, minSidewaysStiffnessBSR);
		}else{
			sidewaysFrictionCurveBSR.extremumValue = Mathf.Clamp(1f - sqrVel, .5f, maxSidewaysStiffnessBSR);
			sidewaysFrictionCurveBSR.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .5f, minSidewaysStiffnessBSR);
		}

	}

	private void AudioBSR(){

		if(RCC_SettingsBSR.InstanceBSR.useSharedAudioSourcesBSR && isSkiddingBSR())
			return;

		if(totalSlipBSR > startSlipValueBSR){

			if(audioSourceBSR.clip != audioClipBSR)
				audioSourceBSR.clip = audioClipBSR;

			if(!audioSourceBSR.isPlaying)
				audioSourceBSR.Play();

			if(rigidBSR.velocity.magnitude > 1f){
				audioSourceBSR.volume = Mathf.Lerp(audioSourceBSR.volume, Mathf.Lerp(0f, 1f, totalSlipBSR - startSlipValueBSR), Time.deltaTime * 5f);
				audioSourceBSR.pitch = Mathf.Lerp(1f, .8f, audioSourceBSR.volume);
			}else{
				audioSourceBSR.volume = Mathf.Lerp(audioSourceBSR.volume, 0f, Time.deltaTime * 5f);
			}
			
		}else{
			
			audioSourceBSR.volume = Mathf.Lerp(audioSourceBSR.volume, 0f, Time.deltaTime * 5f);

			if(audioSourceBSR.volume <= .05f && audioSourceBSR.isPlaying)
				audioSourceBSR.Stop();
			
		}

	}

	private bool isSkiddingBSR(){

		for (int i = 0; i < allWheelCollidersBSR.Count; i++) {

			if(allWheelCollidersBSR[i].totalSlipBSR > totalSlipBSR)
				return true;

		}

		return false;

	}

}