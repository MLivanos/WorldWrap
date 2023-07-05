# TriggerBehavior : MonoBehavior (Abstract Class)

Abstract class from which WrapTriggers, BoundsTrigger, and BlockTriggers inherit from. Finds the WrapManager in the hierachy.

## **Properties**

## Protected

### **wrapManager : WrapManager**

The WrapManager for this scene. Found automatically in start method.

## **Methods**

## Protected

### **Start() -> void (virtual)**

Finds the WrapManager for the scene. Base method to be called by all triggers.

[Back to API Main Page](https://github.com/MLivanos/WorldWrap/blob/DOCS-APIDocumentation/ProgrammingAPI.md)