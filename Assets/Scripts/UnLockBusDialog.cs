using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class UnLockBusDialog : MonoBehaviour {

	public void Buttons(string name){
		switch (name) {
		case "UnLockBus":
			GameManager.Instance.isToBusUnLock = true;
			SceneManager.LoadScene ("MainMenu");
			break;
		case "Close":
			gameObject.SetActive (false);
			break;

		}
	}
}
