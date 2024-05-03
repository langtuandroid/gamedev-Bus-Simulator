using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CameraRotateBSR : MonoBehaviour
{
    [FormerlySerializedAs("targetObject")] public Transform targetObjectBSR;
    [FormerlySerializedAs("targetOffset")] public Vector3 targetOffsetBSR;
    [FormerlySerializedAs("averageDistance")] public float averageDistanceBSR = 5.0f;
    [FormerlySerializedAs("maxDistance")] public float maxDistanceBSR = 20;
    [FormerlySerializedAs("minDistance")] public float minDistanceBSR = .6f;
    [FormerlySerializedAs("xSpeed")] public float xSpeedBSR = 200.0f;
    [FormerlySerializedAs("ySpeed")] public float ySpeedBSR = 200.0f;
    [FormerlySerializedAs("yMinLimit")] public int yMinLimitBSR = -80;
    [FormerlySerializedAs("yMaxLimit")] public int yMaxLimitBSR = 80;
    [FormerlySerializedAs("zoomSpeed")] public int zoomSpeedBSR = 40;
    [FormerlySerializedAs("panSpeed")] public float panSpeedBSR = 0.3f;
    [FormerlySerializedAs("zoomDampening")] public float zoomDampeningBSR = 5.0f;
	[FormerlySerializedAs("rotateOnOff")] public float rotateOnOffBSR = 1;

    private float xDegBSR = 0.0f;
    private float yDegBSR = 0.0f;
    private float currentDistanceBSR;
    private float desiredDistanceBSR;
    private Quaternion currentRotationBSR;
    private Quaternion desiredRotationBSR;
    private Quaternion rotationBSR;
    private Vector3 positionBSR;
	private float idleTimerBSR = 0.0f;
	private float idleSmoothBSR = 0.0f;
	
    private void Start() { InitBSR(); }
    private void OnEnable() { InitBSR(); }

    public void InitBSR()
    {
		if (!targetObjectBSR)
        {
            GameObject go = new GameObject("Cam Target");
			go.transform.position = transform.position + (transform.forward * averageDistanceBSR);
			targetObjectBSR = go.transform;
        }

		currentDistanceBSR = averageDistanceBSR;
		desiredDistanceBSR = averageDistanceBSR;
        
        positionBSR = transform.position;
        rotationBSR = transform.rotation;
        currentRotationBSR = transform.rotation;
        desiredRotationBSR = transform.rotation;
       
        xDegBSR = Vector3.Angle(Vector3.right, transform.right );
        yDegBSR = Vector3.Angle(Vector3.up, transform.up );
		positionBSR = targetObjectBSR.position - (rotationBSR * Vector3.forward * currentDistanceBSR + targetOffsetBSR);
    }

    private void LateUpdate()
    {  
        if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
        {
			desiredDistanceBSR -= Input.GetAxis("Mouse Y") * 0.02f  * zoomSpeedBSR*0.125f * Mathf.Abs(desiredDistanceBSR);
        }
        else if (Input.GetMouseButton(0) )
        {
            xDegBSR += Input.GetAxis("Mouse X") * xSpeedBSR * 0.02f;
            yDegBSR -= Input.GetAxis("Mouse Y") * ySpeedBSR * 0.02f;
            yDegBSR = ClampAngleBSR(yDegBSR, yMinLimitBSR, yMaxLimitBSR);
			
            desiredRotationBSR = Quaternion.Euler(yDegBSR, xDegBSR, 0);
            currentRotationBSR = transform.rotation;
           	rotationBSR = Quaternion.Lerp(currentRotationBSR, desiredRotationBSR, 0.02f  * zoomDampeningBSR);
        	transform.rotation = rotationBSR;
			idleTimerBSR=0;
            idleSmoothBSR=0;
       
		}else{
			idleTimerBSR+=0.02f ;
			if(idleTimerBSR > rotateOnOffBSR && rotateOnOffBSR > 0){
				idleSmoothBSR+=(0.02f +idleSmoothBSR)*0.005f;
				idleSmoothBSR = Mathf.Clamp(idleSmoothBSR, 0, 1);
				xDegBSR += xSpeedBSR * 0.001f * idleSmoothBSR;
			}

            yDegBSR = ClampAngleBSR(yDegBSR, yMinLimitBSR, yMaxLimitBSR);
            desiredRotationBSR = Quaternion.Euler(yDegBSR, xDegBSR, 0);
            currentRotationBSR = transform.rotation;
           	rotationBSR = Quaternion.Lerp(currentRotationBSR, desiredRotationBSR, 0.02f  * zoomDampeningBSR*2);
        	transform.rotation = rotationBSR;
		}
	
		desiredDistanceBSR -= Input.GetAxis("Mouse ScrollWheel") * 0.02f  * zoomSpeedBSR * Mathf.Abs(desiredDistanceBSR);
        desiredDistanceBSR = Mathf.Clamp(desiredDistanceBSR, minDistanceBSR, maxDistanceBSR);
        currentDistanceBSR = Mathf.Lerp(currentDistanceBSR, desiredDistanceBSR, 0.02f  * zoomDampeningBSR);
		positionBSR = targetObjectBSR.position - (rotationBSR * Vector3.forward * currentDistanceBSR + targetOffsetBSR);
        transform.position = positionBSR;
    }

    private static float ClampAngleBSR(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}