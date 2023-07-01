# CHANGELOG:

## v0.2.0 - NavMesh Support & Setup Assistant

### New Features:

* World Wrap Setup Wizard: Create a new WorldWrap project or implement WorldWrap in your existing world in seconds!
* Works with Unity’s NavMesh & NavMeshAgent
* Addition of an example Dodgeball game that uses NavMesh and NavMeshAgent
* Adds suite of unit tests to ensure changes to do affect core functionality

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
