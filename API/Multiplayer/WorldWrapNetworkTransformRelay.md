# WorldWrapTransformRelay : NetworkBehaviour (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/Multiplayer/WorldWrapTransformRelay.cs)

Tracks and relays the changes to the position and rotation of an object and passes that information to the other players so that they can move their respective puppets.

## **Properties**

## Private
___

### **NetworkVariable<Vector3> puppetPosition**

The position of the object. Everyone can read, only owner can write.

### **NetworkVariable<Vector3> puppetRotation**

THe rotation (in Euler Angles) of the object. Everyone can read, only owner can write.

### **NetworkVariable<int> prefabIndex**

Prefab index for the WorldWrapNetworkzManager.

### **WorldWrapNetworkManager worldWrapNetworkManager**

The local WorldWrapNetworkManager.

### **Vector3 lastPosition**

The local position of the puppet object in the previous frame.

### **Vector3 lastRotation**

The local rotation of the puppet object in the previous frame.

### **ulong clientId**

The object owner's ID.
___

## **Method**

## Private
___

### **OnCollisionEnter(Collision collision) -> void**

Registers collision and, if the colliding object is owned by the player, passes that force to the client who owns that object.

### **CollidedWithClient(GameObject collisionGameObject) -> bool**

Returns true if the colliding object is owned by the player.

### **AddToPuppetsClientRpc(string senderName) -> void**

Calls on the player's [WorldWrapNetworkManager](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkManager.md) to add this object to their puppetTransformRelay list.

### **ApplyForceClientRpc(Vector3 force, ClientRpcParams clientRpcParams = default)**

Calls on a client to apply a force to the object governed by this relay.

### **RemovePuppetsClientRpc() -> void**

Calls on all other players to remove this puppet from their scene

### **ChangeOwnershipClientRpc() -> void**

Finds old owner and has them change their client object to a puppet object. Part of the ownership change process.

### **AddToClientsClientRpc() -> void**

Finds new owner and has them change their puppet object to a client object and adds this to their clientRelays list. Part of the ownership change process.

### **SendOffsetClientRpc(float xOffset, float zOffset) -> void**

Sends an offset to everyone's lastPosition to make up for the fact that the players may be in different wrap spaces.

### **AddToPuppetsServerRpc() -> void**

Has the server call AddToPuppetsClientRpc().

### **RemovePuppetsServerRpc() -> void**

Has the server call RemovePuppetsClientRpc().

### **SetClientIdServerRpc(ulong clientIdNumber) -> void**

Has the server update the owner's ID.

### **ApplyForceServerRpc(Vector3 force) -> void**

Has the server call ApplyForceClientRpc to the owner.

### **DespawnServerRpc() -> void**

Calls on the server to despawn this object.

### **SendOffsetServerRpc(float xOffset, float zOffset) -> void**

Calls on server to tell all clients to offset their lastPositions via SendOffsetClientRpc()

### **ChangeOwnershipServerRpc(ulong newClientID) -> void**

Changes ownership of the relay and corresponding object to the client represented by newClientID.

## Public
___

### **IsOwned() -> bool**

Returns true if the caller owns the object

### **Setup() -> void**

Calls on the server to tell all other players to instantiate puppets and tie them to this relay.

### **GetPosition() -> Vector3**

Gets the position vector.

### **GetEulerAngles() -> Vector3**

Gets the rotation vector.

### **GetRotation() -> Vector3**

Gets the rotation vector in Euler angles.

### **GetMovement() -> Vector3**

Gets the relative movement vector: puppetPosition - lastPosition, and updates last position

### **GetRotation() -> Vector3**

Gets the rotation vector: puppetRotation - lastRotation, and updates last rotation

### **Warp(Vector3 offset) -> void**

Modifies position by subtracting out the offset. Used when a player warps so that the translation due to the warp does not effect the puppetPosition variable.

### **OffsetLastPosition(Vector3 offset) -> void**

Subtracts lastPosition by offset.

### **InitializeTransform(Vector3 initialPosition, Vector3 initialRotation) -> void**

Sets puppetPosition/Rotation and lastPosition/rotation.

### **GetPrefabIndex() -> int**

returns the prefabIndex

### **ApplyForce(Vector3 force) -> void**

Makes call to server to apply the force to the owner.

### **ChangeOwnership(ulong newClientID) -> void**

Makes call to server to change the owner to the new owner.

### **ZeroLastPosition() -> void**

Changes lastPosition to the Euclidean Space transformation of wrap space's origin

### **ChangePuppetToClient() -> void**

Makes call to the server to change ownership of the puppet to itself.