 using UnityEngine;
using System.Collections;

public class DomeTextureMovement : MonoBehaviour {

	public float speed=0.1f;
	public Vector2 offset; 
	public Renderer rend;
	// Update is called once per frame
	void Update () {
		offset.x = (offset.x + speed * Time.deltaTime) % 1;
		rend.material.mainTextureOffset = offset;
	}
}
