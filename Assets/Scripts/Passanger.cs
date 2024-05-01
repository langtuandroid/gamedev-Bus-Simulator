using UnityEngine;
using System.Collections;

public class Passanger : MonoBehaviour
{
	public string source_stop_tag = "StopA";
	public string dest_stop_tag = "StopB";
	public int fair = 100;
	Vector3 dir;
	[HideInInspector] public bool is_move;
	Animator anim;
	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public bool is_destination = false;
	[HideInInspector]
	public bool allow_to_enter = true;
	Vector3 bus_door_pos;

	void Start ()
	{
		anim = GetComponent<Animator> ();
		anim.SetBool ("Walk", false);
		is_move = false;
	}

	void Update ()
	{
		if (is_move) {
			float dist = Vector3.Distance (gameObject.transform.position, target.position);
			dir = target.position - gameObject.transform.position;
			dir = dir.normalized;
			gameObject.transform.LookAt (new Vector3 (target.position.x, gameObject.transform.position.y, target.position.z));
			//If moving to the destination, when distance between passanger and target less than 2 then stop moving
			if (is_destination && dist > 2) {
				gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, target.position, 1.2f * Time.deltaTime);
			} else if (is_destination == false) {   //If moving towards bus door

				gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, new Vector3 (target.position.x, gameObject.transform.position.y, target.position.z), 1.2f * Time.deltaTime);
			} else if (allow_to_enter) {

				allow_to_enter = false;
				anim.SetBool ("Walk", false);
			}
		}

	}

	public void RunAnimaiton (Transform target_position, bool is_m)
	{
		source_stop_tag = "";
		target = target_position;
		target.position = new Vector3 (target_position.position.x, gameObject.transform.position.y, target_position.position.z);
		dir = target.position - gameObject.transform.position;
		dir = dir.normalized;
		is_move = is_m;
		anim.SetBool ("Walk", true);

	}

}
