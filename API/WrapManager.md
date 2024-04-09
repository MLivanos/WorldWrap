# WrapManager : MonoBehavior (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/WrapManager.cs)

The WrapManager handles information from various trigger behaviors and is responsible for determining when a wrap is required. There should be exactly 1 (one) wrap manager per scene which requires wraps.

## **Properties**

## Private

___

### **blocks: GameObject[] (SerializeField)**

The blocks that tile the world. This array can be in any order and will still work, as the WrapManager will figure out their structure at the Start method.

### **player: GameObject (SerializeField)**

The GameObject that reprsents the player. Currently, there can only be one player, as the world will wrap around them.

### **lureObject: GameObject (SerializeField)**

The lure object is a GameObject with four children: planes which surround the global bounds. At runtime, if the user has checked isUsingNavMesh, the WrapManager will add NavMesh links from one side of the NavMeshLure to the other, making any NavMeshAgents see that they can access the other side of the map at no cost for movement. It is called a "lure" because it will lure NavMeshAgents out of bounds, but the BoundsTrigger will wrap them around the world before they reach the lure.

### **isUsingNavmesh: bool (SerializeField)**

Check if you are using NavMesh for this scene. This will have the WrapManager set up the NavMesh surface to consider the wrapping effect.

### **blockMatrix: GameObject[,]**

A matrix of the blocks representing the current configuration of the world.

### **initialTrigger: GameObject**

The WrapTrigger the player entered if they entered a WrapTrigger (null if not inside one). Used to determine when to wrap the world. (When the player entered one trigger and exited another).

### **currentTrigger: GameObject**

The WrapTrigger the player is currently inside of (null if not inside one). Used to determine when to wrap the world. (When the player entered one trigger and exited another).

### **previousBlock: GameObject**

The previous block the user was inside that contains a BlockTrigger. Used to determine how to wrap the world. (In the direction of currentTrigger - previousTrigger).

### **currentBlock: GameObject**

The current block the user is inside that contains a BlockTrigger. Used to determine how to wrap the world. (In the direction of currentBlock - previousBlock).

### **isTransitioning: bool**

True if the player is exiting one block and entering around. Used to determine when to wrap.

## **Methods**

___

## Private

### **Start() -> void**

Called when object is created in the scene. Sets up block matrix and NavMesh if applicable.

### **ShouldWrap() -> void**

Returns true if the condition for a wrap has been met: The player entered one wrap trigger, exited out another, and is now in a different block. Returns false otherwise.

## Public

### **LogTriggerEntry(GameObject entryBlock)**

Called when player enters a WrapTrigger (inside entryBlock). Sets initialTrigger, currentTrigger, and isTransitioning based how the player enters the WrapTrigger inside of entryBlock.

### **LogTriggereExit(GameObject entryBlock)**

Called when player exits a WrapTrigger. If ShouldWrap() returns true, it will initiate a wrap via the WrapWorld() function.

### **LogBlockEntry()**

Called by BlockTrigger when the player enters a new block. Update previousBlock and currentBlock.

### **SetPlayer(GameObject newPlayer) -> void**

Sets player object to specified player object. Used by Setup Assistant.

### **SetLureObject(GameObject navMeshLure) -> void**

Sets NavMeshLure object to specified object. Used by Setup Assistant.

### **SetBlocksLength(int length) -> void**

Sets up the blocks property based on the number of rows and columns specified in the Setup Assistant.

### **SetIsUsingNavMesh(bool isUsing) -> void**

Sets isUsingNavMesh bool. Used the Setup Assistant.

[Back to API Main Page](https://github.com/MLivanos/WorldWrap/blob/main/ProgrammingAPI.md)