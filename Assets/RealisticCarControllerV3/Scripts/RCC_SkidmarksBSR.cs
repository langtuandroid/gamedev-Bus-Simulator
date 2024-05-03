//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Skidmarks")]
public class RCC_SkidmarksBSR : MonoBehaviour {
			
	[FormerlySerializedAs("maxMarks")] public int maxMarksBSR = 1024;			// Maximum number of marks total handled by one instance of the script.
	[FormerlySerializedAs("markWidth")] public float markWidthBSR = 0.275f;		// The width of the skidmarks. Should match the width of the wheel that it is used for. In meters.
	[FormerlySerializedAs("groundOffset")] public float groundOffsetBSR = 0.02f;	// The distance the skidmarks is places above the surface it is placed upon. In meters.
	[FormerlySerializedAs("minDistance")] public float minDistanceBSR = 0.1f;		// The minimum distance between two marks places next to each other. 
	
	private int indexShiftBSR;
	private int numMarksBSR = 0;
	
	// Variables for each mark created. Needed to generate the correct mesh.
	class markSectionBSR{
		public Vector3 posBSR = Vector3.zero;
		public Vector3 normalBSR = Vector3.zero;
		public Vector4 tangentBSR = Vector4.zero;
		public Vector3 poslBSR = Vector3.zero;
		public Vector3 posrBSR = Vector3.zero;
		public float intensityBSR = 0.0f;
		public int lastIndexBSR = 0;
	};
	
	private markSectionBSR[] skidmarksBSR;
	
	private bool  updatedBSR = false;
	
	//check if at the origin or not and jump to it if not
	private void  Start (){
		if (transform.position != Vector3.zero)
			transform.position = Vector3.zero;
	}
	
	// Initiallizes the array holding the skidmark sections.
	private void  Awake (){
		skidmarksBSR = new markSectionBSR[maxMarksBSR];
		for(int i= 0; i < maxMarksBSR; i++)
			skidmarksBSR[i]=new markSectionBSR();
		if(GetComponent<MeshFilter>().mesh == null)
			GetComponent<MeshFilter>().mesh = new Mesh();
	}
	
	// Function called by the wheels that is skidding. Gathers all the information needed to
	// create the mesh later. Sets the intensity of the skidmark section b setting the alpha
	// of the vertex color.
	public int AddSkidMarkBSR ( Vector3 pos ,   Vector3 normal ,   float intensity ,   int lastIndex  ){

		if(intensity > 1)
			intensity = 1.0f;
		if(intensity < 0)
			return -1;

		markSectionBSR curr = skidmarksBSR[numMarksBSR % maxMarksBSR];
		curr.posBSR = pos + normal * groundOffsetBSR;
		curr.normalBSR = normal;
		curr.intensityBSR = intensity;
		curr.lastIndexBSR = lastIndex;
		
		if(lastIndex != -1)
		{
			markSectionBSR last = skidmarksBSR[lastIndex % maxMarksBSR];
			Vector3 dir = (curr.posBSR - last.posBSR);
			Vector3 xDir = Vector3.Cross(dir,normal).normalized;
			
			curr.poslBSR = curr.posBSR + xDir * markWidthBSR * 0.5f;
			curr.posrBSR = curr.posBSR - xDir * markWidthBSR * 0.5f;
			curr.tangentBSR = new Vector4(xDir.x, xDir.y, xDir.z, 1);
			
			if(last.lastIndexBSR == -1)
			{
				last.tangentBSR = curr.tangentBSR;
				last.poslBSR = curr.posBSR + xDir * markWidthBSR * 0.5f;
				last.posrBSR = curr.posBSR - xDir * markWidthBSR * 0.5f;
			}
		}
		numMarksBSR++;
		updatedBSR = true;
		return numMarksBSR -1;

	}
	
	// If the mesh needs to be updated, i.e. a new section has been added,
	// the current mesh is removed, and a new mesh for the skidmarks is generated.
	private void  LateUpdate (){
		if(!updatedBSR)
		{
			return;
		}
		updatedBSR = false;
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		int segmentCount = 0;
		for(int j = 0; j < numMarksBSR && j < maxMarksBSR; j++)
			if(skidmarksBSR[j].lastIndexBSR != -1 && skidmarksBSR[j].lastIndexBSR > numMarksBSR - maxMarksBSR)
				segmentCount++;
		
		Vector3[] vertices = new Vector3[segmentCount * 4];
		Vector3[] normals = new Vector3[segmentCount * 4];
		Vector4[] tangents = new Vector4[segmentCount * 4];
		Color[] colors = new Color[segmentCount * 4];
		Vector2[] uvs = new Vector2[segmentCount * 4];
		int[] triangles = new int[segmentCount * 6];
		segmentCount = 0;
		for(int i = 0; i < numMarksBSR && i < maxMarksBSR; i++)
			if(skidmarksBSR[i].lastIndexBSR != -1 && skidmarksBSR[i].lastIndexBSR > numMarksBSR - maxMarksBSR)
		{
			markSectionBSR curr = skidmarksBSR[i];
			markSectionBSR last = skidmarksBSR[curr.lastIndexBSR % maxMarksBSR];
			vertices[segmentCount * 4 + 0] = last.poslBSR;
			vertices[segmentCount * 4 + 1] = last.posrBSR;
			vertices[segmentCount * 4 + 2] = curr.poslBSR;
			vertices[segmentCount * 4 + 3] = curr.posrBSR;
			
			normals[segmentCount * 4 + 0] = last.normalBSR;
			normals[segmentCount * 4 + 1] = last.normalBSR;
			normals[segmentCount * 4 + 2] = curr.normalBSR;
			normals[segmentCount * 4 + 3] = curr.normalBSR;
			
			tangents[segmentCount * 4 + 0] = last.tangentBSR;
			tangents[segmentCount * 4 + 1] = last.tangentBSR;
			tangents[segmentCount * 4 + 2] = curr.tangentBSR;
			tangents[segmentCount * 4 + 3] = curr.tangentBSR;
			
			colors[segmentCount * 4 + 0]=new Color(0, 0, 0, last.intensityBSR);
			colors[segmentCount * 4 + 1]=new Color(0, 0, 0, last.intensityBSR);
			colors[segmentCount * 4 + 2]=new Color(0, 0, 0, curr.intensityBSR);
			colors[segmentCount * 4 + 3]=new Color(0, 0, 0, curr.intensityBSR);
			
			uvs[segmentCount * 4 + 0] = new Vector2(0, 0);
			uvs[segmentCount * 4 + 1] = new Vector2(1, 0);
			uvs[segmentCount * 4 + 2] = new Vector2(0, 1);
			uvs[segmentCount * 4 + 3] = new Vector2(1, 1);
			
			triangles[segmentCount * 6 + 0] = segmentCount * 4 + 0;
			triangles[segmentCount * 6 + 2] = segmentCount * 4 + 1;
			triangles[segmentCount * 6 + 1] = segmentCount * 4 + 2;
			
			triangles[segmentCount * 6 + 3] = segmentCount * 4 + 2;
			triangles[segmentCount * 6 + 5] = segmentCount * 4 + 1;
			triangles[segmentCount * 6 + 4] = segmentCount * 4 + 3;
			segmentCount++;			
		}
		mesh.vertices=vertices;
		mesh.normals=normals;
		mesh.tangents=tangents;
		mesh.triangles=triangles;
		mesh.colors=colors;
		mesh.uv=uvs;
	}
}