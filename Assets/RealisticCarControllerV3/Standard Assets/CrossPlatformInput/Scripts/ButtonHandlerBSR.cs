using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class ButtonHandlerBSR : MonoBehaviour
    {

        [FormerlySerializedAs("Name")] public string NameBSR;

        void OnEnable()
        {

        }

        public void SetDownState()
        {
            CrossPlatformInputManagerBSR.SetButtonDownBSR(NameBSR);
        }


        public void SetUpState()
        {
            CrossPlatformInputManagerBSR.SetButtonUpBSR(NameBSR);
        }


        public void SetAxisPositiveState()
        {
            CrossPlatformInputManagerBSR.SetAxisPositiveBSR(NameBSR);
        }


        public void SetAxisNeutralState()
        {
            CrossPlatformInputManagerBSR.SetAxisZeroBSR(NameBSR);
        }


        public void SetAxisNegativeState()
        {
            CrossPlatformInputManagerBSR.SetAxisNegativeBSR(NameBSR);
        }

        public void Update()
        {

        }
    }
}
