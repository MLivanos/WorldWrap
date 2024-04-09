# WorldWrapper : MonoBehavior (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/WorldWrapper.cs)

The WorldWrapper is used to determine vectors for wrapping and translating blocks and objects.

## **Properties**

___
## Private

___

### **blockMatrix: GameObject[,]**

A matrix of the blocks representing the current configuration of the world.

### **worldWrapNetworkManager: WorldWrapNetworkManager**

WWNM if multiplayer

### **selfWrappers: List<GameObject>**

List of objects maintaining their own wraps

### **isMultiplayer: bool**

True if this is a multiplayer game, false otherwise.

## **Methods**

___

## Private

### **Start() -> void**

### **WrapWorld() -> void**

Wraps the world in the direction of currentBlock - previousBlock.

### **GetTranslations() -> GameObject[,]**

Depending on how the player moved, creates and returns a new matrix which represents the wrapped world.

### **TranslateBlocks(Vector3[,] oldPositions, GameObject[,] newMatrix)**

Translates blocks based on the BlockMatrix and newMatrix returned from GetTranslations. Calls TranslateObjects to maintain local position of every Rigidbody root GameObject in each block.

### **TranslateObjects(GameObject block, Vector3 movementVector, HashSet<int> objectAlreadyMoved) -> void**

Called from TranslateBlocks(). Translates the root Rigidbody GameObjects in each block.

### **MoveObject(GameObject objectToMove, Vector3 movementVector) -> void**

Moves a single GameObject, using NavMesh.warp in the case of a NavMeshObject.

### **GetBlockPositions() -> Vector3[,]**

Returns the position of every block in the block matrix as a matrix of Vector3s.

### **TranslateLeft(GameObject[,] newMatrix, GameObject[,] oldMatrix) -> void**

Translates all blocks left via TranslateBlock(). The leftmost blocks is moved to the rightmost column.

### **TranslateRight(GameObject[,] newMatrix, GameObject[,] oldMatrix) -> void**

Translates all blocks right via TranslateBlock(). The rightmost blocks is moved to the leftmost column.

### **TranslateUp(GameObject[,] newMatrix, GameObject[,] oldMatrix) -> void**

Translates all blocks up via TranslateBlock(). The topmost blocks is moved to the bottom row.

### **TranslateDown(GameObject[,] newMatrix, GameObject[,] oldMatrix) -> void**

Translates all blocks down via TranslateBlock(). The bottom blocks is moved to the topmost row.