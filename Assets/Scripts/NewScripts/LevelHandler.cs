using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {
	public GameObject[] passanger = new GameObject[0];
	public GameObject[] busstops = new GameObject[0];
	public GameObject[] passanger_pos = new GameObject[0];
	public GameObject[] busstops_pos = new GameObject[0];
	public string last_stop;
	public int fair;
	public Transform bus_init_pos;
	GameObject current_bus;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
