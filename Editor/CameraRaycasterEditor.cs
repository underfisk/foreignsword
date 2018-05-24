using UnityEditor;

[CustomEditor (typeof(CameraRaycaster))]
public class CameraRaycasterEditor : Editor
{
    bool isLayerPrioritiesUnfolded = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); //Serialize cameraRaycaster instance

        isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities");
        if (isLayerPrioritiesUnfolded)
        {
            EditorGUI.indentLevel++;
            {
                BindArraySize();
                BindArrayElements();
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties(); //deserialize back to cameraRaycaster (and create undo point)
    }

    protected void BindArraySize()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);
        if (requiredArraySize != currentArraySize)
        {
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

    protected void BindArrayElements()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        for(int i = 0; i < currentArraySize; i++)
        {
            var prop = serializedObject.FindProperty($"layerPriorities.Array.data[{i}]");
            prop.intValue = EditorGUILayout.LayerField($"Layer {i}",prop.intValue);
        }
    }
}
