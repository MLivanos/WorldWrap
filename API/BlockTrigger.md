# BlockTrigger : TriggerBehavior (Class)

Trigger that informs the WrapManager about where the player is and object relative positions.

## **Properties**

## Private

### **residents : List<GameObject>**

List of Rigidbody root game objects in the block.

## **Methods**

## Private

### **OnTriggerEnter(Collider other) -> void**

If the player enters the trigger, log the entry via WrapManager's LogBlockEntry. If another object enters, add it to residents list.

### **OnTriggerExit(Collider other) -> void**

If the player exits the trigger, log the entry via WrapManager's LogBlockExit. If another object exits, remove it from residents list.

## Public

### **getResidents() List<GameObject>**

Public accessor method for the residents list. 

### **removeResident(GameObject oldResident) -> void**

Remvoes a resident that has since left the block.

[Back to API Main Page](https://github.com/MLivanos/WorldWrap/blob/main/ProgrammingAPI.md)