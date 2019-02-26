using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TEST))]
public class TESTEditor : Editor
{

    private void OnSceneGUI()
    {
        Handles.color = Color.blue;
        Handles.DrawWireCube(Vector3.zero, Vector3.one * 2);
        Handles.color = Color.red;
        Handles.DrawWireCube(serializedObject.FindProperty("r").vector3Value, Vector3.one * .05f);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
