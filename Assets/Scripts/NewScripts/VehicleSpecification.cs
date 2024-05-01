using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class VehicleSpecification : MonoBehaviour {

	public GameObject speedBar;
	public GameObject handlingBar;
	public GameObject brakesBar;

	public float speed;
	public float handling;
	public float brakes;

	void OnEnable(){
		LeanTween.scaleX (speedBar, speed, 0.25f);
		LeanTween.scaleX (handlingBar, handling, 0.25f);
		LeanTween.scaleX (brakesBar, brakes, 0.25f);
	}
}
