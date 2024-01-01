# WorldWrapNetworkRelay : NetworkBehaviour (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/Multiplayer/WorldWrapNetworkRelay.cs)

Object that establishes communication with other clients for tasks such as the instantiation of new objects in the scene. 

## **Properties**

## Private

___

### **GameObject transformRelayPrefab**

Prefab for a [WorldWrapTransformRelay](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkRelay.md).

### **WorldWrapNetworkManager worldWrapNetworkManager**

The [WorldWrapNetworkManager](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkManager.md) object. Communication to the network is done between these two objects.

### **ulong clientId**

The network ID for this client
___

## **Method**

## Private

___

### **FindWorldWrapNetworkManager() -> void**

Finds the [WorldWrapNetworkManager](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkManager.md) and sets the relevant variables.

### **InstantiateOnNetworkServerRpc(ulong clientID, int prefabIndex) -> void**

Instantiates a new [WorldWrapTransformRelay](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapTransformRelay.md), thus creating a new client object/puppet on the network. Occurs on the server.

## Public

___

### **InstantiateOnNetwork(int prefabIndex) -> void**

Calls on the server to instantiate a new object.

