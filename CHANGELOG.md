# CHANGELOG:

## v0.3.0 - Multiplayer Support

### New Features:

* Multiplayer: WorldWrap is now multiplayer capable. WorldWrap will maintain wrapping worlds for all players, NPCs, and GameObjects while being completely consistent between clients.
* Multiplayer dodgeball game provided for an example of how to create multiplayer games using WorldWrap
* SelfWrap: Objects that exit the BoundsTrigger from the top will govern their own wrapping until they reenter the BoundsTrigger. Applied automatically, no changes need to be made to existing code. Tangibly, what this means is that objects can fly arbitrarily high in the sky and wrap without issue.
* SafetyTrigger: TriggerBehavior which forces a wrap when the player exits outside its bounds. Useful for games with teleporting mechanics or to fix glitches caused by lag or improperly constructed WrapTriggers

### Bug Fixes:

* Fixes issue with automatic NavMeshLure creation where NavMeshLure would be placed slightly off or into the world

### Changes:

* BoundsTrigger's GetNewPosition() function now maps any point in Euclidean space to WrapSpace, even those arbitrarily far away.
* Adds icons to all WorldWrap classes for easier visualization in the scene editor

### Removed:

## v0.2.0 - NavMesh Support & Setup Assistant

### New Features:

* WorldWrap Setup Wizard: Create a new WorldWrap project or implement WorldWrap in your existing world in seconds!
* Works with Unity’s NavMesh & NavMeshAgent
* Addition of an example Dodgeball game that uses NavMesh and NavMeshAgent
* Adds suite of unit tests to ensure changes to do affect core functionality
* Adds scale tests that display the framerate cost of our method and allow the user to add more objects to the scene.

### Bug Fixes:

* Fixes issue where sphere and capsule colliders can be asked to wrap twice due to their colliders having multiple contact points
* Fixes issue where the player may wrap after exiting the same trigger they entered
* Fixes issue where objects could be shot off the map if their collider was between two blocks when wrapping
* Fixes issue where references to deleted objects would be kept by the block trigger.
* Fixes issues faced when making non-square worlds
* Fixes issues faced when wrapping diagonally

### Changes:

* No longer requires a WorldWrapObjects layer and instead uses a “WorldWrapObject” tag

### Removed:

## v0.1.0

* Introducing WorldWrap: A Lightweight Framework For Creating Seamless Wrapping Worlds In The Unity Game Engine. See the documentation for the full feature list
