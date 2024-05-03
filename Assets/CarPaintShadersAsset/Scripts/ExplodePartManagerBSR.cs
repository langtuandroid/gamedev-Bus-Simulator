using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class ExplodePartManagerBSR : MonoBehaviour
{
	private enum AnimationDirection
	{
		Forward,
		Reverse,
	}

	[FormerlySerializedAs("m_animationTime")] [SerializeField]
	private float m_animationTimeBSR;

	[FormerlySerializedAs("m_key")] [SerializeField]
	private KeyCode m_keyBSR;

	private float m_animationProgressBSR;
	private AnimationDirection m_currentDirectionBSR;
	private ExplodePartBSR[] m_explodePartsBSR;

	#region Unity core events.
	private void Awake()
	{
		m_animationProgressBSR = 0f;
		m_currentDirectionBSR = AnimationDirection.Reverse;
		m_explodePartsBSR = GetComponentsInChildren<ExplodePartBSR>(true);
	}

	private void Update()
	{
		if (Input.GetKeyUp(m_keyBSR))
		{
			m_currentDirectionBSR = m_currentDirectionBSR == AnimationDirection.Forward ?
				AnimationDirection.Reverse : AnimationDirection.Forward;

			StopCoroutine("PlayExplodeAnimationBSR");
			StartCoroutine("PlayExplodeAnimationBSR");
		}
	}
	#endregion //Unity core events.

	#region Class functions.
	private void ApplyExplodeOffsetBSR(float offset)
	{
		foreach (ExplodePartBSR part in m_explodePartsBSR)
		{
			part.ApplyExplodeOffsetBSR(offset);
		}
	}

	private float EaseInOutSineBSR(float value)
	{
		return -0.5f * (Mathf.Cos(Mathf.PI * value) - 1f);
	}

	private IEnumerator PlayExplodeAnimationBSR()
	{
		do
		{
			ApplyExplodeOffsetBSR(EaseInOutSineBSR(m_animationProgressBSR / m_animationTimeBSR));
			yield return null;

			m_animationProgressBSR += m_currentDirectionBSR == AnimationDirection.Forward ? Time.deltaTime : -Time.deltaTime;
		}
		while (m_animationProgressBSR > 0f && m_animationProgressBSR < m_animationTimeBSR);

		m_animationProgressBSR = Mathf.Clamp(m_animationProgressBSR, 0f, m_animationTimeBSR);
		ApplyExplodeOffsetBSR(EaseInOutSineBSR(m_animationProgressBSR / m_animationTimeBSR));
	}
	#endregion //Class functions.
}
