using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathController))]
public class DemoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PathController pathController = (PathController)target;
        if (GUILayout.Button("Update Paths"))
        {
            pathController.updatePathTiles();
        }
        if(GUILayout.Button("Save Paths"))
        {
            pathController.savePaths();
        }
        if (GUILayout.Button("Clear Paths"))
        {
            pathController.clearPaths();
        }
    }
}
