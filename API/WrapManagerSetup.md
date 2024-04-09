# WrapManager : MonoBehavior (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/WrapManagerSetup.cs)

Setups up variables for [WrapManager](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/WrapManager.cs).

## **Properties**

## Private
___

### **blocks: GameObject[] (SerializeField)**

The blocks that tile the world. This array can be in any order and will still work, as the WrapManager will figure out their structure at the Start method.

### **lureObject: GameObject (SerializeField)**

The lure object is a GameObject with four children: planes which surround the global bounds. At runtime, if the user has checked isUsingNavMesh, the WrapManager will add NavMesh links from one side of the NavMeshLure to the other, making any NavMeshAgents see that they can access the other side of the map at no cost for movement. It is called a "lure" because it will lure NavMeshAgents out of bounds, but the BoundsTrigger will wrap them around the world before they reach the lure.

### **blockMatrix: GameObject[,]**

A matrix of the blocks representing the current configuration of the world.

## **Methods**

___

## Private

### **SortCoordinates(out Vector2[] coordinatesByX, out Vector2[] coordinatesByZ) -> void**

Sorts blocks by their X and Z coordinates.

### **SetupMatrix(Vector2[] coordinatesByX, Vector2[] coordinatesByZ, Dictionary<float, int> xToRow, Dictionary<float, int> zToColumn) -> void**

Sets up matrix after sorting coordinates.

### **FillMatrix(Dictionary<float, int> xToRow, Dictionary<float, int> zToColumn) -> void**

Fills BlockMatrix after SetupMatrix is called.

### **AddNavMeshLinks() -> void**

Adds NavMeshLinks to NavMeshLure. This allows NavMeshAgents to navigate the world efficiently.