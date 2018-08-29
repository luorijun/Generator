using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator generator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (generator.AutoUpdate)
            {
                generator.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            generator.GenerateMap();
        }
    }
}
