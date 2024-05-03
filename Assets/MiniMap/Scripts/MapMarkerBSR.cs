using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("MiniMap/Map marker")]
public class MapMarkerBSR : MonoBehaviour
{

    #region Public

    /* Sprite that will be shown on the map
     */
    [FormerlySerializedAs("markerSprite")] public Sprite markerSpriteBSR;

    /* Size of the marker on the map (width & height)
     */
    [FormerlySerializedAs("markerSize")] public float markerSizeBSR = 6.5f;

    /* Enables or disables this marker on the map
     */
    [FormerlySerializedAs("isActive")] public bool isActiveBSR = true;

    public Image MarkerImageBSR
    {
        get
        {
            return markerImageBSR;
        }
    }

    #endregion

    #region Private

    private Image markerImageBSR;

    #endregion

    #region Unity methods

    private void Start () {
        if (!markerSpriteBSR)
        {
            Debug.LogError(" Please, specify the marker sprite.");
        }
        GameObject markerImageObject = new GameObject("Marker");
        markerImageObject.AddComponent<Image>();
        MapCanvasControllerBSR controller = MapCanvasControllerBSR.InstanceBSR;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
		markerImageObject.transform.SetParent(controller.MarkerGroupBSR.MarkerGroupRectBSR);
		markerImageBSR = markerImageObject.GetComponent<Image>();
		markerImageBSR.sprite = markerSpriteBSR;
		markerImageBSR.rectTransform.localPosition = Vector3.zero;
		markerImageBSR.rectTransform.localScale = Vector3.one*2;
		markerImageBSR.rectTransform.sizeDelta = new Vector2(markerSizeBSR, markerSizeBSR);
		markerImageBSR.gameObject.SetActive(false);
	}


	private void Update () {
        MapCanvasControllerBSR controller = MapCanvasControllerBSR.InstanceBSR;
        if (!controller)
        {
            return;
        }
        MapCanvasControllerBSR.InstanceBSR.checkInBSR(this);
        markerImageBSR.rectTransform.rotation = Quaternion.identity;
	}

    private void OnDestroy()
    {
        if (markerImageBSR)
        {
            Destroy(markerImageBSR.gameObject);
        }
    }

    #endregion

    #region Custom methods

    public void showBSR()
    {
        markerImageBSR.gameObject.SetActive(true);
    }

    public void hideBSR()
    {
        markerImageBSR.gameObject.SetActive(false);
    }

    public bool isVisibleBSR()
    {
        return markerImageBSR.gameObject.activeSelf;
    }

    public Vector3 getPositionBSR()
    {
        return gameObject.transform.position;
    }

    public void setLocalPosBSR(Vector3 pos)
    {
        markerImageBSR.rectTransform.localPosition = pos;

    }

    public void setOpacityBSR(float opacity)
    {
        markerImageBSR.color = new Color(1.0f, 1.0f, 1.0f, opacity);
    }

    #endregion
}
