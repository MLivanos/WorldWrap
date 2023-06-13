# WorldWrap
Framework for creating lightweight seamless wrapping worlds in the Unity Game Engine (WIP)

The goal of WorldWrap is to create a world that appears flat and ongoing, however if one were to walk across the entire map in a straight line, they would "wrap-around" the world and eventually come back to their initial starting point.

You can think of this as if your game world was a large planet (eg earth). Imagine you are standing in the middle of a field. The world appears like a flat plane stretching into this distance. If you threw a ball, you could perform calculations assuming you are on a flat plane and predict its trajectory with a high degree of precision. For all you know, you really are on a flat, ever stretching plane. You plant a flag at your starting point and begin walking due East. The whole time, the world appears flat, like you are continuing to walk on the plane (ignoring any mountains, valleys, or hills along the way). Eventually, however, after walking this apparently straight line, you find youself at the flag you planted at the begining of your trip. This is, of course, because you were never walking in a straight line, you were travelling on an arc stretching across a sphere.

This is the sensation we would like to impart on our user, however given that gameworlds are typically much smaller than actual planets, the curvature necessary to create such a world would be readily apparent. Here, we pose a lightweight solution to this problem through our WorldWrap system. We say that we are creating a locally-Euclidean, globally-Spherical world. It is locally Eucldiean because at any point, the player is experiencing the world on a flat plane, but when considering travelling across large distances, the world acts as if it was a sphere all along.

In brief, we accomplish this by tiling the world into discrete chunks, and then when the player moves past a specified point, the chunks rearrange themselves such that the player remains close to the center of the map throughout the experience, mimicing the effects of curvature.

<img width="866" alt="Screen Shot 2023-06-13 at 2 32 49 PM" src="https://github.com/MLivanos/WorldWrap/assets/59032623/0e09957e-c3fd-4c1d-a7e0-4e04d45881a5">

In the figure above demonstrates this idea. The figure initially is in the red square in the center (leftmost image). They move to the left and enter the blue square, continue left to the purple, and continue left to return to the initial red square.*

* This figure was created with accessibility towards people with color vision deficiency (CVD, also refered to as color blindness) in mind. Through my tools, the color-coding of each square should be distinct to people with the three major forms of CVD: protanopia, deuteranopia, and tritanopia. I do not have CVD myself, and therefore cannot confirm this. If you have any of the above forms of CVD, I'd love to hear from you to know if my attempt at accessible color-coding is successful, or if there is room for improvement.
