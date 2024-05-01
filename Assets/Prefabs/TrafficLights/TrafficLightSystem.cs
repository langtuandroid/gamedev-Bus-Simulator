using UnityEngine;
using System.Collections;

public class TrafficLightSystem : MonoBehaviour {
	public GameObject[] trafficLights=new GameObject[0];
	int count=0;
	// Use this for initialization

	void Start () {
		StartCoroutine (Lights (10));
	}
	IEnumerator Lights (int waitTime){
		yield return new WaitForSeconds (waitTime);
		if (count == 7)
			count = 0;
		else
		count += 1;
		switch (count) {
		case 0:
			trafficLights [7].SetActive (false);
			trafficLights [0].SetActive (true);
			break;

		case 1:
			trafficLights [0].SetActive (false);
			trafficLights [1].SetActive (true);
			break;
		case 2:
			trafficLights [1].SetActive (false);
			trafficLights [2].SetActive (true);
			break;
		case 3:
			trafficLights [2].SetActive (false);
			trafficLights [3].SetActive (true);
			break;
		case 4:
			trafficLights [3].SetActive (false);
			trafficLights [4].SetActive (true);
			break;
		case 5:
			trafficLights [4].SetActive (false);
			trafficLights [5].SetActive (true);
			break;
		case 6:
			trafficLights [5].SetActive (false);
			trafficLights [6].SetActive (true);
			break;
		case 7:
			trafficLights [6].SetActive (false);
			trafficLights [7].SetActive (true);
			break;
		}
		if (count % 2 != 0) {
			StartCoroutine (Lights (2));
		} else {
			StartCoroutine (Lights (10));
		}

		//lights1 [0].transform.Find ("YelloLight").gameObject.SetActive(true);	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
