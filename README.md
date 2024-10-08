<div align="center">


# WorldWrap:<br />A Lightweight Framework For<br />Creating Seamless Wrapping 3D Worlds<br />In The Unity Game Engine<br />(v1.0.0)


<img width="1000" alt="WorldWrapLogo" src="https://github.com/MLivanos/WorldWrap/assets/59032623/146963a8-7c19-47e8-b219-a60bc086badd">


![Static Badge](https://img.shields.io/badge/Version-1.0.0-blue?style=flat-square)
![GitHub License](https://img.shields.io/github/license/MLivanos/WorldWrap?style=flat-square&link=https%3A%2F%2Fimg.shields.io%2Fgithub%2Flicense%2FMLivanos%2FWorldWrap%3Flink%3Dhttps%253A%252F%252Fgithub.com%252FMLivanos%252FWorldWrap%252Fblob%252FDOCS-MultiplayerREADMEUpdate%252FLICENSE)


![Static Badge](https://img.shields.io/badge/Unit%20Tests-48%2F48%20Passed-brightgreen?style=flat-square&link=https%3A%2F%2Fgithub.com%2FMLivanos%2FWorldWrap%2Ftree%2Fmain%2FWorldWrap%2FAssets%2FTests%2FScripts%2FUnitTests)
![Static Badge](https://img.shields.io/badge/Code%20Coverage-100%25-brightgreen?style=flat-square)
![Code Climate maintainability](https://img.shields.io/codeclimate/maintainability-percentage/MLivanos/WorldWrap?style=flat-square&logo=code%20climate)
![Static Badge](https://img.shields.io/badge/Snyk%20Security-Monitored-8042d1?style=flat-square&logo=snyk)


![GitHub last commit (branch)](https://img.shields.io/github/last-commit/MLivanos/WorldWrap/dev?style=flat-square)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/MLivanos/WorldWrap?style=flat-square)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/t/MLivanos/WorldWrap?style=flat-square)

[![Static Badge](https://img.shields.io/badge/Ko--fi-F16061?style=for-the-badge&logo=ko-fi&logoColor=white)](https://ko-fi.com/michaellivanos)
[![Static Badge](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/watch?v=MyriUorYnZM)
![GitHub Repo stars](https://img.shields.io/github/stars/MLivanos/WorldWrap?style=for-the-badge&logo=github&color=gold)

All past, current, and future iterations of WorldWrap are/will be free, but if you'd like, you can send a tip through my [Ko-fi](https://ko-fi.com/michaellivanos)

</div>

- [WorldWrap:A Lightweight Framework ForCreating Seamless Wrapping 3D WorldsIn The Unity Game Engine(v1.0.0)](#worldwrapa-lightweight-framework-forcreating-seamless-wrapping-3d-worldsin-the-unity-game-enginev100)
  - [Introduction](#introduction)
    - [0.3.0 Release Notes](#030-release-notes)
    - [0.2.0 Release Notes](#020-release-notes)
  - [How WorldWrap Works](#how-worldwrap-works)
  - [How To Use WorldWrap](#how-to-use-worldwrap)
  - [Unit Tests](#unit-tests)
  - [Multiplayer](#multiplayer)
  - [Troubleshooting](#troubleshooting)
  - [Limitations](#limitations)
    - [Theorema Egregium (Or, Imperfect Global Shapes)](#theorema-egregium-or-imperfect-global-shapes)
    - [Performance Impact](#performance-impact)
    - [Small Worlds (And Workaround)](#small-worlds-and-workaround)
  - [Future Development](#future-development)
  - [Support](#support)

## Introduction


Imagine you are standing in the middle of a field. The world appears like a flat plane stretching into this distance. Now, you plant a flag at your starting point and begin walking due East. The whole time, the world appears flat, but eventually, you find yourself at the flag you planted at the beginning of your trip. This is, of course, because you were never walking in a straight line on a flat plane, you were traveling on an arc stretching across a sphere.

How can we impart this feeling to the users of our games? We could create a curved world or make our game world on a sphere, but unless we make that world truly massive, the world will appear curved the whole time. We could teleport the user to the other end of the map when they walk around, but this would be jarring. Instead, WorldWrap accomplishes this effect by keeping the world as a series of tiles called blocks. When the user exits one block and enters another, the blocks rearrange themselves, any objects inside of them, and any Rigidbody objects moving between them to simulate the world wrapping around.

<div align="center">
  
<img width="866" alt="Screenshot 2023-06-13 at 2 32 49 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/0e09957e-c3fd-4c1d-a7e0-4e04d45881a5">

</div>

The figure above demonstrates this idea. The figure initially is in the red square in the center (leftmost image). They move to the left and enter the blue square, continue left to the purple, and continue left to return to the initial red square.*


*This figure was created with accessibility toward people with color vision deficiency (CVD, also referred to as color blindness) in mind. Through my tools, the color coding of each square should be distinct to people with the three major forms of CVD: protanopia, deuteranopia, and tritanopia. I do not have CVD myself, and therefore cannot confirm this. If you have any of the above forms of CVD, I'd love to hear from you to know if my attempt at accessible color coding is successful, or if there is room for improvement.

Some other reasons to consider WorldWrap for your next game:

💸 Always free - You can use, copy, modify, publish, and sell any project using WorldWrap, 100% free of charge.

🚀 Performance - WorldWrap has minimal impact on performance and maintains high framerates even in complex worlds with thousands of game objects.

🎮 Cross-Platform - WorldWrap can be used on all platforms from mobile games to consoles.

🚧 No More Invisible Walls - How many times have you run into an immersion-breaking invisible wall in a game? Add a unique twist to your game by removing them entirely.

🤖 Pathfinding AI - NPCs can navigate WorldWrap perfectly without extra effort on your part.

🛜 Multiplayer Support - WorldWrap is now completely multiplayer capable. The software takes care of the fact that multiple players may wrap in different directions and maintains the effect for any number of players

🌐 Open-Source - All code is available for free on my GitHub.

🛠 Developer Friendly - From the Setup Assistant that will help you make your world ready to wrap in minutes to developer tools such as unit tests, WorldWrap is made with the developer's interests in mind.

### 0.3.0 Release Notes

The 0.3.0 release fully integrates multiplayer support! Every player will see the wrapping effect on their own end while maintaining all objects' relative positions. There are some other stability and power enhancements, so make sure to check the Change Log for an exhaustive list.

### 0.2.0 Release Notes

The 0.2.0 release allows WorldWrap to be used with Unity's NavMesh pathfinding AI, adds the Setup Assistant, and fixes bugs present in 0.1.x. See the Change Log for full details.

## How WorldWrap Works

WorldWrap organizes the world into a matrix of tiles and, as the player walks around the world, rearranges those tiles to achieve the illusion of the game world taking place in some interesting shape.

<div align="center">

<img width="1053" alt="Screen Shot 2023-06-19 at 2 13 54 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/32ef6218-799e-4dd5-bf06-7deb962800f5">

</div>

UML class diagram for the WorldWrap Demo scene (v0.2.0).

As demonstrated in the above figure, WorldWrap is managed by WrapManager keeps track of the tiles and aggregates information, a series of TriggerBehaviors that map where the player is and where they are going, and the blocks that tile the world. Blocks are just pieces of the world that can be picked up and dropped somewhere else. In the introduction figure (and the Demo scene), each large colored square is a block. Every block has WrapTriggers surrounding it, and when the player enters the WrapTrigger of one block and exits out of another, the WrapManager detects where that action takes place and rearranges the blocks to mimic movement on some non-Euclidean space.

In the Demo scene, if the player moves west from the red block, they end up in the blue block. Without WorldWrap, if the player continues moving west, they'd reach the end of the map and fall off. Instead, between the entrance of the blue block and the exit of the red block are two WrapTriggers. If the player enters the WrapTrigger of the red block and exits the WrapTrigger of the blue block, the WrapManager will detect that as westward motion and shift the entire game world east to compensate. The player is now in the blue block, but the blue block is now in the center of the map, the red block is to the east, and the purple block, which was previously east of the red block, is now west of the blue block.

Similarly, if the player moved north, eventually the WrapManager would shift the gameworld South. Effectively, the WrapManager keeps the player in the center of the world by moving tiles whenever it needs to, and the neighbors of every block stay the same (e.g. the red block is always east of the blue block, south of the green block, etc). To visualize the space this system is simulating, imagine rolling the world up into a cylinder, with the yellow block touching orange, blue touching purple, and gray touching white. Now, imagine twisting that cylinder such that the orange, yellow, and green blocks touch the teal gray, and white. This shape is a torus, colloquially referred to as a donut.

<div align="center">

<img width="621" alt="Screen Shot 2023-06-19 at 10 49 36 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/d80138b0-d1cc-4f28-a603-a1fba6c27d05">

</div>

The transformation from our 3x3 grid game world to a Torus.


There are other ways to wrap worlds that can accomplish different shapes, such as the sphere shape below, but the current version of WorldWrap is designed to mimic toric space. Stay tuned to see more, or play around with the code and try to create something new yourself!


## How To Use WorldWrap

For a full-worked example of WorldWrap, download the project and open up "Demo" in Unity. For a full-worked example of WorldWrap using NavMesh, open "DodgeballGame".

We recommend using the WorldWrap Setup Assistant to automatically set up your world. Open the Assistant via Window->WorldWrap. Because the world is wrapping, global bounds must be created. Enter the x,y, and z size of your world in the top field, followed by the number of rows (x-axis) and columns (z-axis). If you plan on using Unity's built-in pathfinding AI (NavMesh), please check the "Using NavMesh" box and click "Setup WorldWrap". This will create a WrapManager (handles the wrapping effect for the player), GlobalBounds (handles the wrapping effect for NPCs and moving objects), Blocks as empty GameObjects (the units which will move to accomplish the wrap), and BlockTriggers (keeps track of the position of RigidBody objects), and WrapTriggers (registers player's motion and communicates with the WrapManager to initiate wraps where appropriate). If you selected "Using NavMesh", a NavMeshLure object with four planes will be created. At runtime, the WrapManager will use this to tell the pathfinding AI that the world wraps around. You do not need to interact with the NavMeshLure.

After these objects are set up, add your terrain to the scene, being sure to split the terrain up into tiles that span the volume of each BlockTrigger. Be sure to add terrain and other static objects (eg buildings, vegetation, terrain) under their corresponding block. Anything that will move across the scene (eg players, NPCs, projectiles) should have both collider and RigidBody components.

You may also want to resize, reconfigure, and remove some WrapTriggers. Particularly, the end of a WrapTrigger should represent when the player is firmly "inside" the next block. Wraps occur when the player enters one WrapTrigger, moves into another WrapTrigger, and exits into another block. Therefore, the smaller WrapTrigger the more wraps will occur. Since wraps can be expensive, it is generally advisable to make WrapTriggers larger, but this is dependent on your specific world. See the demo scenes for examples of WrapTriggers being sized to a world.

There are three fields for you to fill under the WrapManager component. The Setup Assistant will do its best to fill these in for you. The "Blocks" field should be set up with all of the blocks in the scene, however, they do not need to be in any particular order. The WrapManager will automatically detect the structure of your world. The Setup Assistant will also try to find your player character by finding the GameObject in the scene with the smallest [Hamming distance](https://en.wikipedia.org/wiki/Hamming_distance) to the word "Player" (not case sensitive, null characters added when strings are of different size). Please ensure this field has your player's GameObject, as the Assistant can make a mistake. Please examine any warnings or errors the Setup Assistant shows.

## Unit Tests

WorldWrap contains dozens of unit tests that you can use to verify that any changes you make to the code do not break anything elsewhere in the code. At time of writing, WorldWrap contains 48 unit tests covering 100% of the code*. These include tests for the wrapping behavior of the player, wrapping behavior of NPC's, objects' relative positions, the SafetyTrigger, the SelfWrap component, and NavMesh compatibility. Note that, to best simulate actual use, we employ a scripted series of events that unfold in a scene in real time. These rely on timers, and therefore may give inconsistent results. While we encourage anyone trying to extend WorldWrap for their own purposes to thoroughly validate their code, a unit test occasionally failing while generally passing is not necessarily an indication that the feature is broken.

PHOTOSENSITIVE DEVELOPERS: Because the Unit Tests are testing a large number of wraps in quick succession, the screen will, at times, flash at ~20 Hz. A flash warning will appear on the screen during unit tests for this reason.

*Code coverage is calculated on lines of code covered, and only reflects the single-player WorldWrap classes.

<div align="center">

<img width="1680" alt="Screen Shot 2023-06-19 at 10 53 15 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/ab34d4b9-a80f-473d-a672-ba3a85c4b3e2">

</div>

The WrapManager filled with fields.

You are now all set up! Please thoroughly test your scene to make sure all triggers are placed appropriately so that your game functions as desired. Note that objects with the WrapLayer tag (other than the blocks and their children) will not move. If your game is not wrapping as desired, see the Troubleshooting section.

## Multiplayer

*HOW TO USE MULTIPLAYER*

<div align="center">
  
<img width="646" alt="Screenshot 2023-07-23 at 10 46 50 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/39365ca0-0cf8-4256-a113-58a6ce40f22e">

</div>

Diagram of how network objects are instantiated behind the scenes. Note that this process is accomplished via a single function call (WorldWrapNetworkManager.InstantiateOnNetwork()) from the perspective of a developer, and this process is the same for instantiating a player object when a new client connects.

<div align="center">

<img width="646" alt="Screenshot 2023-07-23 at 10 47 07 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/5bceff15-ee48-432c-830b-766af0f86548">

</div>

A diagram of how motion is logged and transforms updated. Note that this is automatically tracked by the WorldWrapNetworkManager.

## Troubleshooting

As demonstrated in the Demo scene and Dodgeball sample game, when set up properly WorldWrap can produce the desired effect. If you experience issues, please consider the following steps to resolve them:

1. Ensure you have a WrapManager and that it is named WrapManager. If you do not, then your TriggerBehavior objects should be throwing an error at the beginning of the game.
2. Ensure you cannot bypass WrapTriggers. Check and make sure WrapTriggers cover everywhere your player can move. If they do not, then the WrapManager may not have the information needed to perform wraps. For your convenience, we provide three transparent materials for you to add on different trigger behaviors. Set them each to different colors and add some opacity to them all to visualize where your triggers are.
3. Ensure that your blocks are spaced appropriately. All the WrapManager needs is a list of blocks and it will try to match them with an appropriately shaped matrix. This is useful because it means you, as the designer, do not have to manually input coordinates. An assumption of this, however, is that blocks are spaced uniformly on a grid. If one block is at position 0,0,30 and another is at 0,0,29.997, then the WrapManager may interpret this as an extra column.
4. Ensure your global bounds encapsulate the world properly. Are non-player objects wrapping before they are supposed to? Are they falling off the world? The BoundsTrigger should have been detecting them. Make sure that there are no gaps between the BoundsTrigger and the game world, and that the BoundsTrigger is not smaller than the game world.
5. If all else fails, or if there is an exception that my code didn't raise, perhaps I've made a mistake! Please refer to the support section for further help.

## Limitations

While WorldWrap can make some impressive effects, you should be aware of the limitations of the system which we discuss here. Some of these limitations are tagged with (And Workaround), implying that there are ways to overcome those limitations. Some limitations are also labeled with a future release number, as I intend to overcome them by that release.

### Theorema Egregium (Or, Imperfect Global Shapes)


It is important to note that WorldWrap will not perfectly simulate the desired shape. The section "How WorldWrap Works" claims that Demo scene mimics toric space (i.e. the world is a donut), but how true is this? There are some ways that this world is not true toric space. For one, a donut has a smaller inner circle than its outer circle, but in our simulation, one entire wrap is equal in all directions. Similarly, any shape we try to mimic will have some level of imperfection to it. In fact, this can be mathematically derived. A well-known consequence of Gauss's Theorema Egregium is that a sphere cannot be projected onto a plane without distortion.


If you believe that this is a problem, I would encourage you to play the Demo scene and ask yourself if this level of imprecision will hurt your player's user experience. If all that you are looking to do is seamlessly wrap a 3D world around itself, then this tool may be right for you. If you are trying to make a convincing non-Euclidean space, this tool may also be right for you, however, if you are trying to perfectly match the mathematical definition of a specific non-Euclidean space then this tool is not what you are looking for.


In short, WorldWrap is a great system for making believable approximations of non-Euclidean spaces, but don't try to send anyone to the moon with it.


### Performance Impact


We considered many possible approaches to this system and arrived at one we felt posed the least cost to performance while maintaining the desired effects. That being said, a wrap operation is non-trivial and scales up under certain conditions. We believe that WorldWrap is O(mn + k), where m is the number of rows, n is the number of columns, and k is the number of game objects in the scene. This is because on every wrap, all m*n blocks are moved and all k objects inside those blocks are also moved. There is further computational cost associated with TriggerBehavior objects getting and relaying information about collisions to the WrapManager.


The two-WrapTrigger system is designed to ease this impact by reducing the number of wraps needed to accomplish the desired effect. If instead, we wrapped whenever the player enters a new block, WorldWrap would need to wrap several times if the user goes back and forth between two blocks. By requiring entering one trigger and exiting another, we achieve the same effect with significantly fewer wrap operations.


Unlike approaches that make use of extra cameras, however, WorldWrap's complexity is not dependent on the number of triangles in a mesh.


### Small Worlds (And Workaround)


In small worlds with few obstacles in between, the player may be able to see WorldWrap wrapping the world in action, which can reduce immersion. If each block minus WrapTrigger length is larger than the maximum render distance, then the effect will be undetectable. Smaller worlds may require some obstacles to distract the player from the horizon (e.g. trees, hills, buildings, etc). In the Demo scene, we use cubes to block the player's view of the horizon, but one should consider this before choosing WorldWrap for their game.

## Future Development


WorldWrap is still under development, and I'm hoping to make it much more powerful in the future. Here is a list of future improvements, but feel free to suggest more:


* After the multiplayer update, I do not have new features planned. Let me know if there is any functionality you would like to see!
* I'll be working on making some tutortial videos for making both single and multiplayer games. Hopefully this will be helpful if you'd like to use the system in your own projects!
* I'm also going to be making some games using the software to hopefully give people some inspiration.


## Support


To lodge a support ticket, please visit the [Issues page](https://github.com/MLivanos/WorldWrap/issues) on the project GitHub. Please be sure to include system information and how to induce the error.
