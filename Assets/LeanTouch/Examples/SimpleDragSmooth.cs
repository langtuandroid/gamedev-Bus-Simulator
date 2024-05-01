using UnityEngine;

// This script allows you to drag this GameObject using any finger, as long it has a collider
public class SimpleDragSmooth : MonoBehaviour
{
//	[Tooltip("This stores the layers we want the raycast to hit (make sure this GameObject's layer is included!)")]
//	public LayerMask LayerMask = UnityEngine.Physics.DefaultRaycastLayers;
//	
	[Tooltip("How quickly smoothly this GameObject moves toward the target position")]
	public float Sharpness = 10.0f;

	// This stores the finger that's currently dragging this GameObject
	private Lean.LeanFinger draggingFinger;

	public Vector3 targetPosition;
	public Camera cam;
	public Vector2 xBoundry;
	public Vector2 yBoundry;
	private Vector2 xBound;
	private Vector2 yBound;
	public float defaultCamSize;
	public float camHeight=200f;

	protected virtual void OnEnable()
	{
		// Make the target position match the current position at the start
		targetPosition = transform.position;

		// Hook into the OnFingerDown event
		Lean.LeanTouch.OnFingerDown += OnFingerDown;
		
		// Hook into the OnFingerUp event
		Lean.LeanTouch.OnFingerUp += OnFingerUp;
	}
	
	protected virtual void OnDisable()
	{
		// Unhook the OnFingerDown event
		Lean.LeanTouch.OnFingerDown -= OnFingerDown;
		
		// Unhook the OnFingerUp event
		Lean.LeanTouch.OnFingerUp -= OnFingerUp;
	}
	
	protected virtual void LateUpdate()
	{
		// If there is an active finger, move this GameObject based on it
		if (draggingFinger != null)
		{
			// Does the main camera exist?
//			if (Camera.main != null)
//			{
//				// Convert this GameObject's world position into screen coordinates and store it in a temp variable
//				var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
//				
//				// Modify screen position by the finger's delta screen position
//				screenPosition += (Vector3)draggingFinger.DeltaScreenPosition;
//				

			targetPosition += new Vector3 (-1*draggingFinger.DeltaScreenPosition.x,camHeight,-1*draggingFinger.DeltaScreenPosition.y);//Camera.main.ScreenToWorldPoint(screenPosition) - transform.position;
			float orthoSize=cam.orthographicSize;
			xBound.x = xBoundry.x -(defaultCamSize-orthoSize);
			xBound.y = xBoundry.y +(defaultCamSize-orthoSize);
			yBound.x = yBoundry.x -(defaultCamSize-orthoSize);
			yBound.y = yBoundry.y +(defaultCamSize-orthoSize);

			targetPosition = new Vector3 (Mathf.Clamp(targetPosition.x,xBound.x,xBound.y),camHeight,Mathf.Clamp(targetPosition.z,yBound.x,yBound.y));//Camera.main.ScreenToWorldPoint(screenPosition) - transform.position;
			//}
		}

		// The framerate independent damping factor
		var factor = Mathf.Exp(- Sharpness * Time.deltaTime);
		
		// Dampen the current position toward the target
		transform.position = Vector3.Lerp(targetPosition, transform.position, factor);
	}
	
	public void OnFingerDown(Lean.LeanFinger finger)
	{
		// Raycast information
		//var ray = finger.GetRay();
	//	var hit = default(RaycastHit);
		
		// Was this finger pressed down on a collider?
//		if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask) == true)
//		{
//			// Was that collider this one?
//			if (hit.collider.gameObject == gameObject)
//			{
//				// Set the current finger to this one
				draggingFinger = finger;
//			}
//		}
	}
	
	public void OnFingerUp(Lean.LeanFinger finger)
	{
		// Was the current finger lifted from the screen?
		if (finger == draggingFinger)
		{
			// Unset the current finger
			draggingFinger = null;
		}
	}
}