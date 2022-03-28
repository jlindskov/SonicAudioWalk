using System;
using FMODUnity;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioSurfaceType))][CanEditMultipleObjects]
public class AudioSurfaceTypeEditor : Editor
{
    SerializedProperty param;

    SerializedProperty data1, data2;

    [SerializeField] EditorParamRef editorParamRef;
    

    void OnEnable()
    {
        param = serializedObject.FindProperty("parameter");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
EditorGUILayout.Space();
        var oldParam = param.stringValue;
        EditorGUILayout.PropertyField(param, new GUIContent("Parameter"));

        if (!String.IsNullOrEmpty(param.stringValue))
        {
            if (!editorParamRef || param.stringValue != oldParam)
            {
                editorParamRef = EventManager.ParamFromPath(param.stringValue);
            }
            
        }

        serializedObject.ApplyModifiedProperties();
    }
}