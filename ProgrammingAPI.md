# **WorldWrap: Programming API**

## [WrapManager : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/WrapManager.md)

The WrapManager handles information from various trigger behaviors and is responsible for determining when a wrap is required, rearranging blocks after wraps, and performaning the wraps. There should be exactly 1 (one) wrap manager per scene which requires wraps.

## [TriggerBehavior : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/main/API/TriggerBehavior.md)

Abstract class from which WrapTriggers, BoundsTrigger, and BlockTriggers inherit from. Finds the WrapManager in the hierachy.

## [BoundsTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/BoundsTrigger.md)

Trigger wraps NPCs and objects around the world when they reach the end.

## [BlockTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/BlockTrigger.md)

Trigger that informs the WrapManager about where the player is and object relative positions.

## [WrapTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/main/API/WrapTrigger.md)

Trigger that informs the WrapManager of when the player is leaving and entering a block.

# Multiplayer Specific Classes

## WorldWrapNetworkManager : MonoBehaviour

The WorldWrapNetworkManager (WWNM) is in charge of transmitting information from relays to the client and from the client to the rest of the network. They update the transforms of each client's players and the objects throughout the scene. There should be exactly one (1) WWNM in the scene for a multiplayer game. Do not use a WWNM for a singleplayer game.

## WorldWrapNetworkRelay : NetworkBehaviour

Relay in charge of instantiating new GameObjects across the network and keeps track of connection information.

## WorldWrapTransformRelay : NetworkBehaviour

Relay keeps track of a client's object and tells the rest of the network how it moves and rotates. It also tells other clients when a new GameObject is created or destroyed on the network.

## WorldWrapNetworkRigidbody : MonoBehaviour

Communicates information about collisions between puppet objects and other Rigidbodies in the scene. This body goes on puppets, and only when simulating consistent collisions is important for a particular object.
