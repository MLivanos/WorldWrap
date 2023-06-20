<div align="center">


# WorldWrap:<br />A Lightweight Framework For<br />Creating Seamless Wrapping Worlds<br />In The Unity Game Engine<br />(v0.1.0-Alpha.1)


</div>


## Introduction


The goal of WorldWrap is to create a game world that appears flat and ongoing, however, behavior across large distances suggests that the true shape of the world is not flat.


For example, imagine you are standing in the middle of a field on a large planet (like Earth). The world appears like a flat plane stretching into this distance. If you threw a ball, you could perform calculations assuming you are on a flat plane and predict its trajectory with a high degree of precision. Now, you plant a flag at your starting point and begin walking due East. The whole time, the world appears flat (ignoring any mountains, valleys, or hills along the way), but eventually, you find yourself at the flag you planted at the beginning of your trip. This is, of course, because you were never walking in a straight line, you were traveling on an arc stretching across a sphere.


This is the sensation we would like to impart to the player, however, given that gameworlds are typically much smaller than actual planets, the curvature necessary to create such a world would be readily apparent. Here, we pose a lightweight solution to this problem through our WorldWrap system. We say that we are creating a locally-Euclidean, globally-non-Euclidean world. It is locally Euclidean because, at any point, the player is experiencing the world on a flat plane, but when considering traveling across large distances, the world appears to be a different shape (e.g. a sphere, donut, etc). WorldWrap can model a variety of non-Euclidean worlds with the current demo scene set to model walking on a donut.


We accomplish this by tiling the world into discrete chunks, and when the player moves past a specified point, the chunks rearrange themselves to conform to the desired world shape.


<img width="866" alt="Screenshot 2023-06-13 at 2 32 49 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/0e09957e-c3fd-4c1d-a7e0-4e04d45881a5">


The figure above demonstrates this idea. The figure initially is in the red square in the center (leftmost image). They move to the left and enter the blue square, continue left to the purple, and continue left to return to the initial red square.*


*This figure was created with accessibility toward people with color vision deficiency (CVD, also referred to as color blindness) in mind. Through my tools, the color coding of each square should be distinct to people with the three major forms of CVD: protanopia, deuteranopia, and tritanopia. I do not have CVD myself, and therefore cannot confirm this. If you have any of the above forms of CVD, I'd love to hear from you to know if my attempt at accessible color coding is successful, or if there is room for improvement.


## How WorldWrap Works


WorldWrap organizes the world into a matrix of tiles and, as the player walks around the world, rearranges those tiles to achieve the illusion of the game world taking place in some interesting shape.


[INSERT UML FIGURE]


UML class diagram for WorldWrap (v0.1.0-Alpha).


As demonstrated in the above figure, WorldWrap is managed by WrapManager keeps track of the tiles and aggregates information, a series of TriggerBehaviors that map where the player is and where they are going, and the blocks that tile the world. Blocks are just pieces of the world that can be picked up and dropped somewhere else. In the introduction figure (and the SampleScene), each large colored square is a block. Every block has WrapTriggers surrounding it, and when the player enters the WrapTrigger of one block and exits out of another, the WrapManager detects where that action takes place and rearranges the blocks to mimic movement on some non-Euclidean space.


In the SampleScene, if the player moves west from the red block, they end up in the blue block. Without WorldWrap, if the player continues moving west, they'd reach the end of the map and fall off. Instead, between the entrance of the blue block and the exit of the red block are two WrapTriggers. If the player enters the WrapTrigger of the red block and exits the WrapTrigger of the blue block, the WrapManager will detect that as westward motion and shift the entire game world east to compensate. The player is now in the blue block, but the blue block is now in the center of the map, the red block is to the east, and the purple block, which was previously east of the red block, is now west of the blue block.


Similarly, if the player moved north, eventually the WrapManager would shift the gameworld South. Effectively, the WrapManager keeps the player in the center of the world by moving tiles whenever it needs to, and the neighbors of every block stay the same (e.g. the red block is always east of the blue block, south of the green block, etc). To visualize the space this system is simulating, imagine rolling the world up into a cylinder, with the yellow block touching orange, blue touching purple, and gray touching white. Now, imagine twisting that cylinder such that the orange, yellow, and green blocks touch the teal gray, and white. This shape is a torus, colloquially referred to as a donut.


[INSERT TORUS MUTATION FIGURE]


