# WorldWrapNetworkRigidbody : MonoBehaviour (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/Multiplayer/WorldWrapNetworkRigidbody.cs)

Registers collisions with puppets and transfers that force to the respective client.

## **Properties**

## Private
___

### **Rigidbody puppetRigidbody**

The Rigidbody of the puppet, used to register the force of collisions.

### **WorldWrapTransformRelay clientTransformRelay**

The WorldWrapTransformRelay of the object.

### **WorldWrapNetworkManager worldWrapNetworkManager**

The local worldWrapNetworkManager.

___

## **Method**

## Private

___

### **OnCollisionEnter(Collision collision) -> void**

Registers collision and, if the colliding object is owned by the player, passes that force to the client who owns that object.

### **CollidedWithClient(GameObject collisionGameObject) -> bool**

Returns true if the colliding object is owned by the player.

