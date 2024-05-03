using System;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityStandardAssets.CrossPlatformInput
{
    // helps with managing tilt input on mobile devices
    public class TiltInputBSR : MonoBehaviour
    {
        // options for the various orientations
        public enum AxisOptions
        {
            ForwardAxis,
            SidewaysAxis,
        }


        [Serializable]
        public class AxisMappingBSR
        {
            public enum MappingType
            {
                NamedAxis,
                MousePositionX,
                MousePositionY,
                MousePositionZ
            };


            [FormerlySerializedAs("type")] public MappingType typeBSR;
            [FormerlySerializedAs("axisName")] public string axisNameBSR;
        }


        [FormerlySerializedAs("mapping")] public AxisMappingBSR mappingBSR;
        [FormerlySerializedAs("tiltAroundAxis")] public AxisOptions tiltAroundAxisBSR = AxisOptions.ForwardAxis;
        [FormerlySerializedAs("fullTiltAngle")] public float fullTiltAngleBSR = 25;
        [FormerlySerializedAs("centreAngleOffset")] public float centreAngleOffsetBSR = 0;


        private CrossPlatformInputManagerBSR.VirtualAxisBSR m_SteerAxisBSR;


        private void OnEnable()
        {
            if (mappingBSR.typeBSR == AxisMappingBSR.MappingType.NamedAxis)
            {
                m_SteerAxisBSR = new CrossPlatformInputManagerBSR.VirtualAxisBSR(mappingBSR.axisNameBSR);
                CrossPlatformInputManagerBSR.RegisterVirtualAxisBSR(m_SteerAxisBSR);
            }
        }


        private void Update()
        {
            float angle = 0;
            if (Input.acceleration != Vector3.zero)
            {
                switch (tiltAroundAxisBSR)
                {
                    case AxisOptions.ForwardAxis:
                        angle = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y)*Mathf.Rad2Deg +
                                centreAngleOffsetBSR;
                        break;
                    case AxisOptions.SidewaysAxis:
                        angle = Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y)*Mathf.Rad2Deg +
                                centreAngleOffsetBSR;
                        break;
                }
            }

            float axisValue = Mathf.InverseLerp(-fullTiltAngleBSR, fullTiltAngleBSR, angle)*2 - 1;
            switch (mappingBSR.typeBSR)
            {
                case AxisMappingBSR.MappingType.NamedAxis:
                    m_SteerAxisBSR.UpdateBSR(axisValue);
                    break;
                case AxisMappingBSR.MappingType.MousePositionX:
                    CrossPlatformInputManagerBSR.SetVirtualMousePositionXBSR(axisValue*Screen.width);
                    break;
                case AxisMappingBSR.MappingType.MousePositionY:
                    CrossPlatformInputManagerBSR.SetVirtualMousePositionYBSR(axisValue*Screen.width);
                    break;
                case AxisMappingBSR.MappingType.MousePositionZ:
                    CrossPlatformInputManagerBSR.SetVirtualMousePositionZBSR(axisValue*Screen.width);
                    break;
            }
        }


        private void OnDisable()
        {
            m_SteerAxisBSR.RemoveBSR();
        }
    }
}


namespace UnityStandardAssets.CrossPlatformInput.Inspector
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof (TiltInputBSR.AxisMappingBSR))]
    public class TiltInputAxisStylePropertyDrawerBSR : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float x = position.x;
            float y = position.y;
            float inspectorWidth = position.width;

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var props = new[] {"type", "axisName"};
            var widths = new[] {.4f, .6f};
            if (property.FindPropertyRelative("type").enumValueIndex > 0)
            {
                // hide name if not a named axis
                props = new[] {"type"};
                widths = new[] {1f};
            }
            const float lineHeight = 18;
            for (int n = 0; n < props.Length; ++n)
            {
                float w = widths[n]*inspectorWidth;

                // Calculate rects
                Rect rect = new Rect(x, y, w, lineHeight);
                x += w;

                EditorGUI.PropertyField(rect, property.FindPropertyRelative(props[n]), GUIContent.none);
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
#endif
}
