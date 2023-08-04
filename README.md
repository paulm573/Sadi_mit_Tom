# Introduction

We made a small Game. The idea was to create a real World which transfers flawless into a virtual one. This Project was mainly done to proove the concept.
Next ideas would be to take this and turn it into something like a Tower Defense or puzzle games. Strategegy games would be alswo awesome.
So we took our Lego-Tiles and built a world. Then we wrote an applicatition to have an virtual adventure, which can be explored.
Just clone this repo and throw it into unity.
We have an example prepaired.
The normal entry point is the "homScreen"(s. below).

In case you dont have a lego-plate (or cant take a picture);
->Open the maze scene
->replace the string "currentMaze" from the variable "Generate Maze path" to "worldForTesting" and you will get the world, shown in the video.
->The variable you find at the GamaManager Gameobject.
->Now press Play
-> or draw a maze and try that :) but we cant ensure, that that will work flawless.

## Requirements
-> A Webcam or android device
-> A lego-Plate and Tiles

## Features:
### HomeScreen:
A StartScreen which accesses the Device Camera (in unity Editor the webcam)
Functionality which creates the picture and saves it for later use.
Extra Canvas to preview the taken picture and the functionality to start over 
On Click okay it will open the next scene.
Theres an Exitbuttons as well, of course.
### Maze:
On Enter the Scene the App will turn each lego-nipple into a gameobject.
Then a Player will be spawned with which you can explore your built world, started from Lego.
Collect the Coins to gether some points. Its in your hand if you can achieve a new highscore, based on the world u built bevore :)
If you have enough, or like to start over, just press the leave button and return to the Homesceen.

In Unity you can configure the World as you like by adjusting the used assets, changing the size of the Lego-Plate and so on.
At the moment its configured like this:
red = coins
white = walls
grey = floor
blue = secondary walls
green = trees


An example Video is provided in the Asset folder.
