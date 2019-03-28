using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspactor : Editor {
    public TextureCreator creator;

    private void OnEnable() {
        creator = target as TextureCreator;
        Undo.undoRedoPerformed += RefreshCreator;
    }

    private void OnDisable() {
        Undo.undoRedoPerformed -= RefreshCreator;
    }

    private void RefreshCreator() {
        if (Application.isPlaying)
            creator.FillTexture();
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck() && Application.isPlaying) {
            (target as TextureCreator).FillTexture();
        }
    }
}
