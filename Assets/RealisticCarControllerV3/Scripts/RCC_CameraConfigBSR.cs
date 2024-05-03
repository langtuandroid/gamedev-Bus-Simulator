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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Auto Camera Config")]
public class RCC_CameraConfigBSR : MonoBehaviour {

	[FormerlySerializedAs("automatic")] public bool automaticBSR = true;
	private Bounds combinedBoundsBSR;

	[FormerlySerializedAs("distance")] public float distanceBSR = 10f;
	[FormerlySerializedAs("height")] public float heightBSR = 5f;

	private void Awake(){

		if(automaticBSR){

			Quaternion orgRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			distanceBSR = MaxBoundsExtentBSR(transform) * 2.5f;
			heightBSR = MaxBoundsExtentBSR(transform) * .5f;

			transform.rotation = orgRotation;



		}

	}

	public void SetCameraSettingsBSR () {

		RCC_CameraBSR cam = GameObject.FindObjectOfType<RCC_CameraBSR>();
		 
		if(!cam)
			return;
			
		cam.distanceBSR = distanceBSR;
		cam.heightBSR = heightBSR;

	}

	public static float MaxBoundsExtentBSR(Transform obj){
		// get the maximum bounds extent of object, including all child renderers,
		// but excluding particles and trails, for FOV zooming effect.

		var renderers = obj.GetComponentsInChildren<Renderer>();

		Bounds bounds = new Bounds();
		bool initBounds = false;
		foreach (Renderer r in renderers)
		{
			if (!((r is TrailRenderer) ||  (r is ParticleSystemRenderer)))
			{
				if (!initBounds)
				{
					initBounds = true;
					bounds = r.bounds;
				}
				else
				{
					bounds.Encapsulate(r.bounds);
				}
			}
		}
		float max = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
		return max;
	}

}
