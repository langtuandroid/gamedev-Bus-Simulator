using UnityEngine;
using System.Collections;

public class LargeMap : MonoBehaviour {
	public GameObject map;
	public GameObject mainCamera;
	public GameObject mapParts;
	public GameObject closeMapButton;
	public void ShowMapButton(){
		//Debug.Log ("Pressed");
		map.SetActive (true);
		closeMapButton.SetActive (true);
		mainCamera.SetActive (false);
		mapParts.SetActive (false);
	}
	public void CloseMapButton(){
		map.SetActive (false);
		mainCamera.SetActive (true);
		mapParts.SetActive (true);
		closeMapButton.SetActive (false);
	}
}
