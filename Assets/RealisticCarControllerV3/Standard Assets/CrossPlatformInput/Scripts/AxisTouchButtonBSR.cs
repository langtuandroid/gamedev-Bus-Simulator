using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class AxisTouchButtonBSR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// designed to work in a pair with another axis touch button
		// (typically with one having -1 and one having 1 axisValues)
		[FormerlySerializedAs("axisName")] public string axisNameBSR = "Horizontal"; // The name of the axis
		[FormerlySerializedAs("axisValue")] public float axisValueBSR = 1; // The axis that the value has
		[FormerlySerializedAs("responseSpeed")] public float responseSpeedBSR = 3; // The speed at which the axis touch button responds
		[FormerlySerializedAs("returnToCentreSpeed")] public float returnToCentreSpeedBSR = 3; // The speed at which the button will return to its centre

		AxisTouchButtonBSR m_PairedWithBSR; // Which button this one is paired with
		CrossPlatformInputManagerBSR.VirtualAxisBSR m_AxisBSR; // A reference to the virtual axis as it is in the cross platform input

		private void OnEnable()
		{
			if (!CrossPlatformInputManagerBSR.AxisExistsBSR(axisNameBSR))
			{
				// if the axis doesnt exist create a new one in cross platform input
				m_AxisBSR = new CrossPlatformInputManagerBSR.VirtualAxisBSR(axisNameBSR);
				CrossPlatformInputManagerBSR.RegisterVirtualAxisBSR(m_AxisBSR);
			}
			else
			{
				m_AxisBSR = CrossPlatformInputManagerBSR.VirtualAxisReferenceBSR(axisNameBSR);
			}
			FindPairedButtonBSR();
		}

		private void FindPairedButtonBSR()
		{
			// find the other button witch which this button should be paired
			// (it should have the same axisName)
			var otherAxisButtons = FindObjectsOfType(typeof(AxisTouchButtonBSR)) as AxisTouchButtonBSR[];

			if (otherAxisButtons != null)
			{
				for (int i = 0; i < otherAxisButtons.Length; i++)
				{
					if (otherAxisButtons[i].axisNameBSR == axisNameBSR && otherAxisButtons[i] != this)
					{
						m_PairedWithBSR = otherAxisButtons[i];
					}
				}
			}
		}

		private void OnDisable()
		{
			// The object is disabled so remove it from the cross platform input system
			m_AxisBSR.RemoveBSR();
		}


		public void OnPointerDown(PointerEventData data)
		{
			if (m_PairedWithBSR == null)
			{
				FindPairedButtonBSR();
			}
			// update the axis and record that the button has been pressed this frame
			m_AxisBSR.UpdateBSR(Mathf.MoveTowards(m_AxisBSR.GetValueBSR, axisValueBSR, responseSpeedBSR * Time.deltaTime));
		}


		public void OnPointerUp(PointerEventData data)
		{
			m_AxisBSR.UpdateBSR(Mathf.MoveTowards(m_AxisBSR.GetValueBSR, 0, responseSpeedBSR * Time.deltaTime));
		}
	}
}