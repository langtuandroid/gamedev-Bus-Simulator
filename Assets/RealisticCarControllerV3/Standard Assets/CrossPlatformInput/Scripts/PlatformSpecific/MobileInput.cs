using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
    public class MobileInput : VirtualInputBSR
    {
        private void AddButton(string name)
        {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInputManagerBSR.RegisterVirtualButtonBSR(new CrossPlatformInputManagerBSR.VirtualButtonBSR(name));
        }


        private void AddAxes(string name)
        {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInputManagerBSR.RegisterVirtualAxisBSR(new CrossPlatformInputManagerBSR.VirtualAxisBSR(name));
        }


        public override float GetAxisBSR(string name, bool raw)
        {
            if (!m_VirtualAxesBSR.ContainsKey(name))
            {
                AddAxes(name);
            }
            return m_VirtualAxesBSR[name].GetValueBSR;
        }


        public override void SetButtonDownBSR(string name)
        {
            if (!m_VirtualButtonsBSR.ContainsKey(name))
            {
                AddButton(name);
            }
            m_VirtualButtonsBSR[name].PressedBSR();
        }


        public override void SetButtonUpBSR(string name)
        {
            if (!m_VirtualButtonsBSR.ContainsKey(name))
            {
                AddButton(name);
            }
            m_VirtualButtonsBSR[name].ReleasedBSR();
        }


        public override void SetAxisPositiveBSR(string name)
        {
            if (!m_VirtualAxesBSR.ContainsKey(name))
            {
                AddAxes(name);
            }
            m_VirtualAxesBSR[name].UpdateBSR(1f);
        }


        public override void SetAxisNegativeBSR(string name)
        {
            if (!m_VirtualAxesBSR.ContainsKey(name))
            {
                AddAxes(name);
            }
            m_VirtualAxesBSR[name].UpdateBSR(-1f);
        }


        public override void SetAxisZeroBSR(string name)
        {
            if (!m_VirtualAxesBSR.ContainsKey(name))
            {
                AddAxes(name);
            }
            m_VirtualAxesBSR[name].UpdateBSR(0f);
        }


        public override void SetAxisBSR(string name, float value)
        {
            if (!m_VirtualAxesBSR.ContainsKey(name))
            {
                AddAxes(name);
            }
            m_VirtualAxesBSR[name].UpdateBSR(value);
        }


        public override bool GetButtonDownBSR(string name)
        {
            if (m_VirtualButtonsBSR.ContainsKey(name))
            {
                return m_VirtualButtonsBSR[name].GetButtonDownBSR;
            }

            AddButton(name);
            return m_VirtualButtonsBSR[name].GetButtonDownBSR;
        }


        public override bool GetButtonUpBSR(string name)
        {
            if (m_VirtualButtonsBSR.ContainsKey(name))
            {
                return m_VirtualButtonsBSR[name].GetButtonUpBSR;
            }

            AddButton(name);
            return m_VirtualButtonsBSR[name].GetButtonUpBSR;
        }


        public override bool GetButtonBSR(string name)
        {
            if (m_VirtualButtonsBSR.ContainsKey(name))
            {
                return m_VirtualButtonsBSR[name].GetButtonBSR;
            }

            AddButton(name);
            return m_VirtualButtonsBSR[name].GetButtonBSR;
        }


        public override Vector3 MousePositionBSR()
        {
            return virtualMousePositionBSR;
        }
    }
}
