using UnityEngine;
using System.Collections;

public class SettingsDialog : MonoBehaviour {
	public RectTransform highLighter;
	public RectTransform [] controllersPosition=new RectTransform[0];
	public HowToPlay howToPlay;
	void Start(){
		if (RCC_SettingsBSR.instanceBSR.useAccelerometerForSteeringBSR == true) {
//			if (Time.timeScale > 0.5) {
//			//	LeanTween.moveX (highLighter, 180f, 0.25f);
//			} else {
//			//	highLighter.localPosition= new Vector2 (180, 0);
//			}
			highLighter.position = new Vector2(controllersPosition[2].position.x,highLighter.position.y);
			//highLighter.localPosition= controllersPosition [2].position;
		}
		else if (RCC_SettingsBSR.instanceBSR.useSteeringWheelForSteeringBSR == true) {
//			if (Time.timeScale > 0.5) {
//				LeanTween.moveX (highLighter, -170f, 0.25f);
//			} else {
//				highLighter.localPosition = new Vector2 (-170, 0);
//			}
//		
			highLighter.position =new Vector2(controllersPosition[1].position.x,highLighter.position.y);
		} else {
//			if (Time.timeScale > 0.5) {
//				LeanTween.moveX (highLighter, 0, 0.25f);
//			}
//			else{
//				highLighter.localPosition= new Vector2 (0, 0);
//			}
			highLighter.position =new Vector2(controllersPosition[0].position.x,highLighter.position.y);
		}
	}
	public void OkButton(){
		if (GameManager.Instance.isHowToPlay) {
			howToPlay.PlayNextAnimation ();
		}
	}
	public void Buttons(string name){
		switch (name) {
		case "Steering":
//			if (Time.timeScale > 0.5) {
//				LeanTween.moveX (highLighter, -170f, 0.25f);
//			} else {
//				highLighter.localPosition = new Vector2 (-170, 0);
//			}
			highLighter.position = new Vector2(controllersPosition[1].position.x,highLighter.position.y);
			RCC_SettingsBSR.instanceBSR.useSteeringWheelForSteeringBSR = true;
			RCC_SettingsBSR.instanceBSR.useAccelerometerForSteeringBSR = false;
			break;
		case "Buttons":
//			if (Time.timeScale > 0.5) {
//				LeanTween.moveX (highLighter, 0, 0.25f);
//			} else {
//				highLighter.localPosition = new Vector2 (0, 0);
//			}
			highLighter.position = new Vector2(controllersPosition[0].position.x,highLighter.position.y);
			RCC_SettingsBSR.instanceBSR.useSteeringWheelForSteeringBSR = false;
			RCC_SettingsBSR.instanceBSR.useAccelerometerForSteeringBSR = false;
			break;
		case "Accelerometer":
//			if (Time.timeScale > 0.5) {
//				LeanTween.moveX (highLighter, 180f, 0.25f);
//			} else {
//				highLighter.localPosition= new Vector2 (180, 0);
//			}	
			highLighter.position =new Vector2(controllersPosition[2].position.x,highLighter.position.y);
			RCC_SettingsBSR.instanceBSR.useSteeringWheelForSteeringBSR = false;
			RCC_SettingsBSR.instanceBSR.useAccelerometerForSteeringBSR = true;
			break;
		}
	}
}