There are other ways to wrap worlds that can accomplish different shapes, such as the sphere shape below, but the current version of WorldWrap is designed to mimic toric space. Stay tuned to see more, or play around with the code and try to create something new yourself!


## How To Use WorldWrap


For a full-worked example of WorldWrap, download the project and open up "SampleScene" in Unity.


The WrapManager handles how and when wraps are made. To instantiate this manager, simply attach the WrapManager.cs script to an empty game object. This object must be called "WrapManager'' for the system to work properly. This is also a good time to create a layer called "WrapLayer '' (see Limitations for a workaround in case you have run out of layers at this point).


The WrapManager will interact with your world as a series of blocks arranged in a predefined pattern. In the example project, each of these blocks is a different color and arranged in a 3x3 matrix. At the moment, WorldWrap can handle any mxn matrix such that m > 1 and n > 1. The ability to recognize more complex patterns is currently in progress.


[INSERT WRAPMANAGER INSTANTIATION FIGURE]


The empty game world with a WrapManager object.


There are three fields for you to fill under the WrapManager component. You must drag the Player game object to the "player" field, label the wrapLayer field with the integer that represents that layer, and add all blocks (in any order) into the "blocks" list. The WrapManager will automatically detect the structure and organize itself accordingly, creating the block matrix data structure behind the scenes.


[INSERT WRAPMANAGER FIELDS FIGURE]


The WrapManager filled with fields.


A block is a chunk of your game world that will be rearranged to create the wrapping effect. Every block should have its bounds surrounded by a volume with a BlockTrigger script attached to it. Typically, the BlockTrigger will be made transparent so the player cannot see these objects, but this is not required and opaque triggers may be helpful for debugging. WrapTriggers should also be placed just touching each other (but not overlapping) at the intersection of two blocks. There is no requirement for either WrapTriggers or BlockTriggers to be a certain shape, but they must have a collider, have isTrigger set to true, and be placed in the WrapLayer.


It is recommended that blocks parent static environmental pieces, such as buildings, structures, and vegetation. Other game objects do NOT have to be parented for WorldWrap to work properly.


[INSERT BLOCK AND TRIGGER FIGURE]


A block with BlockTrigger and WrapTrigger objects is set up correctly. Your setup may look different depending on how your world is created.


It is recommended that you use a BoundsTrigger object. This is an object that covers the entire game world and makes sure that objects which are not the player's are wrapping around the world properly, even when out of sight. If no such objects exist (ie the player is the only object that can move or objects only move with the player), then this is not necessary. You may choose to parent all of the blocks under the BoundsTrigger object if you would like. This helps keep the hierarchy neat but is not required.


[INSERT BOUNDS TRIGGER FIGURE]


The BoundsTrigger is set over the game world.


You are now all set up! Please thoroughly test your scene to make sure all triggers are placed appropriately so that your game functions as desired. Note that objects in the WrapLayer (other than the blocks and their children) will not move. If your game is not wrapping as desired, see the Troubleshooting section.


## Troubleshooting


As demonstrated in the SampleScene, when set up properly WorldWrap can produce the desired effect. If you experience issues, please consider the following steps to resolve them:


1. Ensure you have a WrapManager and that it is named WrapManager. If you do not, then your TriggerBehavior objects should be throwing an error at the beginning of the game.
2. Ensure you cannot bypass WrapTriggers. Check and make sure WrapTriggers cover everywhere your player can move. If they do not, then the WrapManager may not have the information needed to perform wraps. For your convenience, we provide three transparent materials for you to add on different trigger behaviors. Set them each to different colors and add some opacity to them all to visualize where your triggers are.
3. Ensure that your blocks are spaced appropriately. All the WrapManager needs is a list of blocks and it will try to match them with an appropriately shaped matrix. This is useful because it means you, as the designer, do not have to manually input coordinates. An assumption of this, however, is that blocks are spaced uniformly on a grid. If one block is at position 0,0,30 and another is at 0,0,29.997, then the WrapManager may interpret this as an extra column.
4. Ensure your global bounds encapsulate the world properly. Are non-player objects wrapping before they are supposed to? Are they falling off the world? The BoundsTrigger should have been detecting them. Make sure that there are no gaps between the BoundsTrigger and the game world, and that the BoundsTrigger is not smaller than the game world.
5. If all else fails, or if there is an exception that my code didn't raise, perhaps I've made a mistake! Please refer to the support section for further help.


