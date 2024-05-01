using UnityEngine;

// This script will zoom the main camera based on finger gestures
public class SimpleZoom2D : MonoBehaviour
{
	[Tooltip("The minimum field of view angle we want to zoom to")]
	public float Minimum = 10.0f;
	
	[Tooltip("The maximum field of view angle we want to zoom to")]
	public float Maximum = 60.0f;
	public Camera cam;
	protected virtual void LateUpdate()
	{
		// Does the main camera exist?
		if (cam!= null)
		{
			// Make sure the pinch scale is valid
			if (Lean.LeanTouch.PinchScale > 0.0f)
			{
				// Store the old size in a temp variable
				var orthographicSize = cam.orthographicSize;

				// Scale the size based on the pinch scale
				orthographicSize /= Lean.LeanTouch.PinchScale;
				
				// Clamp the size to out min/max values
				orthographicSize = Mathf.Clamp(orthographicSize, Minimum, Maximum);

				// Set the new size
				cam.orthographicSize = orthographicSize;
			}
		}
	}
}