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
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Colors")]
public class RCC_DashboardColorsBSR : MonoBehaviour {

	[FormerlySerializedAs("huds")] public Image[] hudsBSR;
	[FormerlySerializedAs("hudColor")] public Color hudColorBSR = Color.white;

	[FormerlySerializedAs("hudColor_R")] public Slider hudColor_RBSR;
	[FormerlySerializedAs("hudColor_G")] public Slider hudColor_GBSR;
	[FormerlySerializedAs("hudColor_B")] public Slider hudColor_BBSR;

	private void Awake () {

		if(hudsBSR == null || hudsBSR.Length < 1)
			enabled = false;

		if(hudColor_RBSR && hudColor_GBSR && hudColor_BBSR){
			hudColor_RBSR.value = hudColorBSR.r;
			hudColor_GBSR.value = hudColorBSR.g;
			hudColor_BBSR.value = hudColorBSR.b;
		}
	
	}

	private void Update () {

		if(hudColor_RBSR && hudColor_GBSR && hudColor_BBSR)
			hudColorBSR = new Color(hudColor_RBSR.value, hudColor_GBSR.value, hudColor_BBSR.value);

		for (int i = 0; i < hudsBSR.Length; i++) {

			hudsBSR[i].color = new Color(hudColorBSR.r, hudColorBSR.g, hudColorBSR.b, hudsBSR[i].color.a);

		}
	
	}

}
