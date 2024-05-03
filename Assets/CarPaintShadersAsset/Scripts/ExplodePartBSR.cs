using System;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class ExplodePartBSR : MonoBehaviour
{
	[Serializable]
	public class TransformParamsBSR
	{
		[FormerlySerializedAs("Position")] public Vector3 PositionBSR;
		[FormerlySerializedAs("Rotation")] public Quaternion RotationBSR;
		[FormerlySerializedAs("Scale")] public Vector3 ScaleBSR;
	}

	public TransformParamsBSR FinalTransformBSR
	{
		get
		{
			return m_finalTransformBSR;
		}

		set
		{
			m_finalTransformBSR = value;
		}
	}

	public TransformParamsBSR InitialTransformBSR
	{
		get
		{
			return m_initialTransformBSR;
		}

		set
		{
			m_initialTransformBSR = value;
		}
	}

	[FormerlySerializedAs("m_finalTransform")] [SerializeField]
	private TransformParamsBSR m_finalTransformBSR;

	[FormerlySerializedAs("m_initialTransform")] [SerializeField]
	private TransformParamsBSR m_initialTransformBSR;

	#region Unity core events.
	#endregion //Unity core events.

	#region Class functions.
	public void ApplyExplodeOffsetBSR(float offset)
	{
		Transform cachedTransform = transform;
		offset = Mathf.Clamp01(offset);

		cachedTransform.localPosition = Vector3.Lerp(InitialTransformBSR.PositionBSR, FinalTransformBSR.PositionBSR, offset);
		cachedTransform.localRotation = Quaternion.Slerp(InitialTransformBSR.RotationBSR, FinalTransformBSR.RotationBSR, offset);
		cachedTransform.localScale = Vector3.Lerp(InitialTransformBSR.ScaleBSR, FinalTransformBSR.ScaleBSR, offset);
	}
	#endregion //Class functions.
}
