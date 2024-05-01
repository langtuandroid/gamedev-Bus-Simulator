using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;


public class UnderWater : MonoBehaviour {

	//This script enables underwater effects. Attach to main camera.

	//Define variable
	public int underwaterLevel = 7;

	//The scene's default fog settings
	private bool defaultFog;  
	private Color defaultFogColor;
	private float defaultFogDensity;
	private Material defaultSkybox;
	private Material noSkybox;
///	private 
	bool justOnce=false;
	bool justOnce2=true;
	public GameObject underWaterEnvoirnment;
	public GameObject outWaterEnvoirnment;
	public GameObject waterFog;
	public ColorCorrectionCurves colorCorrectionCurves;
	void Start () {
		//Set the background color
		defaultFog = RenderSettings.fog;
		defaultFogColor = RenderSettings.fogColor;
		defaultFogDensity = RenderSettings.fogDensity;
		defaultSkybox = RenderSettings.skybox;
		Camera.main.backgroundColor = new Color(0, 0.4f, 0.7f, 1);
//		Adds.instance.LogGoogleAnalytics ();
	}

	void Update () {
		if (transform.position.y < underwaterLevel)
		{
			if(justOnce){
				
			outWaterEnvoirnment.SetActive (false);
			underWaterEnvoirnment.SetActive (true);
			Camera.main.clearFlags=CameraClearFlags.SolidColor;
			Camera.main.farClipPlane = 1000;
//				waterFog.SetActive (true);
//			colorCorrectionCurves.enabled = true;
			//globalFog.enabled = true;
//			fishEye.enabled = true;
			justOnce2 = true;
			justOnce = false;
			//RenderSettings.fog = true;
		//	RenderSettings.fogMode = FogMode.Exponential;
//			RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
//			RenderSettings.fogDensity = 0.004f;
			//RenderSettings.skybox = noSkybox;
			}
		}
		else if(justOnce2)
		{
			outWaterEnvoirnment.SetActive (true);
			underWaterEnvoirnment.SetActive (false);
			justOnce2 = false;
			justOnce = true;
//			waterFog.SetActive (false);
//			RenderSettings.fog = defaultFog;
//			RenderSettings.fogColor = defaultFogColor;
//			RenderSettings.fogDensity = defaultFogDensity;
			Camera.main.clearFlags = CameraClearFlags.Skybox;
			Camera.main.farClipPlane = 500;
//			colorCorrectionCurves.enabled = false;
//			fishEye.enabled = false;
			//RenderSettings.skybox =sky;
		}
	}
}
