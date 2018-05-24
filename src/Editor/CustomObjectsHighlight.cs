using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[InitializeOnLoad]
public class CustomObjectsHighlight
{
    private static Vector2 offset = new Vector2(0, 2);

    public static void HierarchyHighlighter()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem_CB;
    }

    public static void HierarchyWindowItem_CB(int instanceID, Rect selectionRect)
    {
        Debug.Log("Changing hierarchy colors");
        Color textColor = Color.green;
        Color backgroundColor = new Color(.76f, .76f, .76f);

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            if (Selection.instanceIDs.Contains(instanceID))
            {
                textColor = Color.white;
                backgroundColor = new Color(0.24f, 0.48f, 0.90f);

            }

            Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
            EditorGUI.DrawRect(selectionRect, backgroundColor);
            EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = textColor},
                fontStyle = FontStyle.Bold
            });
        }
    }
}
