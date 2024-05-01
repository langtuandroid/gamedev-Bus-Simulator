using UnityEngine;
using System.Collections;

public class BusPassanger : MonoBehaviour {
	public Transform target;
	[HideInInspector] public Transform bus; 
	public UnityEngine.AI.NavMeshAgent agent;
	public Animator anim;
	public string pickStop;
	public string dropStop;
	[HideInInspector] public bool isToWalk=false;
	bool isGoingToSeat=false;
	void Start(){
		isToWalk = false;
	}
	public void MovePassanger(Transform target,Transform bus,string busStopName,Transform busStopPosition){
		if (busStopName == pickStop) {
			MoveToSeat (target, bus);
		} else if (busStopName == dropStop) {
			MoveToStop (busStopPosition);
		}
	}
	//Move towards the Seat
	void MoveToSeat(Transform tempSeat,Transform tempBus){
		target = tempSeat;
		bus = tempBus;
		isGoingToSeat = true;
		agent.enabled = true;
		transform.SetParent (bus);
		isToWalk = true;
	}
	//drop yourself from bus and movetowards the bus Stop
	void MoveToStop(Transform tempTarget){
		target = tempTarget;
		anim.SetBool ("Sit", false);
		isGoingToSeat = false;
		transform.parent=null;
		agent.enabled = true;
		isToWalk = true;
	}
	// Update is called once per frame
	void Update () {
		if (isToWalk) {
			agent.SetDestination (target.position);
			anim.SetBool ("Walk", true);
			//Find Distance Between source and destination, Stop move if velocity reaches to zero or distance equals to stopping distance of agent
			if (!agent.pathPending) {
				if (agent.remainingDistance <= agent.stoppingDistance) {
					if (!agent.hasPath || agent.velocity.sqrMagnitude == 0) {
						isToWalk = false;
						anim.SetBool ("Walk", false);
						if(isGoingToSeat)
						anim.SetBool ("Sit", true);
						else
						anim.SetBool ("Idle", true);
						transform.position = target.position;
						transform.rotation = target.rotation;
						agent.enabled = false;
					}
				}
			}
				
		}
	}
}
