
# BoundsTrigger : TriggerBehavior (Class)

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