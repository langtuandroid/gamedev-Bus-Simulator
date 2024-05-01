using UnityEngine;
using System.Collections;

public class WC15PRO : MonoBehaviour
{

	public float wavesHeight = 0.3f;
	public float speed = 0.015f;
	public float wavesMode = 0.2f;

	float buf;
	float iterator;
	int meshCounter;
	Vector3[] lastDy;

	// Use this for initialization
	void Start () {
		buf = 0;
		iterator = 0;
		meshCounter = 0;

		int i = 0;
		lastDy = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];
		while (i < GetComponent<MeshFilter>().mesh.vertices.Length) {
			lastDy[i] = new Vector3();
			i++;
		}
	}



	// Update is called once per frame
	void Update () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3 dy = new Vector3();

		while (meshCounter < vertices.Length) {
			buf = meshCounter;

			if(meshCounter % 11 != 0 && (120 - meshCounter) % 11 != 0)
			{
				if(meshCounter > 10 && meshCounter <110)
				{
					dy = new Vector3(0,Mathf.Sin((Time.deltaTime + buf + iterator)) * wavesHeight * Mathf.Cos((Time.deltaTime + buf + iterator) * wavesMode));
					vertices[meshCounter] += dy - lastDy[meshCounter];	
				}
				else
				{
					dy = new Vector3();
					vertices[meshCounter]+= dy;
				}
			}
			else
			{
				dy = new Vector3();
				vertices[meshCounter]+= dy;

			}
			lastDy[meshCounter] = dy;

			meshCounter++;
		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();	

		meshCounter = 0;

		iterator += speed;
	}

	void FixedUpdate () {
        //Uncomment if you want to move MainTex and BumpMap on your shader
		/*float offsetX = Time.time * speed;
		float offsetY = Time.time * -speed;
		renderer.material.SetTextureOffset ("_MainTex", new Vector2(offsetX,offsetY));
		renderer.material.SetTextureOffset ("_BumpMap", new Vector2(offsetX,offsetY));*/
	}
}
