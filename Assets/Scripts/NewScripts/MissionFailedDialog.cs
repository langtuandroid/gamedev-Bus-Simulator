using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MissionFailedDialog : MonoBehaviour {
	public void Buttons(string name){
		switch (name) {
		case "Restart":
			SceneManager.LoadScene ("GamePlay");
			break;
		case "Menu":
			SceneManager.LoadScene ("NewMenu");
			break;
		case "RateUs":
			//rateus
			Application.OpenURL ("");
			break;
		}
	}
}
