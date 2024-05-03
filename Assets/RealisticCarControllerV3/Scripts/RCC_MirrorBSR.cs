//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Mirror")]
public class RCC_MirrorBSR : MonoBehaviour {

	private Camera camBSR;
	private RCC_CarControllerV3 carControllerBSR;
	
	private void InvertCameraBSR () {

		camBSR = GetComponent<Camera>();
		camBSR.ResetWorldToCameraMatrix ();
		camBSR.ResetProjectionMatrix ();
		camBSR.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
		carControllerBSR = GetComponentInParent<RCC_CarControllerV3>();

	}
	
	private void OnPreRender () {
		GL.invertCulling = true;
	}
	
	private void OnPostRender () {
		GL.invertCulling = false;
	}

	private void Update(){

		if(!camBSR){
			InvertCameraBSR();
			return;
		}

		camBSR.enabled = carControllerBSR.canControl;

	}

}
