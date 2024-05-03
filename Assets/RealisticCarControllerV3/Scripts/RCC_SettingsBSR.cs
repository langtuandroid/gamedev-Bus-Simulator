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
public class RCC_SettingsBSR : ScriptableObject {
	
	#region singleton
	public static RCC_SettingsBSR instanceBSR;
	public static RCC_SettingsBSR InstanceBSR{	get{if(instanceBSR == null) instanceBSR = Resources.Load("RCCAssets/RCC_Settings") as RCC_SettingsBSR; return instanceBSR;}}
	#endregion

	[FormerlySerializedAs("toolbarSelectedIndex")] public int toolbarSelectedIndexBSR;

	[FormerlySerializedAs("overrideFixedTimeStep")] public bool overrideFixedTimeStepBSR = true;
	[FormerlySerializedAs("fixedTimeStep")] [Range(.005f, .06f)]public float fixedTimeStepBSR = .02f;
	[FormerlySerializedAs("maxAngularVelocity")] [Range(.5f, 20f)]public float maxAngularVelocityBSR = 6;

	// Behavior Types
	[FormerlySerializedAs("behaviorType")] public BehaviorType behaviorTypeBSR;
	public enum BehaviorType{Simulator, Racing, SemiArcade, Drift, Fun, Custom}
	[FormerlySerializedAs("useFixedWheelColliders")] public bool useFixedWheelCollidersBSR = true;

	// Controller Type
	[FormerlySerializedAs("controllerType")] public ControllerType controllerTypeBSR;
	public enum ControllerType{Keyboard, Mobile, Custom}

	// Keyboard Inputs
	[FormerlySerializedAs("verticalInput")] public string verticalInputBSR = "Vertical";
	[FormerlySerializedAs("horizontalInput")] public string horizontalInputBSR = "Horizontal";
	[FormerlySerializedAs("handbrakeKB")] public KeyCode handbrakeKBBSR = KeyCode.Space;
	[FormerlySerializedAs("startEngineKB")] public KeyCode startEngineKBBSR = KeyCode.I;
	[FormerlySerializedAs("lowBeamHeadlightsKB")] public KeyCode lowBeamHeadlightsKBBSR = KeyCode.L;
	[FormerlySerializedAs("highBeamHeadlightsKB")] public KeyCode highBeamHeadlightsKBBSR = KeyCode.K;
	[FormerlySerializedAs("rightIndicatorKB")] public KeyCode rightIndicatorKBBSR = KeyCode.E;
	[FormerlySerializedAs("leftIndicatorKB")] public KeyCode leftIndicatorKBBSR = KeyCode.Q;
	[FormerlySerializedAs("hazardIndicatorKB")] public KeyCode hazardIndicatorKBBSR = KeyCode.Z;
	[FormerlySerializedAs("shiftGearUp")] public KeyCode shiftGearUpBSR = KeyCode.LeftShift;
	[FormerlySerializedAs("shiftGearDown")] public KeyCode shiftGearDownBSR = KeyCode.LeftControl;
	[FormerlySerializedAs("boostKB")] public KeyCode boostKBBSR = KeyCode.F;
	[FormerlySerializedAs("changeCameraKB")] public KeyCode changeCameraKBBSR = KeyCode.C;
	[FormerlySerializedAs("enterExitVehicleKB")] public KeyCode enterExitVehicleKBBSR = KeyCode.E;

	// Main Controller Settings
	[FormerlySerializedAs("useAutomaticGear")] public bool useAutomaticGearBSR = true;
	[FormerlySerializedAs("runEngineAtAwake")] public bool runEngineAtAwakeBSR = true;
	[FormerlySerializedAs("keepEnginesAlive")] public bool keepEnginesAliveBSR = true;
	[FormerlySerializedAs("autoReverse")] public bool autoReverseBSR = true;
	[FormerlySerializedAs("contactParticles")] public GameObject contactParticlesBSR;
	[FormerlySerializedAs("units")] public Units unitsBSR;
	public enum Units {KMH, MPH}

	// UI Dashboard Type
	[FormerlySerializedAs("uiType")] public UIType uiTypeBSR;
	public enum UIType{UI, NGUI, None}

	// Information telemetry about current vehicle
	[FormerlySerializedAs("useTelemetry")] public bool useTelemetryBSR = false;

	// For mobile usement
	[FormerlySerializedAs("useAccelerometerForSteering")] public bool useAccelerometerForSteeringBSR;
	[FormerlySerializedAs("useSteeringWheelForSteering")] public bool useSteeringWheelForSteeringBSR;

