# SafetyTrigger : TriggerBehavior (Class)

[Go To File](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/SafetyTrigger.cs)

This trigger is meant as a redundancy to ensure that wraps occur. If the player somehow moves through the world without exiting two [WrapTriggers](https://github.com/MLivanos/WorldWrap/blob/main/WorldWrap/Assets/Scripts/WorldWrap/WrapTrigger.cs) as intended, exiting the SafetyTrigger will trigger a wrap. This is also useful for games with a teleport mechanic, as this will detect such movement and adjust the world accordingly. Finally, SafetyTriggers resolve glitches from players moving extremely fast, low framerates causing skipping, or poorly configured WrapTriggers. For this reason, we advise creating both WrapTriggers and a SafetyTrigger.

## **Methods**

## Private

___

### **OnTriggerExit(Collider collider) -> void**

If the player exits the safety trigger, the world is wrapped.