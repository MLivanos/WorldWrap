using System.Collections;
using System.Collections.Generic;
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
    private bool isMultiplayer;
    private string worldWrapTag;

    [MenuItem("Window/WorldWrap Setup Assistant")]

    public static void ShowWidnow()
    {
        GetWindow<WorldWrapSetupEditor>("WorldWrap Setup");
    }

    private void OnGUI()
    {
        SetupTitle();
        worldWrapTag = "WorldWrapObject";
        worldSize = EditorGUILayout.Vector3Field("World Size: ", worldSize);
        EditorGUILayout.BeginHorizontal();
        numberOfRows = EditorGUILayout.IntField("Number Of Rows: ", numberOfRows);
        numberOfColumns = EditorGUILayout.IntField("Number Of Columns: ", numberOfColumns);
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 175;
        isUsingNavmesh = EditorGUILayout.Toggle("Using NavMesh: ", isUsingNavmesh);
        isMultiplayer = EditorGUILayout.Toggle("Is Multiplayer: ", isMultiplayer);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Setup WorldWrap"))
        {
            CreatWorldWrapObjects();
        }
        if (GUILayout.Button("Clear WorldWrap"))
        {
            ClearWorldWrapObjects();
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
        EditorGUILayout.LabelField("Welcome To The WorldWrap Setup Assistant", windowTitleStyle);
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
        SetupSafetyTrigger();
        ParentWrapManagers();
        if (isUsingNavmesh)
        {
            wrapManagerScript.SetIsUsingNavMesh(true);
            CreateNavMeshPlanes();
        }
        if (isMultiplayer)
        {
            wrapManagerScript.UsingMultiplayer(true);
            CreateWorldWrapNetworkManager();
        }
    }

    private void SetupWrapManager()
    {
        wrapManagerObject = new GameObject("WrapManager");
        wrapManagerObject.tag = worldWrapTag;
        wrapManagerScript = wrapManagerObject.AddComponent(typeof(WrapManager)) as WrapManager;
        wrapManagerScript.SetBlocksLength(numberOfRows * numberOfColumns);
    }

    private void SetupGlobalBounds()
    {
        bounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bounds.name = "GlobalBounds";
        bounds.tag = worldWrapTag;
        bounds.transform.localScale = worldSize;
        bounds.GetComponent<Renderer>().material = (Material)Resources.Load("Translucent1", typeof(Material));
        bounds.AddComponent(typeof(BoundsTrigger));
    }

    private void SetupSafetyTrigger()
    {
        bounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bounds.name = "SafetyTrigger";
        bounds.tag = worldWrapTag;
        bounds.transform.localScale = new Vector3(2 * worldSize.x / numberOfRows, worldSize.y, 2 * worldSize.z / numberOfColumns);
        bounds.GetComponent<Renderer>().material = (Material)Resources.Load("Translucent3", typeof(Material));
        bounds.AddComponent(typeof(SafetyTrigger));
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

    private GameObject SetupBlock(float xPosition, float zPosition, int blockNumber)
    {
        float rowInterval = worldSize.x / numberOfRows;
        float columnInterval = worldSize.z / numberOfColumns;
        Material clearMaterial = (Material)Resources.Load("Translucent2", typeof(Material));
        GameObject block = new GameObject(string.Format("Block{0}", blockNumber));
        block.transform.position = new Vector3(xPosition, 0.0f, zPosition);
        block.tag = worldWrapTag;
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
        wrapManagerScript.AddBlock(block);
        // AddWrapTriggers
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
        return block;
    }

    private void CreateTrigger(Vector3 scale, Vector3 position)
    {
        Material clearMaterial = (Material)Resources.Load("Translucent3", typeof(Material));
        GameObject wrapTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wrapTrigger.name = "WrapTrigger";
        wrapTrigger.tag = worldWrapTag;
        wrapTrigger.transform.localScale = scale;
        wrapTrigger.transform.position = position;
        wrapTrigger.GetComponent<Renderer>().material = clearMaterial;
        wrapTrigger.AddComponent(typeof(WrapTrigger));
    }

    private void CreateWorldWrapNetworkManager()
    {
        GameObject networkManagerObject = new GameObject("WorldWrapNetworkManager");
        networkManagerObject.tag = worldWrapTag;
        WorldWrapNetworkManager networkManagerComponent = networkManagerObject.AddComponent(typeof(WorldWrapNetworkManager)) as WorldWrapNetworkManager;
        wrapManagerScript.SetNetworkManager(networkManagerObject);
        Debug.LogWarning("Please ensure that Unity's Netcode for GameObjects is installed and that a NetworkManager object is created with the appropriate settings");
        Debug.LogWarning("Please set NetworkRelay prefab. If you are using an unmodified copy of WorldWrap, this prefab can be found in WorldWrap/Assets/Prefabs/WorldWrapNetworkRelay");
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
        wrapTriggerParent.tag = worldWrapTag;
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene) 
        {
            if (objectInScene.name == "WrapTrigger")
            {
                objectInScene.transform.parent = wrapTriggerParent.transform;
            }
        }
    }

    private void CreateNavMeshPlanes()
    {
        float planeZOffset = worldSize.z / numberOfColumns;
        float planeXOffset = worldSize.x / numberOfRows;
        float planeZScale = worldSize.z / 10.0f;
        float planeXScale = worldSize.x / 10.0f;
        float planePosition = -1 * worldSize.z / 2.0f - 1.5f;
        GameObject navMeshLure = new GameObject("NavMeshLure");
        navMeshLure.tag = worldWrapTag;
        GameObject plane1 = CreateNavMeshPlane(planeXScale, 0.0f, planePosition, navMeshLure);
        GameObject plane2 = CreateNavMeshPlane(planeXScale, 180.0f, planePosition, navMeshLure);
        planePosition = -1 * worldSize.z / 2.0f - 1.5f;
        GameObject plane3 = CreateNavMeshPlane(planeZScale, 90.0f, planePosition, navMeshLure);
        GameObject plane4 = CreateNavMeshPlane(planeZScale, 270.0f, planePosition, navMeshLure);
        navMeshLure.transform.position = wrapManagerObject.transform.position;
        wrapManagerScript.SetLureObject(navMeshLure);
    }

    private GameObject CreateNavMeshPlane(float scale, float rotation, float offset, GameObject navMeshLure)
    {
        GameObject plane  = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(scale, 1.0f, 0.3f);
        plane.transform.Rotate(0.0f, rotation, 0.0f, Space.World);
        // Create a plane that is at the origin on two axes, offset on the other to be touching the edge of the world
        plane.transform.Translate(plane.transform.TransformVector(Vector3.forward).normalized * offset, Space.World);
        plane.GetComponent<Renderer>().material = (Material)Resources.Load("Translucent1", typeof(Material));
        plane.transform.parent = navMeshLure.transform;
        return plane;
    }

    private void SetupExistingScene()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        Dictionary<float, int> numberOfObjectsByX = new Dictionary<float, int>();
        Dictionary<float, int> numberOfObjectsByZ = new Dictionary<float, int>();
        float objectXValue;
        float objectZValue;
        foreach (GameObject objectInScene in gameObjectsInScene) 
        {
            objectXValue = objectInScene.transform.position.x;
            objectZValue = objectInScene.transform.position.x;
            if (!numberOfObjectsByX.ContainsKey(objectXValue))
            {
                numberOfObjectsByX[objectXValue] = 0;
            }
            if (!numberOfObjectsByZ.ContainsKey(objectZValue))
            {
                numberOfObjectsByZ[objectZValue] = 0;
            }
            numberOfObjectsByX[objectXValue]++;
            numberOfObjectsByZ[objectZValue]++;
        }
        List<float> xRowPosition = CountAxis(numberOfRows, numberOfObjectsByZ, "row");
        List<float> zColumnPosition = CountAxis(numberOfColumns, numberOfObjectsByX, "column");
        for(int row = 0; row < numberOfRows; row++)
        {
            for(int column = 0; column < numberOfColumns; column++)
            {
                GameObject terrain = GetObjectAtPoint(xRowPosition[row], zColumnPosition[column]);
                GameObject block = SetupBlock(xRowPosition[row], zColumnPosition[column], row * numberOfColumns + column);
                terrain.transform.parent = block.transform.parent;
            }
        }
    }

    private List<float> CountAxis(int numberOfElementsInAxis, Dictionary<float, int> countByPosition, string axisName)
    {
        int numberOfElementsLeft = numberOfElementsInAxis;
        List<float> positions = new List<float>();
        foreach(KeyValuePair<float,int> positionValue in countByPosition)
        {
            if (positionValue.Value == numberOfElementsInAxis)
            {
                positions.Add(positionValue.Key);
                numberOfElementsLeft--;
            }
        }
        CheckNumberOfElements(numberOfElementsLeft, numberOfElementsInAxis, axisName);
        positions.Sort();
        return positions;
    }

    private void CheckNumberOfElements(int numberOfElementsLeft, int numberOfElementsInAxis, string axisName)
    {
        if (numberOfElementsLeft != 0)
        {
            string tooManyOrFew = "many";
            if (numberOfElementsLeft > 0)
            {
                tooManyOrFew = "few";
            }
            Debug.LogError(string.Format("Too {0} {1}s found. {2} {1}s were found, but {3} {1}s were specified. Check that {1}s are aligned and the number of {1}s matches the number of objects in that {1}.", tooManyOrFew, axisName, numberOfElementsInAxis - numberOfElementsLeft, numberOfElementsInAxis));
        }
    }

    private GameObject GetObjectAtPoint(float x, float z)
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if (objectInScene.transform.position.x == x && objectInScene.transform.position.z == z)
            {
                return objectInScene;
            }
        }
        return null;
    }

    private void ClearWorldWrapObjects()
    {
        if(!EditorUtility.DisplayDialog("Delete all WorldWrap Objects?",
                "Are you sure you want to delete all WorldWrap Objects? This will delete all object with either the "
                + worldWrapTag  + " along with all of their children. Please deparent all objects you intend to keep. This action cannot be undone.",
                "Yes, delete the objects", "No, cancel"))
        {
            return;
        }
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if (objectInScene.tag == worldWrapTag)
            {
                DestroyImmediate(objectInScene);
            }
        }
    }
}
