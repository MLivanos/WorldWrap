using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class WorldWrapSetupEditor : EditorWindow
{
    GUIStyle checkboxStyle = new GUIStyle();
    private WrapManager wrapManagerScript;
    private GameObject wrapManagerObject;
    private GameObject bounds;
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
            Debug.LogError("WrapManager already exists! Aborting Setup.", wrapManagerObject);
            return;
        }
        if (HasInvalidDimensions())
        {
            return;
        }
        SetupWrapManager();
        SetupGlobalBounds();
        SetupBlocks();
        ParentWrapManagers();
    }

    private void SetupWrapManager()
    {
        wrapManagerObject = new GameObject("WrapManager");
        wrapManagerScript = wrapManagerObject.AddComponent(typeof(WrapManager)) as WrapManager;
        findPlayer();
    }

    private void SetupGlobalBounds()
    {
        bounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bounds.name = "GlobalBounds";
        bounds.transform.localScale = worldSize;
        bounds.GetComponent<Renderer>().material = (Material)Resources.Load("Translucent1", typeof(Material));
        bounds.AddComponent(typeof(BoundsTrigger));
    }

    private void SetupBlocks()
    {
        float rowInterval = worldSize.x / numberOfRows;
        float columnInterval = worldSize.z / numberOfColumns;
        float initialX = (-1 * worldSize.x + rowInterval) / 2.0f;
        float initialZ = (-1 * worldSize.z + columnInterval) / 2.0f;
        for(int row = 0; row < numberOfRows; row++)
        {
            for(int column = 0; column < numberOfColumns; column++)
            {
                SetupBlock(initialX + rowInterval * row, initialZ + columnInterval * column, row * numberOfColumns + column);
            }
        }
    }

    private void SetupBlock(float xPosition, float zPosition, int blockNumber)
    {
        float rowInterval = worldSize.x / numberOfRows;
        float columnInterval = worldSize.z / numberOfColumns;
        Material clearMaterial = (Material)Resources.Load("Translucent2", typeof(Material));
        GameObject block = new GameObject(string.Format("Block{0}", blockNumber));
        block.transform.position = new Vector3(xPosition, 0.0f, zPosition);
        GameObject blockBounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        blockBounds.name = "BlockBounds";
        BoxCollider blockCollider = blockBounds.GetComponent<BoxCollider>();
        blockBounds.transform.parent = block.transform;
        blockBounds.transform.localPosition = Vector3.zero;
        blockBounds.GetComponent<Renderer>().material = clearMaterial;
        blockCollider.isTrigger = true;
        blockBounds.transform.localScale = new Vector3(rowInterval, Mathf.Max(rowInterval, columnInterval), columnInterval);
        blockBounds.AddComponent(typeof(BlockTrigger));
        block.transform.parent = bounds.transform;

        Vector3 scale;
        Vector3 position;

        float triggerScaleFactor = 5.0f;
        float inverseScaleFactor = (triggerScaleFactor-1)/triggerScaleFactor;

        scale = new Vector3(rowInterval * inverseScaleFactor, 1.0f, columnInterval / triggerScaleFactor * 0.5f);
        position = new Vector3(xPosition, 0.0f, zPosition + columnInterval / 2.0f - scale.z / 2.0f);
        CreateTrigger(scale, position);
        position = new Vector3(xPosition, 0.0f, zPosition - columnInterval / 2.0f + scale.z / 2.0f);
        CreateTrigger(scale, position);
        scale = new Vector3(rowInterval / triggerScaleFactor * 0.5f, 1.0f, columnInterval);
        position = new Vector3(xPosition + rowInterval / 2.0f - scale.x / 2.0f, 0.0f, zPosition);
        CreateTrigger(scale, position);
        position = new Vector3(xPosition - rowInterval / 2.0f + scale.x / 2.0f, 0.0f, zPosition);
        CreateTrigger(scale, position);
    }

    private void CreateTrigger(Vector3 scale, Vector3 position)
    {
        Material clearMaterial = (Material)Resources.Load("Translucent3", typeof(Material));
        GameObject wrapTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wrapTrigger.name = "WrapTrigger";
        wrapTrigger.transform.localScale = scale;
        wrapTrigger.transform.position = position;
        wrapTrigger.GetComponent<Renderer>().material = clearMaterial;
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

    private bool HasInvalidDimensions()
    {
        if (worldSize.x <= 0 || worldSize.y <= 0 || worldSize.z <= 0)
        {
            Debug.LogError("World size must be positive non-zero for all dimensions x,y, and z.");
            return true;
        }
        if (numberOfRows <= 0)
        {
            Debug.LogError("Number of rows must be positve non-zero");
            return true;
        }
        if (numberOfColumns <= 0)
        {
            Debug.LogError("Number of columns must be positve non-zero");
            return true;
        }
        if (numberOfColumns == 1 && numberOfRows == 1)
        {
            Debug.LogError("At least one dimension (row or column) must be greater than 1");
            return true;
        }
        return false;
    }

    private void ParentWrapManagers()
    {
        GameObject wrapTriggerParent = new GameObject("WrapTriggers");
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene) 
        {
            if (objectInScene.name == "WrapTrigger")
            {
                objectInScene.transform.parent = wrapTriggerParent.transform;
            }
        }
    }

}
