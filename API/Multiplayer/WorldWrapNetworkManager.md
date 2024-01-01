# WorldWrapNetworkManager : MonoBehaviour (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/Multiplayer/WorldWrapNetworkManager.cs)

Manages the updates to and from the network to maintain seemless wrapping world for each player will maintaining consistency for all other players and world objects. Note that this is a MonoBehaviour, not a NetworkBehavior. This is not a shared resource - each player connecting to the server will have their own WorldWrapNetworkManager.

## **Properties**

## Private

___

### **GameObject[] clientPrefabs**

A list of prefabs to be instantiated client-side representing the client's player and objects.

### **GameObject[] puppetPrefabs**

A list of prefabs to be instantiated to represent other players and their objects.

### **GameObject transformRelayPrefab**

A prefab for the [WorldWrapTransformRelay](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapTransformRelay.md) object for the game.. Should be set to WorldWrap/Prefabs/TransformRelay. Newly instantiated objects get their own transform relay.

### **string puppetName**

What to name new puppet objects in the heirachy. Has no effect on final build, but may be useful for testing purposes.

### **bool firstPrefabIsPlayer**

Check if the first item on the clientPrefabs is the player and you want the player to be instantiated as soon as they connect to the server.

### **WrapManager wrapManager**

The [WrapManager](https://github.com/MLivanos/WorldWrap/blob/main/API/WrapManager.md) object for the game.

### **WorldWrapNetworkRelay networkRelay**

Each player will get a network relay that helps them communicate with all other players connected to the server.

### **List<GameObject> puppets**

A list of all puppets in the current scene. Every update, each puppet will be moved based on how the client object moved on their repsective owner's session.

### **List<WorldWrapTransformRelay> puppets**

A list of all WorldWrapTransformRelays for all puppets in the scene. These are networkVariables that contain information for how the puppets should be moved each frame.

### **List<GameObject> clientObjects**

A list of all client GameObjects. Every update, the movement of these client objects is logged and that information is distributed across the network to update all other player's puppets.

### **List<WorldWrapTransformRelay> puppets**

A list of all WorldWrapTransformRelays for all clients in the scene. These are networkVariables that contain information for how the clients moved each frame.

### **List<Vector3> lastPosition**

The position last from for every client object.

### **List<Vector3> puppets**

The index in clientRelays that represents the client's player.

## **Methods**

## Private

___

### **UpdateAllPuppets() -> void**

Reads the movement from every other player's TransformRelays and updates the position of every puppet.

### **SendUpdates() -> void**

Reads the movement from the local player's TransformRelays and updates the movement for every client object for the other players to read.

### **FindWrapManager() -> void**

Finds the WrapManager object in the scene and sets the WrapManager property.

### **HasPrefabName(string name) -> bool**

Returns true if the input string starts with the puppetName.

### **SetupRigidbody(GameObject newPuppet, WorldWrapTransformRelay puppetTransformRelay) -> void** 

Adds a [WorldWrapNetworkRigidBody](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkRigidBody.md) to the newly instantiated object. This allows collisions between rigidbodies and WorldWrapNetworkRigidBodies to produce force to the respective clients.

### **SetupNetworkObjectScript(GameObject newPuppet, WorldWrapTransformRelay puppetTransformRelay) -> void** 

Adds a [WorldWrapNetworkObject](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkObject.md) component to the newly instantiated object.

### **UpdatePuppetPosition(int puppetIndex) -> void**

Updates the position and rotation of the puppet at puppetIndex.

### **SendPositionUpdate(int objectIndex) -> void**

Sends the movement for the clinet object at objectIndex to the network.

### **SendPositionUpdate(Vector3 offset, int objectIndex) -> void**

Sends the movement for the clinet object at objectIndex to the network minus the offset.

### **ShouldNotCreatePuppet(GameObject newPuppetRelay) -> bool**

Returns true if we own the object, meaning that we do not need to create a puppet for it.

### **OwnsRelay(WorldWrapNetworkRelay newPuppetRelay) -> bool**

Returns true if we own the relay.

### **OffsetChildren(Vector3 movementVector, GameObject objectToMove) -> void**

For all the children of objectToMove, send the position update to the respective relay. Recursively calls itself for each child found.

## Public

___

### **AddToPuppets(GameObject newRelay) -> void** 

Calls for the creation of a new puppet object from the newly instantiated relay, newRelay. Adds newRelay to puppetTransforms.

### **CreatePuppetObject(WorldWrapTransformRelay newRelay) -> void** 

Creates the puppet object and adds it to the puppets list. Sets up relevant fields such as [WorldWrapNetworkRigidBodies](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkRigidBody.md) and [WorldWrapNetworkObjects](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkObjects.md) if appropriate.

### **AddToClientObjects(WorldWrapTransformRelay newRelay) -> void**

Calls for the instantiation of a new client object and sets up the relay on the network.

### **AddClientRelay(WorldWrapTransformRelay newRelay) -> void**

Sets up local variables for a new client object.

### **ChangePuppetToClient(GameObject oldPuppet, int prefabIndex) -> void**

When the client gains ownership of a gameObject (the object represented by oldPuppet), this function destroys the old puppet and adds a new client object instead.

### **SetNetworkRelay(WorldWrapNetworkRelay relay) -> void**

Sets the network relay variable and instantiates the player object if firstPrefabIsPlayer is marked true.

### **InstantiateOnNetwork(int prefabIndex) -> void**

Instantiates a new client object (based on prefab index) on the network.

### **GetPuppetOffset() -> Vector3**

Gets the semantic offset to convert between the initial Euclidean space and WrapSpace.

### **FindPuppets() -> void**

Upon logging on to the server, this finds all of the WorldWrapTransformRelays already on the network and creates puppets for each.

### **OffsetTransform(Vector3 offset) -> void**

Offset the player character's movement vector. Used to ensure that wraps do not warp puppets for other players.

### **Warp(Vector3 movementVector, GameObject objectToMove) -> void**

Moves and object by movement vector and offsets its WorldWrapTransformRelay and those of its children by movementVector.

### **ApplyForce(WorldWrapTransformRelay clientRelay, Vector3 force) -> void**

Adds force as an impulse to the rigidbody of clientRelay. Used when other players or their objects collide with a puppet.

### **IsClient(GameObject possibleClient) -> bool**

Returns true if the GameObject is in the clinetObjects

### **ReplaceClientWithPuppet(WorldWrapTransformRelay relayToReplace) -> void**

Replaces a client object with the relevant puppet object. Used when the ownership of an object is transfered from one client to another.

### **RemoveClient(WorldWrapTransformRelay relayToRemove, bool deleteClientObject = true)**

Removes one of the client objects across all players on the server

### **RemoveClient(GameObject objectToRemove, bool deleteClientObject = true)**

Removes one of the client objects across all players on the server

### **RemovePuppet(WorldWrapTransformRelay relayToRemove)**

Removes puppet transform from the puppet transform list.

### **AcceptNewTransform(WorldWrapTransformRelay transformRelay)**

Adds new transform to the puppetTransform list.