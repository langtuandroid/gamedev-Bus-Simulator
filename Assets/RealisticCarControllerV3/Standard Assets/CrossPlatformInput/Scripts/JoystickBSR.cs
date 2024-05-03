using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class JoystickBSR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		[FormerlySerializedAs("MovementRange")] public int MovementRangeBSR = 100;
		[FormerlySerializedAs("axesToUse")] public AxisOption axesToUseBSR = AxisOption.Both; // The options for the axes that the still will use
		[FormerlySerializedAs("horizontalAxisName")] public string horizontalAxisNameBSR = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		[FormerlySerializedAs("verticalAxisName")] public string verticalAxisNameBSR = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPosBSR;
		bool m_UseXBSR; // Toggle for using the x axis
		bool m_UseYBSR; // Toggle for using the Y axis
		CrossPlatformInputManagerBSR.VirtualAxisBSR m_HorizontalVirtualAxisBSR; // Reference to the joystick in the cross platform input
		CrossPlatformInputManagerBSR.VirtualAxisBSR m_VerticalVirtualAxisBSR; // Reference to the joystick in the cross platform input

		private void OnEnable()
		{
			CreateVirtualAxesBSR();
		}

        private void Start()
        {
            m_StartPosBSR = transform.position;
        }

		private void UpdateVirtualAxesBSR(Vector3 value)
		{
			var delta = m_StartPosBSR - value;
			delta.y = -delta.y;
			delta /= MovementRangeBSR;
			if (m_UseXBSR)
			{
				m_HorizontalVirtualAxisBSR.UpdateBSR(-delta.x);
			}

			if (m_UseYBSR)
			{
				m_VerticalVirtualAxisBSR.UpdateBSR(delta.y);
			}
		}

		private void CreateVirtualAxesBSR()
		{
			// set axes to use
			m_UseXBSR = (axesToUseBSR == AxisOption.Both || axesToUseBSR == AxisOption.OnlyHorizontal);
			m_UseYBSR = (axesToUseBSR == AxisOption.Both || axesToUseBSR == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseXBSR)
			{
				m_HorizontalVirtualAxisBSR = new CrossPlatformInputManagerBSR.VirtualAxisBSR(horizontalAxisNameBSR);
				CrossPlatformInputManagerBSR.RegisterVirtualAxisBSR(m_HorizontalVirtualAxisBSR);
			}
			if (m_UseYBSR)
			{
				m_VerticalVirtualAxisBSR = new CrossPlatformInputManagerBSR.VirtualAxisBSR(verticalAxisNameBSR);
				CrossPlatformInputManagerBSR.RegisterVirtualAxisBSR(m_VerticalVirtualAxisBSR);
			}
		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;

			if (m_UseXBSR)
			{
				int delta = (int)(data.position.x - m_StartPosBSR.x);
				delta = Mathf.Clamp(delta, - MovementRangeBSR, MovementRangeBSR);
				newPos.x = delta;
			}

			if (m_UseYBSR)
			{
				int delta = (int)(data.position.y - m_StartPosBSR.y);
				delta = Mathf.Clamp(delta, -MovementRangeBSR, MovementRangeBSR);
				newPos.y = delta;
			}
			transform.position = new Vector3(m_StartPosBSR.x + newPos.x, m_StartPosBSR.y + newPos.y, m_StartPosBSR.z + newPos.z);
			UpdateVirtualAxesBSR(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			transform.position = m_StartPosBSR;
			UpdateVirtualAxesBSR(m_StartPosBSR);
		}


		public void OnPointerDown(PointerEventData data) { }

		private void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseXBSR)
			{
				m_HorizontalVirtualAxisBSR.RemoveBSR();
			}
			if (m_UseYBSR)
			{
				m_VerticalVirtualAxisBSR.RemoveBSR();
			}
		}
	}
}