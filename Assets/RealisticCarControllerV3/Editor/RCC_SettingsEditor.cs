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

[CustomEditor(typeof(RCC_SettingsBSR))]
public class RCC_SettingsEditor : Editor {

//	[MenuItem("Assets/Create/YourClass")]
//	public static void CreateAsset ()
//	{
//		ScriptableObjectUtility.CreateAsset<Vehicles> ();
//	}

	RCC_SettingsBSR RCCSettingsAsset;

	Color originalGUIColor;
	Vector2 scrollPos;
	PhysicMaterial[] physicMaterials;

	bool foldGeneralSettings = false;
	bool foldControllerSettings = false;
	bool foldUISettings = false;
	bool foldWheelPhysics = false;
	bool foldSFX = false;
	bool foldOptimization = false;
	bool foldTagsAndLayers = false;

	void OnEnable(){

		foldGeneralSettings = RCC_SettingsBSR.InstanceBSR.foldGeneralSettingsBSR;
		foldControllerSettings = RCC_SettingsBSR.InstanceBSR.foldControllerSettingsBSR;
		foldUISettings = RCC_SettingsBSR.InstanceBSR.foldUISettingsBSR;
		foldWheelPhysics = RCC_SettingsBSR.InstanceBSR.foldWheelPhysicsBSR;
		foldSFX = RCC_SettingsBSR.InstanceBSR.foldSFXBSR;
		foldOptimization = RCC_SettingsBSR.InstanceBSR.foldOptimizationBSR;
		foldTagsAndLayers = RCC_SettingsBSR.InstanceBSR.foldTagsAndLayersBSR;

	}

	void OnDestroy(){

		RCC_SettingsBSR.InstanceBSR.foldGeneralSettingsBSR = foldGeneralSettings;
		RCC_SettingsBSR.InstanceBSR.foldControllerSettingsBSR = foldControllerSettings;
		RCC_SettingsBSR.InstanceBSR.foldUISettingsBSR = foldUISettings;
		RCC_SettingsBSR.InstanceBSR.foldWheelPhysicsBSR = foldWheelPhysics;
		RCC_SettingsBSR.InstanceBSR.foldSFXBSR = foldSFX;
		RCC_SettingsBSR.InstanceBSR.foldOptimizationBSR = foldOptimization;
		RCC_SettingsBSR.InstanceBSR.foldTagsAndLayersBSR = foldTagsAndLayers;

	}

	public override void OnInspectorGUI (){

		serializedObject.Update();
		RCCSettingsAsset = (RCC_SettingsBSR)target;

		originalGUIColor = GUI.color;
		EditorGUIUtility.labelWidth = 250;
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("RCC Asset Settings Editor Window", EditorStyles.boldLabel);
		GUI.color = new Color(.75f, 1f, .75f);
		EditorGUILayout.LabelField("This editor will keep update necessary .asset files in your project for RCC. Don't change directory of the ''Resources/RCCAssets''.", EditorStyles.helpBox);
		GUI.color = originalGUIColor;
		EditorGUILayout.Space();

		EditorGUI.indentLevel++;

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false );

		EditorGUILayout.Space();

		foldGeneralSettings = EditorGUILayout.Foldout(foldGeneralSettings, "General Settings");

		if(foldGeneralSettings){

			EditorGUILayout.BeginVertical (GUI.skin.box);
			GUILayout.Label("General Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("overrideFixedTimeStep"), new GUIContent("Override FixedTimeStep"));
			if(RCCSettingsAsset.overrideFixedTimeStepBSR)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedTimeStep"), new GUIContent("Fixed Timestep"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAngularVelocity"), new GUIContent("Maximum Angular Velocity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("behaviorType"), new GUIContent("Behavior Type"));
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("Using behavior preset will override wheelcollider settings, chassis joint, antirolls, and other stuff. Using ''Custom'' mode will not override anything.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useFixedWheelColliders"), new GUIContent("Use Fixed WheelColliders"));
			EditorGUILayout.EndVertical ();

		}

		EditorGUILayout.Space();

		foldControllerSettings = EditorGUILayout.Foldout(foldControllerSettings, "Controller Settings");

