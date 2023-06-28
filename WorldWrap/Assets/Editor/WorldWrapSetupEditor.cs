using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class WorldWrapSetupEditor : EditorWindow
{
    GUIStyle checkboxStyle = new GUIStyle();
    private WrapManager wrapManagerScript;
    private GameObject wrapManagerObject;
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
        EditorGUIUtility.labelWidth = 175;
        isUsingNavmesh = EditorGUILayout.Toggle("Using NavMesh: ", isUsingNavmesh);
        isExisting = EditorGUILayout.Toggle("Setting Up An Existing Scene: ", isExisting, GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Setup WorldWrap"))
        {
            CreatWorldWrapObjects();
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

    private void CreatWorldWrapObjects()
    {
        if (WorldWrapAlreadyExists())
        {
            Debug.LogWarning("WrapManager already exists! Aborting Setup.", wrapManagerObject);
            return;
        }
        wrapManagerObject = new GameObject("WrapManager");
        wrapManagerScript = wrapManagerObject.AddComponent(typeof(WrapManager)) as WrapManager;
        findPlayer();
    }

    private void findPlayer()
    {
        GameObject playerObject = null;
        string objectName = "";
        int hammingDistance;
        bool hasPlayerInName;
        int minimumHammingDistance = int.MaxValue;
        bool existsAnotherCandidate = false;
        foreach (GameObject objectInScene in FindObjectsOfType(typeof(GameObject))) 
        {
            objectName = objectInScene.name.ToLower();
            hammingDistance = HammingDistanceToPlayer(objectName);
            hasPlayerInName = objectName.Contains("player");
            if (hasPlayerInName &&  hammingDistance < minimumHammingDistance)
            {
                minimumHammingDistance = hammingDistance;
                playerObject = objectInScene;
                existsAnotherCandidate = false;
                wrapManagerScript.SetPlayer(playerObject);
            }
            else if (objectName.Contains("player") &&  hammingDistance == minimumHammingDistance)
            {
                existsAnotherCandidate = true;
            }
        }
        if (playerObject == null)
        {
            Debug.LogWarning("WorldWrap requires an object to be designated as the player. No such object was found automatically. Please add one to the Player field under WrapManager.", wrapManagerObject);
        }
        if (existsAnotherCandidate)
        {
            Debug.LogWarning(string.Format("WorldWrap requires an object to be designated as the player. We beleive {0} is your player object, but we may be wrong. Please check if this is correct, and change the player object in the Player field if need be.", objectName), wrapManagerObject);
        }
    }

    private int HammingDistanceToPlayer(string inputString)
    {
        // Number of letters that are not 'player'
        return inputString.Length - 6;
    }

    private bool WorldWrapAlreadyExists()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene) 
        {
            if (objectInScene.name == "WrapManager")
            {
                wrapManagerObject = objectInScene;
                return true;
            }
        }
        return false;
    }

}
