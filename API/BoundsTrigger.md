
# BoundsTrigger : TriggerBehavior (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/BoundsTrigger.cs)

Trigger wraps helps NPCs and objects around the world when they reach the end.

## **Properties**

## Private

### **lowerXBound : float**

Lower X bound for the world.

### **lowerZBound : float**

Lower Z bound for the world.

### **upperXBound : float**

Upper X bound for the world.

### **upperZBound : float**

Upper Z bound for the world.

## **Methods**

## Private

### **Start() -> void**

Sets bounds based on world size.

### **OnTriggerExit(Collider other) -> void**

When an object exits the bounds of the world, it will wrap around to the other side.

## Public

### **GetXBounds -> Vector2**

Gets X bounds as a Vector2 of lowerXBound, upperXBound

### **GetZBounds -> Vector2**

Gets Z bounds as a Vector2 of lowerZBound, upperZBound

[Back to API Main Page](https://github.com/MLivanos/WorldWrap/blob/main/ProgrammingAPI.md)