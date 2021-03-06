﻿using UnityEngine;
using UnityEditor;

namespace KPlugin.Editor
{
    /// <summary>
    /// Custom property drawer for drawing <c>ReadonlyAttribute</c>
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyAttributePropertyDrawer : PropertyDrawer
    {
        override public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        override public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
