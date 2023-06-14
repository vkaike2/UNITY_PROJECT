using System;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class CustomHierarchy : MonoBehaviour
{
    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI -= HierarchyHighlight_OnGUI;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyHighlight_OnGUI;
    }

    private static void HierarchyHighlight_OnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        // separator rule
        if(gameObject != null && gameObject.name.StartsWith("---"))
        {
            EditorGUI.DrawRect(selectionRect, Color.black);
            EditorGUI.LabelField(selectionRect, gameObject.name, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = Color.white },
                fontStyle = FontStyle.Bold
            });
        }
    }
}
