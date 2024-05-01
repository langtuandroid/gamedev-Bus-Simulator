using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//************************//
//This class used to share the data between class
//************************//
public class GameManager
{
	private GameManager ()
	{
	}

	private static GameManager instance = null;

	public static GameManager Instance {

		get { 
			if (instance == null) {
				instance = new GameManager ();
			}
			return instance;
		}
	}
	public int totalMissions=15;
	public int mission_reward=0;
	public string mission_end_time;
	public int mission_no =0;
	public int bus_number = 0;
	public string destination_scene_name = "Level1";
	public bool is_continuou = false;
	public string bus_name = "RCCBus1";
	public int current_bus_index = 0;
	public int mission_minutes =2;
	public int mission_seconds = 30;
	public bool isFirstTimeGamePlay=true;
	public int controllerType=0;
	public bool isToBusUnLock=false;
	public int unlockBusIndex = 0;
	public bool fromFuelStation = false;
	public bool isHowToPlay=false;
	public List<GameObject> bus_stops = new List<GameObject> ();
	public bool isForGarage=false;
}
