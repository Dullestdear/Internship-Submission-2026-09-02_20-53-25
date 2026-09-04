# Crimson Tactics: Tactical Grid System

**Developer:** Prakhar Bhatnagar

**Unity Version:** 6000.3.7f1

I built 3D isometric tactical grid system built specifically for turn-based strategy games. This project meets all the assignment requirements focusing on a system architecture, a custom pathfinding solution and strong editor tools. The design also emphasizes a visual presentation.

## Core Architecture

* **Dynamic Grid Generation:** The system creates a grid procedurally with a default size of 10x10 tiles. It includes real-time tracking of tile coordinates in the UI when the player hovers over a tile with the mouse.

* **Custom Pathfinding:** A custom grid-based pathfinding algorithm handles navigation around obstacles. Unity’s built-in pathfinding tools are not used at all. The player cannot input commands while a unit is moving.

* **Enemy AI:** The enemy unit operates autonomously. Inherits from an `AIInterface`. It calculates the path to one of the four tiles adjacent to the player then waits for the player’s turn before moving again.

## How to Use the Grid & Editor Tool

**Changing the Grid Size**

* In the Unity Project window find the `ObstacleData` Scriptable Object.

* In the Inspector change the values for `Grid Width` and `Grid Height`.

* Press Play. The system will automatically generate the board. The camera and lighting scripts adjust the zoom and spotlight intensity to fit the new layout perfectly.

**Using the Custom Obstacle Editor**

* Open the custom Obstacle Editor window from the menu.

* The tool reads the grid dimensions from the Scriptable Object. Shows a grid of buttons that can be toggled.

* Click any button to turn it on (blocking movement) or off (allowing movement).

* The tool saves the layout directly to the Scriptable Object. When you press Play obstacles appear where you placed them.

## Art. Aesthetic

* **Theatrical Tabletop:** The scene is set inside a pitch- void with deep linear fog. A harsh overhead spotlight shines down. Moves dynamically to center the action drawing attention directly to the tactical gameplay.

