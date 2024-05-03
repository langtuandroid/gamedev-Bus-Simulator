using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

[AddComponentMenu("MiniMap/Map canvas controller")]
[RequireComponent(typeof(RectTransform))]
public class MapCanvasControllerBSR : MonoBehaviour
{
    #region Singleton
    public static MapCanvasControllerBSR InstanceBSR
    {
        get
        {
            if (!_instanceBSR)
            {
                MapCanvasControllerBSR[] controllers = GameObject.FindObjectsOfType<MapCanvasControllerBSR>();

                if (controllers.Length != 0)
                {
                    if (controllers.Length == 1)
                    {
                        _instanceBSR = controllers[0];
                    }
                    else
                    {
                        Debug.LogError("You have more than one MapCanvasController in the scene.");
                    }
                }
                else
                {
                    Debug.LogError("You should add Map prefab to your canvas");
                }


            }
           

            return _instanceBSR;
        }
    }

    private static MapCanvasControllerBSR _instanceBSR;
    #endregion

    #region Public

    /* Transform of the player that will be shown in the center of the map
     */ 
    [FormerlySerializedAs("playerTransform")] public Transform playerTransformBSR;

    /* Distance from which the objects will be shown on the map
     * If objects are farer - they will be always on the border of the map or now shown at all
     */
    [FormerlySerializedAs("radarDistance")] public float radarDistanceBSR = 10;

    /* If true - objects out of range (radarDistance) will be hidden
     * If false - objects out of range (radarDistance) will stick to the border of the map
     * */
    [FormerlySerializedAs("hideOutOfRadius")] public bool hideOutOfRadiusBSR = true;

    /* Applied to objects that are out of range (radarDistance)
     * If true - the farer the object is, the more transparent it is
     * Used only when hideOutOfRadius == false
     * */
    [FormerlySerializedAs("useOpacity")] public bool useOpacityBSR = true;

    /* Objects that are farer than radar distance but closer than maxRadarDistance
     * will be shown on the border of the map.
     * Used only when hideOutOfRadius == false
     */
    [FormerlySerializedAs("maxRadarDistance")] public float maxRadarDistanceBSR = 10;

    /* Enables or disables the rotation of the map
     * If true - the map is rotated and the arrow is not
     * and vice versa
     */
    [FormerlySerializedAs("rotateMap")] public bool rotateMapBSR = false;

    /* Sets the scale of the radaDistance and maxRadarDistance
     */
    [FormerlySerializedAs("scale")] public float scaleBSR = 1.0f;

    /*Minimal opacity for the markers that are farther than radar distance
     */
    [FormerlySerializedAs("minimalOpacity")] public float minimalOpacityBSR = 0.3f;


    public InnerMapBSR InnerMapComponentBSR
    {
        get
        {
            return innerMapBSR;
        }
    }

    public MarkerGroupBSR MarkerGroupBSR
    {
        get
        {
            return markerGroupBSR;
        }
    }
    #endregion


    #region Private
    private RectTransform mapRectBSR;
    private InnerMapBSR innerMapBSR;
    private MapArrowBSR mapArrowBSR;
    private MarkerGroupBSR markerGroupBSR;
    private float innerMapRadiusBSR;
    #endregion

    #region Unity Methods

	[FormerlySerializedAs("miniMapCam")] public Transform miniMapCamBSR;

    private void Awake()
    {

        if (!playerTransformBSR)
        {
           // Debug.LogError("You must specify the player transform");
        }

        mapRectBSR = GetComponent<RectTransform>();

        innerMapBSR = GetComponentInChildren<InnerMapBSR>();
        if (!innerMapBSR)
        {
            Debug.LogError("InnerMap component is missing from children");
        }

        mapArrowBSR = GetComponentInChildren<MapArrowBSR>();
        if (!mapArrowBSR)
        {
            Debug.LogError("MapArrow component is missing from children");
        }

        markerGroupBSR = GetComponentInChildren<MarkerGroupBSR>();
        if (!markerGroupBSR)
        {
            Debug.LogError("MerkerGroup component is missing. It must be a child of InnerMap");
        }

        innerMapRadiusBSR = innerMapBSR.getMapRadiusBSR();

    }

	private void Update () 
    {
        if (!playerTransformBSR)
        {
            //error was already fired in Awake()
            return;
        }
		miniMapCamBSR.position = new Vector3 (playerTransformBSR.position.x,miniMapCamBSR.position.y,playerTransformBSR.position.z);
        
		if (rotateMapBSR)
        {
            mapRectBSR.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransformBSR.eulerAngles.y));
            mapArrowBSR.rotateBSR(Quaternion.identity);
        }
        else
        {
            mapArrowBSR.rotateBSR(Quaternion.Euler(new Vector3(0, 0, -playerTransformBSR.eulerAngles.y)));
        }
    }

    #endregion

    #region Custom methods

    public void checkInBSR(MapMarkerBSR marker)
    {
        if (!playerTransformBSR)
        {
            //error was already fired in Awake()
            return;
        }

        float scaledRadarDistance = radarDistanceBSR * scaleBSR;
        float scaledMaxRadarDistance = maxRadarDistanceBSR * scaleBSR;

        if (marker.isActiveBSR)
        {
            float distance = distanceToPlayerBSR(marker.getPositionBSR());
            float opacity = 1.0f;

            if (distance > scaledRadarDistance)
            {
                if (hideOutOfRadiusBSR)
                {
                    if (marker.isVisibleBSR()) 
                    { 
                        marker.hideBSR(); 
                    }
                    return;
                }
                else
                {
                    if (distance > scaledMaxRadarDistance)
                    {
                        if (marker.isVisibleBSR()) 
                        { 
                            marker.hideBSR(); 
                        }
                        return;
                    }
                    else
                    {
                        if (useOpacityBSR) 
                        {
                            float opacityRange = scaledMaxRadarDistance - scaledRadarDistance;
                            if (opacityRange <= 0)
                            {
                                Debug.LogError("Max radar distance should be bigger than radar distance");
                                return;
                            }
                            else
                            {
                                float distanceDiff = distance - scaledRadarDistance;
                                opacity = 1 - (distanceDiff / opacityRange);

                                if (opacity < minimalOpacityBSR)
                                {
                                    opacity = minimalOpacityBSR;
                                }
                            }
                        }
                        distance = scaledRadarDistance;
                    }
                }
            }

            if (!marker.isVisibleBSR())
            {
                marker.showBSR();
            }

            Vector3 posDif = marker.getPositionBSR() - playerTransformBSR.position;
            Vector3 newPos = new Vector3(posDif.x, posDif.z, 0);
            newPos.Normalize();

            float markerRadius = (marker.markerSizeBSR / 4);
            float newLen = (distance / scaledRadarDistance) * (innerMapRadiusBSR - markerRadius);

            newPos *= newLen;
            marker.setLocalPosBSR(newPos);
            marker.setOpacityBSR(opacity);
        }
        else
        {
            if (marker.isVisibleBSR())
            {
                marker.hideBSR();
            }
        }
    }

    private float distanceToPlayerBSR(Vector3 other)
    {
        
        return Vector2.Distance(new Vector2(playerTransformBSR.position.x, playerTransformBSR.position.z), new Vector2(other.x, other.z));
    }

    #endregion
}