		if(foldControllerSettings){
			
			List<string> controllerTypeStrings =  new List<string>();
			controllerTypeStrings.Add("Keyboard");	controllerTypeStrings.Add("Mobile");		controllerTypeStrings.Add("Custom");
			EditorGUILayout.BeginVertical (GUI.skin.box);

			GUI.color = new Color(.5f, 1f, 1f, 1f);
			GUILayout.Label("Main Controller Type", EditorStyles.boldLabel);
			RCCSettingsAsset.toolbarSelectedIndexBSR = GUILayout.Toolbar(RCCSettingsAsset.toolbarSelectedIndexBSR, controllerTypeStrings.ToArray());
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();


			if(RCCSettingsAsset.toolbarSelectedIndexBSR == 0){

				RCCSettingsAsset.controllerTypeBSR = RCC_SettingsBSR.ControllerType.Keyboard;

				EditorGUILayout.BeginVertical (GUI.skin.box);

				GUILayout.Label("Keyboard Settings", EditorStyles.boldLabel);

				EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalInput"), new GUIContent("Gas/Reverse Input Axis"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalInput"), new GUIContent("Steering Input Axis"));
				GUI.color = new Color(.75f, 1f, .75f);
				EditorGUILayout.HelpBox("You can edit your vertical and horizontal input axis in Edit --> Project Settings --> Input.", MessageType.Info);
				GUI.color = originalGUIColor;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("startEngineKB"), new GUIContent("Start/Stop Engine Key"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lowBeamHeadlightsKB"), new GUIContent("Low Beam Headlights"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("highBeamHeadlightsKB"), new GUIContent("High Beam Headlights"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("changeCameraKB"), new GUIContent("Change Camera"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rightIndicatorKB"), new GUIContent("Indicator Right"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("leftIndicatorKB"), new GUIContent("Indicator Left"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("hazardIndicatorKB"), new GUIContent("Indicator Hazard"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("shiftGearUp"), new GUIContent("Gear Shift Up"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("shiftGearDown"), new GUIContent("Gear Shift Down"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("boostKB"), new GUIContent("Boost"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("enterExitVehicleKB"), new GUIContent("Get In & Get Out Of The Vehicle"));
				EditorGUILayout.Space();

				EditorGUILayout.EndVertical ();

		}
				
		if(RCCSettingsAsset.toolbarSelectedIndexBSR == 1){

			EditorGUILayout.BeginVertical (GUI.skin.box);

			RCCSettingsAsset.controllerTypeBSR = RCC_SettingsBSR.ControllerType.Mobile;

			GUILayout.Label("Mobile Settings", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("uiType"), new GUIContent("UI Type"));

			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("All UI/NGUI buttons will feed the vehicles at runtime.", MessageType.Info);
			GUI.color = originalGUIColor;

			EditorGUILayout.PropertyField(serializedObject.FindProperty("UIButtonSensitivity"), new GUIContent("UI Button Sensitivity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("UIButtonGravity"), new GUIContent("UI Button Gravity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("gyroSensitivity"), new GUIContent("Gyro Sensitivity"));

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useAccelerometerForSteering"), new GUIContent("Use Accelerometer For Steering"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useSteeringWheelForSteering"), new GUIContent("Use Steering Wheel For Steering"));
			
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("You can enable/disable Accelerometer in your game by just calling ''RCCSettings.Instance.useAccelerometerForSteering = true/false;''.", MessageType.Info);
			EditorGUILayout.HelpBox("You can enable/disable Steering Wheel Controlling in your game by just calling ''RCCSettings.Instance.useSteeringWheelForSteering = true/false;''.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();

			EditorGUILayout.EndVertical ();

		}

		if(RCCSettingsAsset.toolbarSelectedIndexBSR == 2){

				EditorGUILayout.BeginVertical (GUI.skin.box);

			RCCSettingsAsset.controllerTypeBSR = RCC_SettingsBSR.ControllerType.Custom;

				GUILayout.Label("Custom Input Settings", EditorStyles.boldLabel);

				GUI.color = new Color(.75f, 1f, .75f);
				EditorGUILayout.HelpBox("In this mode, car controller won't receive these inputs from keyboard or UI buttons. You need to feed these inputs in your own script.", MessageType.Info);
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("Car controller uses these inputs; \n  \n    gasInput = Clamped 0f - 1f.  \n    brakeInput = Clamped 0f - 1f.  \n    steerInput = Clamped -1f - 1f. \n    clutchInput = Clamped 0f - 1f. \n    handbrakeInput = Clamped 0f - 1f. \n    boostInput = Clamped 0f - 1f.", MessageType.Info);
				EditorGUILayout.Space();
				GUI.color = originalGUIColor;

				EditorGUILayout.EndVertical ();
			
		}

			EditorGUILayout.BeginVertical(GUI.skin.box);

			GUILayout.Label("Main Controller Settings", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("units"), new GUIContent("Units"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useAutomaticGear"), new GUIContent("Use Automatic Gear"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("runEngineAtAwake"), new GUIContent("Engines Are Running At Awake"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("keepEnginesAlive"), new GUIContent("Keep Engines Alive When Player Get In-Out Vehicles"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoReverse"), new GUIContent("Auto Reverse"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("contactParticles"), new GUIContent("Contact Particles On Collision"));

			EditorGUILayout.EndVertical ();

		EditorGUILayout.EndVertical ();




		}

		EditorGUILayout.Space();

		foldUISettings = EditorGUILayout.Foldout(foldUISettings, "UI Settings");

		if(foldUISettings){
			
			EditorGUILayout.BeginVertical (GUI.skin.box);
			GUILayout.Label("UI Dashboard Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("uiType"), new GUIContent("UI Type"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useTelemetry"), new GUIContent("Use Telemetry"));
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical ();

		}

		EditorGUILayout.Space();

		foldWheelPhysics = EditorGUILayout.Foldout(foldWheelPhysics, "Wheel Physics Settings");

		if(foldWheelPhysics){

			if(RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR != null && RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR.Length > 0){

					EditorGUILayout.BeginVertical (GUI.skin.box);
					GUILayout.Label("Ground Physic Materials", EditorStyles.boldLabel);

					physicMaterials = new PhysicMaterial[RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR.Length];
					
					for (int i = 0; i < physicMaterials.Length; i++) {
						physicMaterials[i] = RCC_GroundMaterialsBSR.InstanceBsr.frictionsBSR[i].groundMaterialBSR;
						EditorGUILayout.BeginVertical(GUI.skin.box);
						EditorGUILayout.ObjectField("Ground Physic Materials " + i, physicMaterials[i], typeof(PhysicMaterial), false);
						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.Space();

			}

			GUI.color = new Color(.5f, 1f, 1f, 1f);
			
			if(GUILayout.Button("Configure Ground Physic Materials")){
				Selection.activeObject = Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterialsBSR;
			}

			GUI.color = originalGUIColor;

			EditorGUILayout.EndVertical ();

		}

		EditorGUILayout.Space();

		foldSFX = EditorGUILayout.Foldout(foldSFX, "SFX Settings");

		if(foldSFX){

			EditorGUILayout.BeginVertical(GUI.skin.box);

			GUILayout.Label("Sound FX", EditorStyles.boldLabel);

			EditorGUILayout.Space();
			GUI.color = new Color(.5f, 1f, 1f, 1f);
			if(GUILayout.Button("Configure Wheel Slip Sounds")){
				Selection.activeObject = Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterialsBSR;
			}
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("crashClips"), new GUIContent("Crashing Sounds"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("gearShiftingClips"), new GUIContent("Gear Shifting Sounds"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("indicatorClip"), new GUIContent("Indicator Clip"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("exhaustFlameClips"), new GUIContent("Exhaust Flame Clips"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("NOSClip"), new GUIContent("NOS Clip"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("turboClip"), new GUIContent("Turbo Clip"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("blowoutClip"), new GUIContent("Blowout Clip"), true);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("reversingClip"), new GUIContent("Reverse Transmission Sound"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("windClip"), new GUIContent("Wind Sound"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeClip"), new GUIContent("Brake Sound"), true);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxGearShiftingSoundVolume"), new GUIContent("Max Gear Shifting Sound Volume"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxCrashSoundVolume"), new GUIContent("Max Crash Sound Volume"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxWindSoundVolume"), new GUIContent("Max Wind Sound Volume"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxBrakeSoundVolume"), new GUIContent("Max Brake Sound Volume"), true);

			EditorGUILayout.EndVertical();

		}

		EditorGUILayout.Space();

		foldOptimization = EditorGUILayout.Foldout(foldOptimization, "Optimization");

		if(foldOptimization){

			EditorGUILayout.BeginVertical(GUI.skin.box);

			GUILayout.Label("Optimization", EditorStyles.boldLabel);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useLightsAsVertexLights"), new GUIContent("Use Lights As Vertex Lights On Vehicles"));
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("Always use vertex lights for mobile platform. Even only one pixel light will drop your performance dramaticaly!", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useLightProjectorForLightingEffect"), new GUIContent("Use Light Projector For Lighting Effect"));
			GUI.color = new Color(.75f, .75f, 0f);
			EditorGUILayout.HelpBox("Unity's Projector will be used for lighting effect. Be sure it effects to your road only. Select ignored layers below this section. Don't let projectors hits the vehicle itself. It may increase your drawcalls if it hits unnecessary high numbered materials. It should just hit the road, nothing else.", MessageType.Warning);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("projectorIgnoreLayer"), new GUIContent("Light Projector Ignore Layer"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useSharedAudioSources"), new GUIContent("Use Shared Audio Sources", "For ex, 4 Audio Sources will be created for each wheel. This option merges them to only 1 Audio Source."), true);
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("For ex, 4 Audio Sources will be created for each wheelslip SFX. This option merges them to only 1 Audio Source.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dontUseAnyParticleEffects"), new GUIContent("Do Not Use Any Particle Effects"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dontUseSkidmarks"), new GUIContent("Do Not Use Skidmarks"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dontUseChassisJoint"), new GUIContent("Do Not Use Chassis Joint"));
			GUI.color = new Color(.75f, 1f, .75f);
			if(!RCCSettingsAsset.dontUseChassisJointBSR)
				EditorGUILayout.HelpBox("Chassis Joint is a main Configurable Joint for realistic body movements. Script is getting all colliders attached to chassis, and moves them outside to joint. It can be trouble if you are making game about interacting objects inside the car. If you don't want to use it, chassis will be simulated based on rigid velocity and angular velocity, like older versions of RCC.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();

			EditorGUILayout.EndVertical();

		}

		foldTagsAndLayers = EditorGUILayout.Foldout(foldTagsAndLayers, "Tags & Layers");

		if (foldTagsAndLayers) {

			EditorGUILayout.BeginVertical (GUI.skin.box);

			GUILayout.Label ("Tags & Layers", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("setTagsAndLayers"), new GUIContent("Set Tags And Layers Auto"), false);

			if (RCCSettingsAsset.setTagsAndLayersBSR) {

				EditorGUILayout.PropertyField (serializedObject.FindProperty ("RCCLayer"), new GUIContent ("Vehicle Layer"), false);
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("RCCTag"), new GUIContent ("Vehicle Tag"), false);
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("tagAllChildrenGameobjects"), new GUIContent ("Tag All Children Gameobjects"), false);
				GUI.color = new Color (.75f, 1f, .75f);
				EditorGUILayout.HelpBox ("Be sure you have that tag and layer in your Tags & Layers", MessageType.Warning);
				EditorGUILayout.HelpBox ("All vehicles powered by Realistic Car Controller are using this layer. What does this layer do? It was used for masking wheel rays, light masks, and projector masks. Just create a new layer for vehicles from Edit --> Project Settings --> Tags & Layers, and select the layer here.", MessageType.Info);
				GUI.color = originalGUIColor;

			}

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();

		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("headLights"), new GUIContent("Head Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeLights"), new GUIContent("Brake Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("reverseLights"), new GUIContent("Reverse Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("indicatorLights"), new GUIContent("Indicator Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("mirrors"), new GUIContent("Mirrors"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("skidmarksManager"), new GUIContent("Skidmarks Manager"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("projector"), new GUIContent("Light Projector"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("exhaustGas"), new GUIContent("Exhaust Gas"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("chassisJoint"), new GUIContent("Chassis Joint"), false);

		EditorGUILayout.Space();

		EditorGUILayout.EndScrollView();
		
		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical (GUI.skin.button);

		GUI.color = new Color(.75f, 1f, .75f);

		GUI.color = new Color(.5f, 1f, 1f, 1f);
		
		if(GUILayout.Button("Reset To Defaults")){
			ResetToDefaults();
			Debug.Log("Resetted To Defaults!");
		}
		
		if(GUILayout.Button("Open PDF Documentation")){
			string url = "http://www.bonecrackergames.com/realistic-car-controller";
			Application.OpenURL(url);
		}

		GUI.color = originalGUIColor;
		
		EditorGUILayout.LabelField("Realistic Car Controller V3.0f\nBoneCrackerGames", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		EditorGUILayout.LabelField("Created by Buğra Özdoğanlar", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
		
		if(GUI.changed)
			EditorUtility.SetDirty(RCCSettingsAsset);

	}

	void ResetToDefaults(){

		RCCSettingsAsset.overrideFixedTimeStepBSR = true;
		RCCSettingsAsset.fixedTimeStepBSR = .02f;
		RCCSettingsAsset.maxAngularVelocityBSR = 6f;
		RCCSettingsAsset.behaviorTypeBSR = RCC_SettingsBSR.BehaviorType.Custom;
		RCCSettingsAsset.verticalInputBSR = "Vertical";
		RCCSettingsAsset.horizontalInputBSR = "Horizontal";
		RCCSettingsAsset.handbrakeKBBSR = KeyCode.Space;
		RCCSettingsAsset.startEngineKBBSR = KeyCode.I;
		RCCSettingsAsset.lowBeamHeadlightsKBBSR = KeyCode.L;
		RCCSettingsAsset.highBeamHeadlightsKBBSR = KeyCode.K;
		RCCSettingsAsset.rightIndicatorKBBSR = KeyCode.E;
		RCCSettingsAsset.leftIndicatorKBBSR = KeyCode.Q;
		RCCSettingsAsset.hazardIndicatorKBBSR = KeyCode.Z;
		RCCSettingsAsset.shiftGearUpBSR = KeyCode.LeftShift;
		RCCSettingsAsset.shiftGearDownBSR = KeyCode.LeftControl;
		RCCSettingsAsset.boostKBBSR = KeyCode.F;
		RCCSettingsAsset.changeCameraKBBSR = KeyCode.C;
		RCCSettingsAsset.enterExitVehicleKBBSR = KeyCode.E;
		RCCSettingsAsset.useAutomaticGearBSR = true;
		RCCSettingsAsset.runEngineAtAwakeBSR = true;
		RCCSettingsAsset.keepEnginesAliveBSR = true;
		RCCSettingsAsset.autoReverseBSR = true;
		RCCSettingsAsset.unitsBSR = RCC_SettingsBSR.Units.KMH;
		RCCSettingsAsset.uiTypeBSR = RCC_SettingsBSR.UIType.UI;
		RCCSettingsAsset.useTelemetryBSR = false;
		RCCSettingsAsset.useAccelerometerForSteeringBSR = false;
		RCCSettingsAsset.useSteeringWheelForSteeringBSR = false;
		RCCSettingsAsset.UIButtonSensitivityBSR = 3f;
		RCCSettingsAsset.UIButtonGravityBSR = 5f;
		RCCSettingsAsset.gyroSensitivityBSR = 2f;
		RCCSettingsAsset.useLightsAsVertexLightsBSR = true;
		RCCSettingsAsset.useLightProjectorForLightingEffectBSR = false;
		RCCSettingsAsset.RCCLayerBSR = "RCC";
		RCCSettingsAsset.RCCTagBSR = "Player";
		RCCSettingsAsset.tagAllChildrenGameobjectsBSR = false;
		RCCSettingsAsset.dontUseAnyParticleEffectsBSR = false;
		RCCSettingsAsset.dontUseChassisJointBSR = false;
		RCCSettingsAsset.dontUseSkidmarksBSR = false;
		RCCSettingsAsset.useSharedAudioSourcesBSR = true;
		RCCSettingsAsset.maxGearShiftingSoundVolumeBSR = .25f;
		RCCSettingsAsset.maxCrashSoundVolumeBSR = 1f;
		RCCSettingsAsset.maxWindSoundVolumeBSR = .1f;
		RCCSettingsAsset.maxBrakeSoundVolumeBSR = .1f;
		RCCSettingsAsset.foldGeneralSettingsBSR = false;
		RCCSettingsAsset.foldControllerSettingsBSR = false;
		RCCSettingsAsset.foldUISettingsBSR = false;
		RCCSettingsAsset.foldWheelPhysicsBSR = false;
		RCCSettingsAsset.foldSFXBSR = false;
		RCCSettingsAsset.foldOptimizationBSR = false;

	}

}
