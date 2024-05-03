using UnityEngine;
using System.Collections;

[AddComponentMenu("MiniMap/Marker Group")]
[RequireComponent(typeof(RectTransform))]
public class MarkerGroupBSR : MonoBehaviour {

    public RectTransform MarkerGroupRectBSR
    {
        get
        {
            if (!_rectTransformBSR)
            {
                _rectTransformBSR = GetComponent<RectTransform>();
            }

            return _rectTransformBSR;
        }
    }

    private RectTransform _rectTransformBSR;

}
