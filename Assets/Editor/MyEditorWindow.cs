using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

public class MyEditorWindow : EditorWindow
{
    private static class Styles
    {
        public static GUIContent presetIcon = EditorGUIUtility.IconContent("Preset.Context");
        public static GUIStyle iconButton = new GUIStyle("IconButton");
    }

    Editor m_SettingEditor;
    MyWindowSettings m_SerializedSettings;

    public string someSettings
    {
        get
        {
            return EditorPrefs.GetString("MyEditorWindow_SomeSettings");
        }
        set
        {
            EditorPrefs.SetString("MyEditorWindow_SomeSettings", value);
        }
    }

    [MenuItem("Window/MyEditorWindow")]
    static void OpenWindow()
    {
        GetWindow<MyEditorWindow>();
    }

    void OnEnable()
    {
        m_SerializedSettings = ScriptableObject.CreateInstance<MyWindowSettings>();
        m_SerializedSettings.Init(this);
        m_SettingEditor = Editor.CreateEditor(m_SerializedSettings);
    }

    void OnDisable()
    {
        Object.DestroyImmediate(m_SerializedSettings);
        Object.DestroyImmediate(m_SettingEditor);
    }

    // Update is called once per frame
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("My Custom Settings", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        var buttonPosition = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, Styles.iconButton);
        if (EditorGUI.DropdownButton(buttonPosition, Styles.presetIcon, FocusType.Passive, Styles.iconButton))
        {
            var presetReceiver = ScriptableObject.CreateInstance<MySettingsReceiver>();
            presetReceiver.Init(m_SerializedSettings, this);
            PresetSelector.ShowSelector(m_SerializedSettings, null, true, presetReceiver);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        m_SettingEditor.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            m_SerializedSettings.ApplySettings(this);
        }
    }
}
