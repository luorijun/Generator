using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainManager manager = (TerrainManager)target;

        if (DrawDefaultInspector())
        {
            if (manager.autoUpdate)
            {
                manager.Generate();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            manager.Generate();
        }
    }
}