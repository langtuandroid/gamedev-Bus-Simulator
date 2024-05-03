using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("MiniMap/Inner map")]
[RequireComponent(typeof(Image))]
public class InnerMapBSR : MonoBehaviour {

    private RectTransform _innerMapRectBSR;

    public RectTransform InnerMapRectBSR
    {
        get
        {
            if (!_innerMapRectBSR)
            {
                _innerMapRectBSR = GetComponent<RectTransform>();
            }

            return _innerMapRectBSR;
        }
    }

    public float getMapRadiusBSR()
    {
        Vector3[] arr = new Vector3[4];
        InnerMapRectBSR.GetLocalCorners(arr);

        float mapRadius;

        if (Mathf.Abs(arr[0].x) < Mathf.Abs(arr[0].y))
        {
            mapRadius = Mathf.Abs(arr[0].x);
        }
        else
        {
            mapRadius = Mathf.Abs(arr[0].y);
        }

        return mapRadius;
    }

}
