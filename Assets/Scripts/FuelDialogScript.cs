using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FuelDialogScript : MonoBehaviour {
	public Text userEarningText;
	public Text fuelFillingText;
	int earning = 0;
	int fuelAmount=0;
	public AudioSource audioSource;
	public AudioClip fuelRefill;
	public AudioClip warningClip;
	float tempFuel=0;
	public GameObject unityRewardedAdDialog;
	public GameController gameController;
	public void Buttons(string name){
		switch (name) {
		case "Plus":
			if (earning >= 100 && tempFuel <100) {
				earning -= 100;
				userEarningText.text = "$" + earning;
				fuelAmount += 100;
				tempFuel += 5;
				fuelFillingText.text = "" + fuelAmount;
			} else {
				fuelFillingText.text = "Max " + fuelAmount;
			}

			break;
		case "Minus":
			if(fuelAmount>=100){
				earning += 100;
				userEarningText.text = "" + earning;
				fuelAmount -= 100;
				tempFuel-= 5;
				fuelFillingText.text = "" + fuelAmount;
			}
			break;
		case "Fill":
			
			if (fuelAmount > 0) {
				Time.timeScale = 1f;
				audioSource.clip = fuelRefill;
				audioSource.Play ();
				gameController.FillTank (tempFuel);
				PlayerPrefs.SetInt ("UserEarning", (PlayerPrefs.GetInt ("UserEarning") - fuelAmount));
				gameObject.SetActive (false);
			} else {
				fuelFillingText.text="Amount is too Low";
				//GameObject.FindGameObjectWithTag ("Bus").GetComponent<BusController> ().FillTank (0);
			}
							
			break;
		case "Cancel":
			gameObject.SetActive (false);
			if (fuelAmount == 0) {
				gameController.FillTank (0);
			}
			break;
		case "FreeCash":
			unityRewardedAdDialog.SetActive (true);
			break;

		}

	}
	void OnEnable(){
//		Debug.Log ("Enable");
	//	LeanTween.cancelAll ();
		tempFuel=0;
		//tempFuel=GameObject.FindGameObjectWithTag ("Bus").GetComponent<BusController> ().fuel;
		audioSource.loop = false;
		audioSource.clip = fuelRefill;
		earning = PlayerPrefs.GetInt ("UserEarning");
		if(GameManager.Instance.fromFuelStation==false)
		earning -= 50;
		userEarningText.text = "$" + earning;
		fuelFillingText.text = "0";
		Time.timeScale = 0.01f;
//		Adds.instance.LogGoogleAnalytics ();
	}
	void OnDisable(){
	//	audioSource.loop = true;
	//	audioSource.clip = null;
		earning = 0;
		fuelAmount = 0;
		Time.timeScale = 1;
	}
}
