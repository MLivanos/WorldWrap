using UnityEngine;
using UnityEditor;

public class WorldWrapSetupEditor : EditorWindow
{
    GUIStyle checkboxStyle = new GUIStyle();
    private Vector3 worldSize;
    private int numberOfRows;
    private int numberOfColumns;
    private bool isUsingNavmesh;
    private bool isExisting;

    [MenuItem("Window/World Wrap")]

    public static void ShowWidnow()
    {
        GetWindow<WorldWrapSetupEditor>("WorldWrap Setup");
    }

    private void OnGUI()
    {
        SetupTitle();
        worldSize = EditorGUILayout.Vector3Field("World Size: ", worldSize);
        EditorGUILayout.BeginHorizontal();
        numberOfRows = EditorGUILayout.IntField("Number Of Rows: ", numberOfRows);
        numberOfColumns = EditorGUILayout.IntField("Number Of Columns: ", numberOfColumns);
        EditorGUILayout.EndHorizontal();
        SetupCheckboxStyle();
        EditorGUIUtility.labelWidth = 175;
        isUsingNavmesh = EditorGUILayout.Toggle("Using NavMesh: ", isUsingNavmesh);
        isExisting = EditorGUILayout.Toggle("Setting Up An Existing Scene: ", isExisting, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Click Setup World When Ready");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Setup WorldWrap"))
        {
            Debug.Log("Pressed");
        }
        if (GUILayout.Button("Clear WorldWrap"))
        {
            Debug.Log("Pressed");
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SetupTitle()
    {
        GUIStyle windowTitleStyle = new GUIStyle();
        windowTitleStyle.alignment = TextAnchor.UpperCenter;
        windowTitleStyle.fontSize = 24;
        windowTitleStyle.fontStyle = FontStyle.Bold;
        windowTitleStyle.wordWrap = true;
        windowTitleStyle.normal.textColor = new Color(1,1,1);
        EditorGUILayout.LabelField("Welcome To The WorldWrap Setup Window", windowTitleStyle);
    }

    private void SetupCheckboxStyle()
    {
        checkboxStyle.wordWrap = true;
        checkboxStyle.normal.textColor = new Color(1,1,1);
    }

}
