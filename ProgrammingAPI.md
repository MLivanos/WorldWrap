# **WorldWrap: Programming API**

## [WrapManager : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/WrapManager.md)

The WrapManager handles information from various trigger behaviors and is responsible for determining when a wrap is required. There should be exactly 1 (one) wrap manager per scene which requires wraps.

## [WorldWrapper : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/WorldWrapper.md)

The WorldWrapper is used to determine vectors for wrapping and translating blocks and objects.

## [WrapManagerSetup : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/WrapManagerSetup.md)

Setups up variables for WrapManager.

## [TriggerBehavior : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/TriggerBehavior.md)

Abstract class from which WrapTriggers, BoundsTrigger, and BlockTriggers inherit from. Finds the WrapManager in the hierachy.

## [BoundsTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/BoundsTrigger.md)

Trigger wraps helps NPCs and objects around the world when they reach the end.

## [BlockTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/BlockTrigger.md)

Trigger that informs the WrapManager about where the player is and object relative positions.

## [WrapTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/WrapTrigger.md)

=======
Trigger that informs the WrapManager of when the player is leaving and entering a block.

## [SafetyTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/SafetyTrigger.md)

Trigger larger than a BlockTrigger but smaller than a BoundsTrigger. Initiates wraps when the player leaves it. This is useful for games that employ any teleporting mechanics and can fix any issues in wrapping due to lag or improper WrapTrigger placement. 

## [SelfWrap : Monobehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/SelfWrap.md)

This script allows non-player objects to govern their own wraps. The BoundsTrigger will add this component if an object leaves the vertical bounds of the world and removes this component when it returns inside the global bounds. Useful in games where objects may be shot arbitrarily high into the sky. 

# Multiplayer Specific Classes

## [WorldWrapNetworkManager : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkManager.md)

The WorldWrapNetworkManager (WWNM) is in charge of transmitting information from relays to the client and from the client to the rest of the network. They update the transforms of each client's players and the objects throughout the scene. There should be exactly one (1) WWNM in the scene for a multiplayer game. Do not use a WWNM for a singleplayer game.

## [WorldWrapNetworkRelay : NetworkBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkRelay.md)

Relay in charge of instantiating new GameObjects across the network and keeps track of connection information.

## [WorldWrapTransformRelay : NetworkBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapTransformRelay.md)

Relay keeps track of a client's object and tells the rest of the network how it moves and rotates. It also tells other clients when a new GameObject is created or destroyed on the network.

## [WorldWrapNetworkRigidbody : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkRigidbody.md)

Communicates information about collisions between puppet objects and other Rigidbodies in the scene. This body goes on puppets, and only when simulating consistent collisions is important for a particular object.

## [WorldWrapNetworkObject : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/Multiplayer/WorldWrapNetworkObject.md)

Contains information about object ownership. This script changes object ownership when parented by another clinet. Use of the WorldWrapNetwork Object is governed by the WorldWrapNetworkManager automatically without need for programmer intervention.
