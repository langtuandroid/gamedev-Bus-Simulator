//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Button")]
public class RCC_UIDashboardButtonBSR : MonoBehaviour {
	
	[FormerlySerializedAs("_buttonType")] public ButtonType _buttonTypeBSR;
	public enum ButtonType{Start, ABS, ESP, TCS, Headlights, LeftIndicator, RightIndicator, Gear, Low, Med, High, SH};
	private Scrollbar gearSliderBSR;

	private RCC_CarControllerV3[] carControllersBSR;
	private int gearDirectionBSR = 0;

	private void Start(){

		if(GetComponentInChildren<Scrollbar>()){
			gearSliderBSR = GetComponentInChildren<Scrollbar>();
			gearSliderBSR.onValueChanged.AddListener (delegate {ChangeGearBSR ();});
		}

	}

	private void OnEnable(){

		CheckBSR();

	}
	
	public void OnClicked () {
		
		carControllersBSR = GameObject.FindObjectsOfType<RCC_CarControllerV3>();
		
		switch(_buttonTypeBSR){
			
		case ButtonType.Start:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController)
					carControllersBSR[i].KillOrStartEngine();
				
			}
			
			break;
			
		case ButtonType.ABS:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController)
					carControllersBSR[i].ABS = !carControllersBSR[i].ABS;
				
			}
			
			break;
			
		case ButtonType.ESP:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController)
					carControllersBSR[i].ESP = !carControllersBSR[i].ESP;
				
			}
			
			break;
			
		case ButtonType.TCS:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController)
					carControllersBSR[i].TCS = !carControllersBSR[i].TCS;
				
			}
			
			break;

		case ButtonType.SH:

			for(int i = 0; i < carControllersBSR.Length; i++){

				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController)
					carControllersBSR[i].steeringHelper = !carControllersBSR[i].steeringHelper;

			}

			break;
			
		case ButtonType.Headlights:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController){
					if(!carControllersBSR[i].highBeamHeadLightsOn && carControllersBSR[i].lowBeamHeadLightsOn){
						carControllersBSR[i].highBeamHeadLightsOn = true;
						carControllersBSR[i].lowBeamHeadLightsOn = true;
						break;
					}
					if(!carControllersBSR[i].lowBeamHeadLightsOn)
						carControllersBSR[i].lowBeamHeadLightsOn = true;
					if(carControllersBSR[i].highBeamHeadLightsOn){
						carControllersBSR[i].lowBeamHeadLightsOn = false;
						carControllersBSR[i].highBeamHeadLightsOn = false;
					}
				}
				
			}
			
			break;

		case ButtonType.LeftIndicator:

			for(int i = 0; i < carControllersBSR.Length; i++){

				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController){
					if(carControllersBSR[i].indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Left)
						carControllersBSR[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Left;
					else
						carControllersBSR[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}

			}

			break;

		case ButtonType.RightIndicator:

			for(int i = 0; i < carControllersBSR.Length; i++){

				if(carControllersBSR[i].canControl && !carControllersBSR[i].AIController){
					if(carControllersBSR[i].indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Right)
						carControllersBSR[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Right;
					else
						carControllersBSR[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}

			}

			break;

		case ButtonType.Low:

			QualitySettings.SetQualityLevel (1);

			break;

		case ButtonType.Med:

			QualitySettings.SetQualityLevel (3);

			break;

		case ButtonType.High:

			QualitySettings.SetQualityLevel (5);

			break;
			
		}
		
		CheckBSR();
		
	}
	
	public void CheckBSR(){
		
		carControllersBSR = GameObject.FindObjectsOfType<RCC_CarControllerV3>();

		if (!GetComponent<Image> ())
			return;
		
		switch(_buttonTypeBSR){
			
		case ButtonType.ABS:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl && carControllersBSR[i].ABS)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.ESP:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl && carControllersBSR[i].ESP)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.TCS:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl && carControllersBSR[i].TCS)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;

		case ButtonType.SH:

			for(int i = 0; i < carControllersBSR.Length; i++){

				if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl && carControllersBSR[i].steeringHelper)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);

			}

			break;
			
		case ButtonType.Headlights:
			
			for(int i = 0; i < carControllersBSR.Length; i++){
				
				if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl && carControllersBSR[i].lowBeamHeadLightsOn || carControllersBSR[i].highBeamHeadLightsOn)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		}
		
	}

	public void ChangeGearBSR(){

		if(gearDirectionBSR == (int)gearSliderBSR.value)
			return;

		gearDirectionBSR = (int)gearSliderBSR.value;

		for(int i = 0; i < carControllersBSR.Length; i++){

			if(!carControllersBSR[i].AIController && carControllersBSR[i].canControl){
				
				carControllersBSR[i].semiAutomaticGear = true;

				if(gearDirectionBSR == 1)
					carControllersBSR[i].StartCoroutine("ChangingGear", -1);
				else
					carControllersBSR[i].StartCoroutine("ChangingGear", 0);
				
			}

		}

	}

	private void OnDisable(){

		if(_buttonTypeBSR == ButtonType.Gear){

			carControllersBSR = GameObject.FindObjectsOfType<RCC_CarControllerV3>();

			foreach(RCC_CarControllerV3 rcc in carControllersBSR){

				if(!rcc.AIController && rcc.canControl)
					rcc.semiAutomaticGear = false;

			}

		}

	}
	
}
