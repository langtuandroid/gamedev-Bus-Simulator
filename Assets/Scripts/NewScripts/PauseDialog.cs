using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PauseDialog : MonoBehaviour {


	void OnEnable(){
		//	AdsManager._instance.ShowunityAds ();
		Time.timeScale = 0.0001f;
	}
	public void Buttons(string name){
		switch (name) {
		case "Resume":
			gameObject.SetActive (false);
			Time.timeScale = 1f;
			break;
		case "Restart":
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			break;
		case "Home":
			SceneManager.LoadScene ("NewMenu");
			break;

		}
	}
}
