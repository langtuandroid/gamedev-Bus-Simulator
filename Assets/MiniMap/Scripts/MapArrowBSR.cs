using UnityEngine;
using System.Collections;

[AddComponentMenu("MiniMap/Map arrow")]
public class MapArrowBSR : MonoBehaviour {

    private RectTransform _arrowRectBSR;

    public RectTransform ArrowRectBSR
    {
        get
        {
            if (!_arrowRectBSR)
            {
                _arrowRectBSR = GetComponent<RectTransform>();
                if (!_arrowRectBSR)
                {
                    Debug.LogError("RectTransform not found. MapArrow script must by attached to an Image.");
                }
            }

            return _arrowRectBSR;
        }
    }

    public void rotateBSR(Quaternion quat)
    {
        ArrowRectBSR.rotation = quat;
    }

}
