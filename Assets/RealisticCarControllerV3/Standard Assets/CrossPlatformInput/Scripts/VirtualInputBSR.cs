using System;
using System.Collections.Generic;
using UnityEngine;


namespace UnityStandardAssets.CrossPlatformInput
{
    public abstract class VirtualInputBSR
    {
        public Vector3 virtualMousePositionBSR { get; private set; }
        
        
        protected Dictionary<string, CrossPlatformInputManagerBSR.VirtualAxisBSR> m_VirtualAxesBSR =
            new Dictionary<string, CrossPlatformInputManagerBSR.VirtualAxisBSR>();
            // Dictionary to store the name relating to the virtual axes
        protected Dictionary<string, CrossPlatformInputManagerBSR.VirtualButtonBSR> m_VirtualButtonsBSR =
            new Dictionary<string, CrossPlatformInputManagerBSR.VirtualButtonBSR>();
        protected List<string> m_AlwaysUseVirtualBSR = new List<string>();
            // list of the axis and button names that have been flagged to always use a virtual axis or button
        

        public bool AxisExistsBSR(string name)
        {
            return m_VirtualAxesBSR.ContainsKey(name);
        }

        public bool ButtonExistsBSR(string name)
        {
            return m_VirtualButtonsBSR.ContainsKey(name);
        }


        public void RegisterVirtualAxisBSR(CrossPlatformInputManagerBSR.VirtualAxisBSR axis)
        {
            // check if we already have an axis with that name and log and error if we do
            if (m_VirtualAxesBSR.ContainsKey(axis.nameBSR))
            {
                Debug.LogError("There is already a virtual axis named " + axis.nameBSR + " registered.");
            }
            else
            {
                // add any new axes
                m_VirtualAxesBSR.Add(axis.nameBSR, axis);

                // if we dont want to match with the input manager setting then revert to always using virtual
                if (!axis.matchWithInputManagerBSR)
                {
                    m_AlwaysUseVirtualBSR.Add(axis.nameBSR);
                }
            }
        }


        public void RegisterVirtualButtonBSR(CrossPlatformInputManagerBSR.VirtualButtonBSR button)
        {
            // check if already have a buttin with that name and log an error if we do
            if (m_VirtualButtonsBSR.ContainsKey(button.nameBSR))
            {
                Debug.LogError("There is already a virtual button named " + button.nameBSR + " registered.");
            }
            else
            {
                // add any new buttons
                m_VirtualButtonsBSR.Add(button.nameBSR, button);

                // if we dont want to match to the input manager then always use a virtual axis
                if (!button.matchWithInputManagerBSR)
                {
                    m_AlwaysUseVirtualBSR.Add(button.nameBSR);
                }
            }
        }


        public void UnRegisterVirtualAxisBSR(string name)
        {
            // if we have an axis with that name then remove it from our dictionary of registered axes
            if (m_VirtualAxesBSR.ContainsKey(name))
            {
                m_VirtualAxesBSR.Remove(name);
            }
        }


        public void UnRegisterVirtualButtonBSR(string name)
        {
            // if we have a button with this name then remove it from our dictionary of registered buttons
            if (m_VirtualButtonsBSR.ContainsKey(name))
            {
                m_VirtualButtonsBSR.Remove(name);
            }
        }


        // returns a reference to a named virtual axis if it exists otherwise null
        public CrossPlatformInputManagerBSR.VirtualAxisBSR VirtualAxisReferenceBSR(string name)
        {
            return m_VirtualAxesBSR.ContainsKey(name) ? m_VirtualAxesBSR[name] : null;
        }


        public void SetVirtualMousePositionXBSR(float f)
        {
            virtualMousePositionBSR = new Vector3(f, virtualMousePositionBSR.y, virtualMousePositionBSR.z);
        }


        public void SetVirtualMousePositionYBSR(float f)
        {
            virtualMousePositionBSR = new Vector3(virtualMousePositionBSR.x, f, virtualMousePositionBSR.z);
        }


        public void SetVirtualMousePositionZBSR(float f)
        {
            virtualMousePositionBSR = new Vector3(virtualMousePositionBSR.x, virtualMousePositionBSR.y, f);
        }


        public abstract float GetAxisBSR(string name, bool raw);
        
        public abstract bool GetButtonBSR(string name);
        public abstract bool GetButtonDownBSR(string name);
        public abstract bool GetButtonUpBSR(string name);

        public abstract void SetButtonDownBSR(string name);
        public abstract void SetButtonUpBSR(string name);
        public abstract void SetAxisPositiveBSR(string name);
        public abstract void SetAxisNegativeBSR(string name);
        public abstract void SetAxisZeroBSR(string name);
        public abstract void SetAxisBSR(string name, float value);
        public abstract Vector3 MousePositionBSR();
    }
}
