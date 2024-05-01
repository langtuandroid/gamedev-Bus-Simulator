using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public Transform target;
	public float speed=20f;
	public Transform cam;
	public Transform defaultPos;
	// Use this for initialization
	void OnEnable () {
		cam.position = defaultPos.position;
		cam.rotation = defaultPos.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.RotateAround (target.position, Vector3.up ,Time.deltaTime * speed);
	}
}
