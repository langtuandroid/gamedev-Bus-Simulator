using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class InputAxisScrollbarBSR : MonoBehaviour
    {
        [FormerlySerializedAs("axis")] public string axisBSR;

	    void Update() { }

	    public void HandleInput(float value)
        {
            CrossPlatformInputManagerBSR.SetAxisBSR(axisBSR, (value*2f) - 1f);
        }
    }
}
