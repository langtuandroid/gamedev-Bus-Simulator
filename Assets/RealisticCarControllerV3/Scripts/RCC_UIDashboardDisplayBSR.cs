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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Displayer")]
[RequireComponent (typeof(RCC_DashboardInputsBSR))]
public class RCC_UIDashboardDisplayBSR : MonoBehaviour {

	private RCC_DashboardInputsBSR inputsBSR;
	
	[FormerlySerializedAs("RPMLabel")] public Text RPMLabelBSR;
	[FormerlySerializedAs("KMHLabel")] public Text KMHLabelBSR;
	[FormerlySerializedAs("GearLabel")] public Text GearLabelBSR;

	[FormerlySerializedAs("ABS")] public Image ABSBSR;
	[FormerlySerializedAs("ESP")] public Image ESPBSR;
	[FormerlySerializedAs("Park")] public Image ParkBSR;
	[FormerlySerializedAs("Headlights")] public Image HeadlightsBSR;
	[FormerlySerializedAs("leftIndicator")] public Image leftIndicatorBSR;
	[FormerlySerializedAs("rightIndicator")] public Image rightIndicatorBSR;
	
	private void Start () {
		
		inputsBSR = GetComponent<RCC_DashboardInputsBSR>();
		StartCoroutine("LateDisplayBSR");
		
	}

	private void OnEnable(){

		StopAllCoroutines();
		StartCoroutine("LateDisplayBSR");

	}
	
	private IEnumerator LateDisplayBSR () {

		while(true){

			yield return new WaitForSeconds(.04f);
		
			if(RPMLabelBSR){
				RPMLabelBSR.text = inputsBSR.RPMBSR.ToString("0");
			}
			
			if(KMHLabelBSR){
				if (RCC_SettingsBSR.InstanceBSR.unitsBSR == RCC_SettingsBSR.Units.KMH)
					KMHLabelBSR.text = inputsBSR.KMHBSR.ToString ("0");//+ "\nKMH";
				else
					KMHLabelBSR.text = (inputsBSR.KMHBSR * 0.62f).ToString("0") + "\nMPH";
			}

			if(GearLabelBSR){
				if(!inputsBSR.NGearBSR)
					GearLabelBSR.text = inputsBSR.directionBSR == 1 ? (inputsBSR.GearBSR + 1).ToString("0") : "R";
				else
					GearLabelBSR.text = "N";
			}

			if(ABSBSR)
				ABSBSR.color = inputsBSR.ABSBSR == true ? Color.red : Color.white;
			if(ESPBSR)
				ESPBSR.color = inputsBSR.ESPBSR == true ? Color.red : Color.white;
			if(ParkBSR)
				ParkBSR.color = inputsBSR.ParkBSR == true ? Color.red : Color.white;
			if(HeadlightsBSR)
				HeadlightsBSR.color = inputsBSR.HeadlightsBSR == true ? Color.green : Color.white;

			if(leftIndicatorBSR && rightIndicatorBSR){

				switch(inputsBSR.indicatorsBSR){

				case RCC_CarControllerV3.IndicatorsOn.Left:
					leftIndicatorBSR.color = new Color(1f, .5f, 0f);
					rightIndicatorBSR.color = new Color(.5f, .25f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.Right:
					leftIndicatorBSR.color = new Color(.5f, .25f, 0f);
					rightIndicatorBSR.color = new Color(1f, .5f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.All:
					leftIndicatorBSR.color = new Color(1f, .5f, 0f);
					rightIndicatorBSR.color = new Color(1f, .5f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.Off:
					leftIndicatorBSR.color = new Color(.5f, .25f, 0f);
					rightIndicatorBSR.color = new Color(.5f, .25f, 0f);
					break;
				}

			}

		}

	}

}
