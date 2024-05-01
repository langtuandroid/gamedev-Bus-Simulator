using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HowToPlay : MonoBehaviour {

	public GameController gameController;

	bool followArrow=false;
	Transform guideArrow;
	Transform bus;
	Transform target;
	int count=0;
	public GameObject tapOnDoorLable;
	public GameObject tapOnDoorAgainLable;
	public GameObject followtheMapLable;
	public GameObject settingDialog;
	public Animator anim;
	public GameObject tutorialCompleteDialog;
	// Use this for initialization
	void Start () {
//		PlayerPrefs.DeleteAll ();

		GameManager.Instance.isHowToPlay = true;
	}
	public void EnableArrow(){
		bus = gameController.bus.transform;
		guideArrow = bus.Find ("GuideArrow");
		guideArrow.gameObject.SetActive(true);
		target= gameController.levels [0].transform.GetChild (0);
		Debug.Log (target.name);
		gameObject.GetComponent<Image> ().raycastTarget = false;
		followArrow = true;
	}
	public void EnableSettingDialog(){
		settingDialog.SetActive (true);
	}
	public void PlayNextAnimation(){
		anim.SetTrigger ("SecondPart");
	}
	public void Next(){
		if (count == 0 || count == 4 ) {
			tapOnDoorLable.SetActive (true);
		} else if (count == 2|| count == 6) {
			tapOnDoorAgainLable.SetActive (true);
		}
		else if (count == 3||count==7) {
			tapOnDoorAgainLable.SetActive (false);
			Invoke ("EnableFollowTheMapLable",0.5f);
		}
		else if (count == 1||count==5) {
			tapOnDoorLable.SetActive (false);
		}
		if (count == 3) {
			target= gameController.levels [0].transform.GetChild (1);
		}
		if (count == 7) {
			gameObject.transform.parent.gameObject.SetActive (false);
	
			GameManager.Instance.isHowToPlay = false;
			guideArrow.gameObject.SetActive (false);
		}
//		Debug.Log (count);
		count += 1;
	}
	void EnableFollowTheMapLable(){
		followtheMapLable.SetActive (true);
		Invoke ("DeActivateFollowTheMapLable",2f);
	}
	void DeActivateFollowTheMapLable(){
		followtheMapLable.SetActive (false);
	}
	public void TutorialComplete(){
		if (GameManager.Instance.mission_no < GameManager.Instance.totalMissions) {
			if (GameManager.Instance.mission_no >= PlayerPrefs.GetInt ("LevelLocked")) {
				PlayerPrefs.SetInt ("LevelLocked", (PlayerPrefs.GetInt ("LevelLocked") + 1));  		
			}
		}
		tutorialCompleteDialog.SetActive (true);
	}
	public void PauseAnimation(){
		anim.enabled = false;
	}
	public void PlayAnimation(){
		anim.enabled = true;
	}
	public void Restart(){
		GameManager.Instance.mission_no = 0;
		GameManager.Instance.bus_stops.Clear ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	// Update is called once per frame
	void Update () {
		if (followArrow) {
			guideArrow.LookAt (target);
		}
	}
}
