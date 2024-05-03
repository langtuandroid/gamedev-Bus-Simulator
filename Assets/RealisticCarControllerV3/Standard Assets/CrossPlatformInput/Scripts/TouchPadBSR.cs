using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class TouchPadBSR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// Options for which axes to use
		public enum AxisOption
		{
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}


		public enum ControlStyle
		{
			Absolute, // operates from teh center of the image
			Relative, // operates from the center of the initial touch
			Swipe, // swipe to touch touch no maintained center
		}


		[FormerlySerializedAs("axesToUse")] public AxisOption axesToUseBSR = AxisOption.Both; // The options for the axes that the still will use
		[FormerlySerializedAs("controlStyle")] public ControlStyle controlStyleBSR = ControlStyle.Absolute; // control style to use
		[FormerlySerializedAs("horizontalAxisName")] public string horizontalAxisNameBSR = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		[FormerlySerializedAs("verticalAxisName")] public string verticalAxisNameBSR = "Vertical"; // The name given to the vertical axis for the cross platform input
		[FormerlySerializedAs("Xsensitivity")] public float XsensitivityBSR = 1f;
		[FormerlySerializedAs("Ysensitivity")] public float YsensitivityBSR = 1f;

		Vector3 m_StartPosBSR;
		Vector2 m_PreviousDeltaBSR;
		Vector3 m_JoytickOutputBSR;
		bool m_UseXBSR; // Toggle for using the x axis
		bool m_UseYBSR; // Toggle for using the Y axis
		CrossPlatformInputManagerBSR.VirtualAxisBSR m_HorizontalVirtualAxisBSR; // Reference to the joystick in the cross platform input
		CrossPlatformInputManagerBSR.VirtualAxisBSR m_VerticalVirtualAxisBSR; // Reference to the joystick in the cross platform input
		bool m_DraggingBSR;
		int m_IdBSR = -1;
		Vector2 m_PreviousTouchPosBSR; // swipe style control touch


#if !UNITY_EDITOR
    private Vector3 m_Center;
    private Image m_Image;
#else
		Vector3 m_PreviousMouseBSR;
#endif

		void OnEnable()
		{
			CreateVirtualAxesBSR();
		}

        void Start()
        {
#if !UNITY_EDITOR
            m_Image = GetComponent<Image>();
            m_Center = m_Image.transform.position;
#endif
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

		private void UpdateVirtualAxesBSR(Vector3 value)
		{
			value = value.normalized;
			if (m_UseXBSR)
			{
				m_HorizontalVirtualAxisBSR.UpdateBSR(value.x);
			}

			if (m_UseYBSR)
			{
				m_VerticalVirtualAxisBSR.UpdateBSR(value.y);
			}
		}


		public void OnPointerDown(PointerEventData data)
		{
			m_DraggingBSR = true;
			m_IdBSR = data.pointerId;
#if !UNITY_EDITOR
        if (controlStyle != ControlStyle.Absolute )
            m_Center = data.position;
#endif
		}

		void Update()
		{
			if (!m_DraggingBSR)
			{
				return;
			}
			if (Input.touchCount >= m_IdBSR + 1 && m_IdBSR != -1)
			{
#if !UNITY_EDITOR

            if (controlStyle == ControlStyle.Swipe)
            {
                m_Center = m_PreviousTouchPos;
                m_PreviousTouchPos = Input.touches[m_Id].position;
            }
            Vector2 pointerDelta = new Vector2(Input.touches[m_Id].position.x - m_Center.x , Input.touches[m_Id].position.y - m_Center.y).normalized;
            pointerDelta.x *= Xsensitivity;
            pointerDelta.y *= Ysensitivity;
#else
				Vector2 pointerDelta;
				pointerDelta.x = Input.mousePosition.x - m_PreviousMouseBSR.x;
				pointerDelta.y = Input.mousePosition.y - m_PreviousMouseBSR.y;
				m_PreviousMouseBSR = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
#endif
				UpdateVirtualAxesBSR(new Vector3(pointerDelta.x, pointerDelta.y, 0));
			}
		}


		public void OnPointerUp(PointerEventData data)
		{
			m_DraggingBSR = false;
			m_IdBSR = -1;
			UpdateVirtualAxesBSR(Vector3.zero);
		}

		private void OnDisable()
		{
			if (CrossPlatformInputManagerBSR.AxisExistsBSR(horizontalAxisNameBSR))
				CrossPlatformInputManagerBSR.UnRegisterVirtualAxisBSR(horizontalAxisNameBSR);

			if (CrossPlatformInputManagerBSR.AxisExistsBSR(verticalAxisNameBSR))
				CrossPlatformInputManagerBSR.UnRegisterVirtualAxisBSR(verticalAxisNameBSR);
		}
	}
}