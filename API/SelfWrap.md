# SelfWrap : MonoBehaviour (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/SelfWrap.cs)

If a rigidbody object exits the global bounds through the top or bottom of the world, the BoundTrigger will add this script to the escaping object. This script monitors the x and z axis information and allows the object to manage its own wrapping. This script is set automatically without need for the programmer to interact with it directly.

## **Properties**

## Private

___

### **float lowerXBound**

The lower x-axis bound in Euclidean space for the world. Set automatically by the BoundsTrigger.

### **float lowerZBound**

The lower z-axis bound in Euclidean space for the world. Set automatically by the BoundsTrigger.

### **float upperXBound**

The upper x-axis bound in Euclidean space for the world. Set automatically by the BoundsTrigger.

### **float upperZBound**

The upper z-axis bound in Euclidean space for the world. Set automatically by the BoundsTrigger.

### **BoundsTrigger boundsTrigger**

The BoundsTrigger object for the world. Set automatically by the BoundsTrigger.

## **Methods**

## Private

___

### **SetXBounds(Vector2 bounds) -> void**

Sets the upper and lower x bounds.

### **SetZBounds(Vector2 bounds) -> void**

Sets the upper and lower z bounds.

## Public

___

### **SetBounds(BoundsTrigger bounds) -> void**

Sets up all properties. Called by the BoundsTrigger immediately upon instantiation.