## Limitations


While WorldWrap can make some impressive effects, you should be aware of the limitations of the system which we discuss here. Some of these limitations are tagged with (And Workaround), implying that there are ways to overcome those limitations.


### Theorema Egregium (Or, Imperfect Global Shapes)


It is important to note that WorldWrap will not perfectly simulate the desired shape. The section "How WorldWrap Works" claims that SampleScene mimics toric space (i.e. the world is a donut), but how true is this? There are some ways that this world is not true toric space. For one, a donut has a smaller inner circle than its outer circle, but in our simulation, one entire wrap is equal in all directions. Similarly, any shape we try to mimic will have some level of imperfection to it. In fact, this can be mathematically derived. A well-known consequence of Gauss's Theorema Egregium is that a sphere cannot be projected onto a plane without distortion.


If you believe that this is a problem, I would encourage you to play the SampleScene and ask yourself if this level of imprecision will hurt your player's user experience. If all that you are looking to do is seamlessly wrap a 3D world around itself, then this tool may be right for you. If you are trying to make a convincing non-Euclidean space, this tool may also be right for you, however, if you are trying to perfectly match the mathematical definition of a specific non-Euclidean space then this tool is not what you are looking for.


In short, WorldWrap is a great system for making believable approximations of non-Euclidean spaces, but don't try to send anyone to the moon with it.


### Performance Impact


We considered many possible approaches to this system and arrived at one we felt posed the least cost to performance while maintaining the desired effects. That being said, a wrap operation is non-trivial and scales up under certain conditions. We believe that WorldWrap is O(mn + k), where m is the number of rows, n is the number of columns, and k is the number of game objects in the scene. This is because on every wrap, all m*n blocks are moved and all k objects inside those blocks are also moved. There is further computational cost associated with TriggerBehavior objects getting and relaying information about collisions to the WrapManager.


The two-WrapTrigger system is designed to ease this impact by reducing the number of wraps needed to accomplish the desired effect. If instead, we wrapped whenever the player enters a new block, WorldWrap would need to wrap several times if the user goes back and forth between two blocks. By requiring entering one trigger and exiting another, we achieve the same effect with significantly fewer wrap operations.


Unlike approaches that make use of extra cameras, however, WorldWrap's complexity is not dependent on the number of triangles in a mesh.


### Small Worlds (And Workaround)


In small worlds with few obstacles in between, the player may be able to see WorldWrap wrapping the world in action, which can reduce immersion. If each block minus WrapTrigger length is larger than the maximum render distance, then the effect will be undetectable. Smaller worlds may require some obstacles to distract the player from the horizon (e.g. trees, hills, buildings, etc). In the SampleScene, we use cubes to block the player's view of the horizon, but one should consider this before choosing WorldWrap for their game.


### Running Out Of Layers (And Workaround)


Out of the box, WorldWrap uses a layer to separate some of its objects. Unfortunately, Unity only allows a maximum of 32 layers to be used, making this a scarce resource. As of version 0.1.0-Alpha, the layer system can be refactored by using tags instead, as Unity can support virtually as many tags as one would like. I chose to use a layer to make use of layer masks, however, I do not use layer masks as of yet. If you believe that you may need the extra layer, feel free to make this change while being reasonably confident that it will not impact any existing systems.


### Multiplayer (And Workaround)


WorldWrap assumes that there is a single player whose actions dictate how the world will wrap around itself. If two players are moving simultaneously, there is no guarantee that we can create a good wrapping effect for both players. One possible workaround for this, however, is to run WorldWrap at the application leave for every player in a game, but have simple teleporting-edge-boundaries server side. That way, consistency is maintained and every user experiences the world as if it was wrapping around them.


To be clear, WorldWrap is perfectly capable of handling single-player games with NPCs as is.


## Future Development


WorldWrap is still under development, and I'm hoping to make it much more powerful in the future. Here is a list of future improvements, but feel free to suggest more:


* Represent Spherical Space: Right now, WorldWrap only wraps around the world like a donut, however, we hope to simulate other non-Euclidean worlds soon. Spherical Space representation is currently being designed.
* Sample Games: We hope to have a few sample games for you to play in hopes of providing inspiration for future development.


## Support


To lodge a support ticket, please visit the [Issues page](https://github.com/MLivanos/WorldWrap/issues) on the project GitHub. Please be sure to include system information and how to induce the error.