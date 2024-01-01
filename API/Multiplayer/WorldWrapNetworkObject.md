# WorldWrapNetworkObject : MonoBehaviour (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/Multiplayer/WorldWrapNetworkObject.cs)

Contains information about who owns a particular object and will initiate the change in ownership if/when it, as a puppet, is parented by another player. This is attatched to either a client or a puppet object

## **Properties**

## Private

___

### **WorldWrapTransformRelay relay**

The relay that governs (in the case of a puppet) or relays (in the case of a client object) information about the object's movement.

## **Methods**

## Private

___

### **OnTransformParentChanged() -> void**

If the transform's parent is changed by one of the other players, ownership is transfered to that player.

## Public

___

### **GetClientID() -> ulong**

Returns owner's clientID

### **getTransformRelay() -> WorldWrapTransformRelay**

Returns the WorldWrapTransformRelay for this object

### **IsOwned() -> bool**

Returns true if this client owns this object as a client object