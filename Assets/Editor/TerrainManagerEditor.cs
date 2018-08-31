using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LandManager))]
public class LandManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LandManager manager = (LandManager)target;

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