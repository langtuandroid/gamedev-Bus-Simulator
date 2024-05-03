using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
    public class StandaloneInput : VirtualInputBSR
    {
        public override float GetAxisBSR(string name, bool raw)
        {
            return raw ? Input.GetAxisRaw(name) : Input.GetAxis(name);
        }


        public override bool GetButtonBSR(string name)
        {
            return Input.GetButton(name);
        }


        public override bool GetButtonDownBSR(string name)
        {
            return Input.GetButtonDown(name);
        }


        public override bool GetButtonUpBSR(string name)
        {
            return Input.GetButtonUp(name);
        }


        public override void SetButtonDownBSR(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetButtonUpBSR(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisPositiveBSR(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisNegativeBSR(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisZeroBSR(string name)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisBSR(string name, float value)
        {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override Vector3 MousePositionBSR()
        {
            return Input.mousePosition;
        }
    }
}