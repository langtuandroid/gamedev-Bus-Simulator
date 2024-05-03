using UnityEngine;
using UnityEngine.Serialization;

namespace UnityStandardAssets.ImageEffects{
public sealed class CameraFocusBSR : MonoBehaviour
{
	[FormerlySerializedAs("m_camera")] [SerializeField]
	private Camera m_cameraBSR;

	[FormerlySerializedAs("m_depthOfField")] [SerializeField]
	private DepthOfField m_depthOfFieldBSR;

	[FormerlySerializedAs("m_focusDelay")] [SerializeField]
	private float m_focusDelayBSR;

	#region Unity core events.
	private void Update()
	{
		Transform cameraTransform = m_cameraBSR.transform;

		RaycastHit hitInfo;
		if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo))
		{
			m_depthOfFieldBSR.focalLength = Mathf.Lerp(m_depthOfFieldBSR.focalLength, hitInfo.distance, Time.deltaTime * m_focusDelayBSR);
		}
	}
	#endregion //Unity core events.

	#region Class functions.
	#endregion //Class functions.
}
}
