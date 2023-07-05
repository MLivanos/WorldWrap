# **WorldWrap: Programming API**

## [WrapManager : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/DOCS-APIDocumentation/API/WrapManager.md)

The WrapManager handles information from various trigger behaviors and is responsible for determining when a wrap is required, rearranging blocks after wraps, and performaning the wraps. There should be exactly 1 (one) wrap manager per scene which requires wraps.

## [TriggerBehavior : MonoBehaviour](https://github.com/MLivanos/WorldWrap/blob/DOCS-APIDocumentation/API/TriggerBehavior.md)

Abstract class from which WrapTriggers, BoundsTrigger, and BlockTriggers inherit from. Finds the WrapManager in the hierachy.

## [BoundsTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/DOCS-APIDocumentation/API/BoundsTrigger.md)

Trigger wraps helps NPCs and objects around the world when they reach the end.

## [BlockTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/DOCS-APIDocumentation/API/BlockTrigger.md)

Trigger that informs the WrapManager about where the player is and object relative positions.

## [WrapTrigger : TriggerBehavior](https://github.com/MLivanos/WorldWrap/blob/DOCS-APIDocumentation/API/WrapTrigger.md)

Trigger that informs the WrapManager of when the player is leaving and entering a block.