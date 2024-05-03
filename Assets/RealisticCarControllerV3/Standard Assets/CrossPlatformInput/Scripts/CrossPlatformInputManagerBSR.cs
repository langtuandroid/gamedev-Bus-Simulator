using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;

namespace UnityStandardAssets.CrossPlatformInput
{
	public static class CrossPlatformInputManagerBSR
	{
		public enum ActiveInputMethod
		{
			Hardware,
			Touch
		}


		private static VirtualInputBSR activeInputBSR;

		private static VirtualInputBSR s_TouchInputBSR;
		private static VirtualInputBSR s_HardwareInputBSR;


		static CrossPlatformInputManagerBSR()
		{
			s_TouchInputBSR = new MobileInput();
			s_HardwareInputBSR = new StandaloneInput();
#if MOBILE_INPUT
            activeInputBSR = s_TouchInputBSR;
#else
			activeInput = s_HardwareInput;
#endif
		}

		public static void SwitchActiveInputMethodBSR(ActiveInputMethod activeInputMethod)
		{
			switch (activeInputMethod)
			{
				case ActiveInputMethod.Hardware:
					activeInputBSR = s_HardwareInputBSR;
					break;

				case ActiveInputMethod.Touch:
					activeInputBSR = s_TouchInputBSR;
					break;
			}
		}

		public static bool AxisExistsBSR(string name)
		{
			return activeInputBSR.AxisExistsBSR(name);
		}

		public static bool ButtonExistsBSR(string name)
		{
			return activeInputBSR.ButtonExistsBSR(name);
		}

		public static void RegisterVirtualAxisBSR(VirtualAxisBSR axis)
		{
			activeInputBSR.RegisterVirtualAxisBSR(axis);
		}


		public static void RegisterVirtualButtonBSR(VirtualButtonBSR button)
		{
			activeInputBSR.RegisterVirtualButtonBSR(button);
		}


		public static void UnRegisterVirtualAxisBSR(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			activeInputBSR.UnRegisterVirtualAxisBSR(name);
		}


		public static void UnRegisterVirtualButtonBSR(string name)
		{
			activeInputBSR.UnRegisterVirtualButtonBSR(name);
		}


		// returns a reference to a named virtual axis if it exists otherwise null
		public static VirtualAxisBSR VirtualAxisReferenceBSR(string name)
		{
			return activeInputBSR.VirtualAxisReferenceBSR(name);
		}


		// returns the platform appropriate axis for the given name
		public static float GetAxisBSR(string name)
		{
			return GetAxisBSR(name, false);
		}


		public static float GetAxisRawBSR(string name)
		{
			return GetAxisBSR(name, true);
		}


		// private function handles both types of axis (raw and not raw)
		private static float GetAxisBSR(string name, bool raw)
		{
			return activeInputBSR.GetAxisBSR(name, raw);
		}


		// -- Button handling --
		public static bool GetButtonBSR(string name)
		{
			return activeInputBSR.GetButtonBSR(name);
		}


		public static bool GetButtonDownBSR(string name)
		{
			return activeInputBSR.GetButtonDownBSR(name);
		}


		public static bool GetButtonUpBSR(string name)
		{
			return activeInputBSR.GetButtonUpBSR(name);
		}


		public static void SetButtonDownBSR(string name)
		{
			activeInputBSR.SetButtonDownBSR(name);
		}


		public static void SetButtonUpBSR(string name)
		{
			activeInputBSR.SetButtonUpBSR(name);
		}


		public static void SetAxisPositiveBSR(string name)
		{
			activeInputBSR.SetAxisPositiveBSR(name);
		}


		public static void SetAxisNegativeBSR(string name)
		{
			activeInputBSR.SetAxisNegativeBSR(name);
		}


		public static void SetAxisZeroBSR(string name)
		{
			activeInputBSR.SetAxisZeroBSR(name);
		}


		public static void SetAxisBSR(string name, float value)
		{
			activeInputBSR.SetAxisBSR(name, value);
		}


		public static Vector3 mousePositionBSR
		{
			get { return activeInputBSR.MousePositionBSR(); }
		}


		public static void SetVirtualMousePositionXBSR(float f)
		{
			activeInputBSR.SetVirtualMousePositionXBSR(f);
		}


		public static void SetVirtualMousePositionYBSR(float f)
		{
			activeInputBSR.SetVirtualMousePositionYBSR(f);
		}


		public static void SetVirtualMousePositionZBSR(float f)
		{
			activeInputBSR.SetVirtualMousePositionZBSR(f);
		}


		// virtual axis and button classes - applies to mobile input
		// Can be mapped to touch joysticks, tilt, gyro, etc, depending on desired implementation.
		// Could also be implemented by other input devices - kinect, electronic sensors, etc
		public class VirtualAxisBSR
		{
			public string nameBSR { get; private set; }
			private float m_ValueBSR;
			public bool matchWithInputManagerBSR { get; private set; }


			public VirtualAxisBSR(string name)
				: this(name, true)
			{
			}


			public VirtualAxisBSR(string name, bool matchToInputSettings)
			{
				this.nameBSR = name;
				matchWithInputManagerBSR = matchToInputSettings;
			}


			// removes an axes from the cross platform input system
			public void RemoveBSR()
			{
				UnRegisterVirtualAxisBSR(nameBSR);
			}


			// a controller gameobject (eg. a virtual thumbstick) should update this class
			public void UpdateBSR(float value)
			{
				m_ValueBSR = value;
			}


			public float GetValueBSR
			{
				get { return m_ValueBSR; }
			}


			public float GetValueRawBSR
			{
				get { return m_ValueBSR; }
			}
		}

		// a controller gameobject (eg. a virtual GUI button) should call the
		// 'pressed' function of this class. Other objects can then read the
		// Get/Down/Up state of this button.
		public class VirtualButtonBSR
		{
			public string nameBSR { get; private set; }
			public bool matchWithInputManagerBSR { get; private set; }

			private int m_LastPressedFrameBSR = -5;
			private int m_ReleasedFrameBSR = -5;
			private bool m_PressedBSR;


			public VirtualButtonBSR(string name)
				: this(name, true)
			{
			}


			public VirtualButtonBSR(string name, bool matchToInputSettings)
			{
				this.nameBSR = name;
				matchWithInputManagerBSR = matchToInputSettings;
			}


			// A controller gameobject should call this function when the button is pressed down
			public void PressedBSR()
			{
				if (m_PressedBSR)
				{
					return;
				}
				m_PressedBSR = true;
				m_LastPressedFrameBSR = Time.frameCount;
			}


			// A controller gameobject should call this function when the button is released
			public void ReleasedBSR()
			{
				m_PressedBSR = false;
				m_ReleasedFrameBSR = Time.frameCount;
			}


			// the controller gameobject should call Remove when the button is destroyed or disabled
			public void RemoveBSR()
			{
				UnRegisterVirtualButtonBSR(nameBSR);
			}


			// these are the states of the button which can be read via the cross platform input system
			public bool GetButtonBSR
			{
				get { return m_PressedBSR; }
			}


			public bool GetButtonDownBSR
			{
				get
				{
					return m_LastPressedFrameBSR - Time.frameCount == -1;
				}
			}


			public bool GetButtonUpBSR
			{
				get
				{
					return (m_ReleasedFrameBSR == Time.frameCount - 1);
				}
			}
		}
	}
}