	// Mobile controller buttons and accelerometer sensitivity
	[FormerlySerializedAs("UIButtonSensitivity")] public float UIButtonSensitivityBSR = 3f;
	[FormerlySerializedAs("UIButtonGravity")] public float UIButtonGravityBSR = 5f;
	[FormerlySerializedAs("gyroSensitivity")] public float gyroSensitivityBSR = 2f;

	// Used for using the lights more efficent and realistic
	[FormerlySerializedAs("useLightsAsVertexLights")] public bool useLightsAsVertexLightsBSR = true;
	[FormerlySerializedAs("useLightProjectorForLightingEffect")] public bool useLightProjectorForLightingEffectBSR = false;

	// Other stuff
	[FormerlySerializedAs("setTagsAndLayers")] public bool setTagsAndLayersBSR = false;
	[FormerlySerializedAs("RCCLayer")] public string RCCLayerBSR;
	[FormerlySerializedAs("RCCTag")] public string RCCTagBSR;
	[FormerlySerializedAs("tagAllChildrenGameobjects")] public bool tagAllChildrenGameobjectsBSR = false;

	[FormerlySerializedAs("chassisJoint")] public GameObject chassisJointBSR;
	[FormerlySerializedAs("exhaustGas")] public GameObject exhaustGasBSR;
	[FormerlySerializedAs("skidmarksManager")] public RCC_SkidmarksBSR skidmarksManagerBSR;
	[FormerlySerializedAs("projector")] public GameObject projectorBSR;
	[FormerlySerializedAs("projectorIgnoreLayer")] public LayerMask projectorIgnoreLayerBSR;

	[FormerlySerializedAs("headLights")] public GameObject headLightsBSR;
	[FormerlySerializedAs("brakeLights")] public GameObject brakeLightsBSR;
	[FormerlySerializedAs("reverseLights")] public GameObject reverseLightsBSR;
	[FormerlySerializedAs("indicatorLights")] public GameObject indicatorLightsBSR;
	[FormerlySerializedAs("mirrors")] public GameObject mirrorsBSR;

	[FormerlySerializedAs("dontUseAnyParticleEffects")] public bool dontUseAnyParticleEffectsBSR = false;
	[FormerlySerializedAs("dontUseChassisJoint")] public bool dontUseChassisJointBSR = false;
	[FormerlySerializedAs("dontUseSkidmarks")] public bool dontUseSkidmarksBSR = false;

	// Sound FX
	[FormerlySerializedAs("gearShiftingClips")] public AudioClip[] gearShiftingClipsBSR;
	[FormerlySerializedAs("crashClips")] public AudioClip[] crashClipsBSR;
	[FormerlySerializedAs("reversingClip")] public AudioClip reversingClipBSR;
	[FormerlySerializedAs("windClip")] public AudioClip windClipBSR;
	[FormerlySerializedAs("brakeClip")] public AudioClip brakeClipBSR;
	[FormerlySerializedAs("indicatorClip")] public AudioClip indicatorClipBSR;
	[FormerlySerializedAs("NOSClip")] public AudioClip NOSClipBSR;
	[FormerlySerializedAs("turboClip")] public AudioClip turboClipBSR;
	[FormerlySerializedAs("blowoutClip")] public AudioClip[] blowoutClipBSR;
	[FormerlySerializedAs("exhaustFlameClips")] public AudioClip[] exhaustFlameClipsBSR;
	[FormerlySerializedAs("useSharedAudioSources")] public bool useSharedAudioSourcesBSR = true;

	[FormerlySerializedAs("maxGearShiftingSoundVolume")] [Range(0f, 1f)]public float maxGearShiftingSoundVolumeBSR = .25f;
	[FormerlySerializedAs("maxCrashSoundVolume")] [Range(0f, 1f)]public float maxCrashSoundVolumeBSR = 1f;
	[FormerlySerializedAs("maxWindSoundVolume")] [Range(0f, 1f)]public float maxWindSoundVolumeBSR = .1f;
	[FormerlySerializedAs("maxBrakeSoundVolume")] [Range(0f, 1f)]public float maxBrakeSoundVolumeBSR = .1f;

	// Used for folding sections of RCC Settings
	[FormerlySerializedAs("foldGeneralSettings")] public bool foldGeneralSettingsBSR = false;
	[FormerlySerializedAs("foldControllerSettings")] public bool foldControllerSettingsBSR = false;
	[FormerlySerializedAs("foldUISettings")] public bool foldUISettingsBSR = false;
	[FormerlySerializedAs("foldWheelPhysics")] public bool foldWheelPhysicsBSR = false;
	[FormerlySerializedAs("foldSFX")] public bool foldSFXBSR = false;
	[FormerlySerializedAs("foldOptimization")] public bool foldOptimizationBSR = false;
	[FormerlySerializedAs("foldTagsAndLayers")] public bool foldTagsAndLayersBSR = false;

}
