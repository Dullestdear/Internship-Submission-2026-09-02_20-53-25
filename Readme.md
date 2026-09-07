# Crimson Tactics: Tactical Grid System

**Developer:** Prakhar Bhatnagar[cite: 12]  
**Unity Version:** 6000.3.7f1[cite: 12]  
**Architecture:** Event-Driven 3D Isometric Turn-Based Grid  

A 3D isometric tactical grid framework built for turn-based strategy mechanics[cite: 12]. Designed with decoupled event-driven architecture, a custom A* pathfinding system without Unity NavMesh, mathematical raycasting, dynamic grid scaling, and custom editor tooling[cite: 12].

---

## Visual Preview

<!-- ================================================================= -->
<!-- IMAGE PLACEHOLDER 1: MAIN GAMEPLAY PREVIEW                         -->
<!-- Recommended file: A 4-6 second looping GIF or high-res PNG        -->
<!-- Path: Documentation/Gameplay_Preview.gif                          -->
<!-- ================================================================= -->
![Gameplay Preview](Documentation/Gameplay_Preview.gif)
*Dynamic isometric camera framing, responsive tile coordinates, and event-driven tactical turns.*

---

## Core Architecture & Engineering Highlights

* **Event-Driven Turn Management:** The turn system uses a dedicated `TurnManager` state machine decoupled via pure C# events (`OnMovementCompleted`, `OnTurnCompleted`). Gameplay entities signal turn resolution independently, removing coroutine and polling dependencies.

* **Zero-Collider Mathematical Raycasting:** Individual `BoxCollider` components have been removed from tile prefabs to eliminate physics overhead. Pointer interaction and tile coordinates are resolved mathematically using `Plane.Raycast` against grid bounds.

* **Dynamic & Asymmetrical Grid Scaling:** Fully supports non-square grid dimensions (e.g., $5 \times 11$). The pathfinding heuristic, coordinate clamping, lighting rig, and orthographic camera bounds dynamically adjust without distortion or out-of-bounds null exceptions[cite: 11].

* **Clean Runtime Hierarchy:** All procedurally instantiated grid tiles and obstacle meshes are grouped under dedicated parent container transforms (`Tiles Container`, `Obstacles Container`), preventing root-level scene clutter.

* **Custom Pathfinding:** A custom grid-based A* algorithm handles navigation around obstacles using Manhattan distance heuristic ($H\text{-cost}$) and cumulative movement expense ($G\text{-cost}$) without relying on Unity NavMesh[cite: 12]. Traversal produces procedural hop arcs using sinusoidal vertical offsets[cite: 9, 11].

* **Enemy AI & Tactical Pacing:** The enemy unit operates autonomously, implementing `AIInterface`[cite: 11, 12]. It calculates the optimal path toward adjacent tiles around the player and incorporates a turn movement budget (`maxMoveSteps`) to prevent input locking during extended path traversals[cite: 11].

---

## How to Use the Grid & Editor Tool

<!-- ================================================================= -->
<!-- IMAGE PLACEHOLDER 2: NON-SQUARE GRID SCALING (5x11)               -->
<!-- Recommended file: Screenshot of the game running at 5x11          -->
<!-- Path: Documentation/Grid_5x11_Preview.png                         -->
<!-- ================================================================= -->
![5x11 Grid Layout](Documentation/Grid_5x11_Preview.png)
*Dynamic camera and board base automatically adapting to non-square 5x11 grid dimensions.*

### Changing the Grid Size
* In the Unity Project window, locate the `ObstacleData` ScriptableObject[cite: 12].
* In the Inspector, change the values for `Grid Width` and `Grid Height` (e.g., test with `5` and `11`)[cite: 12].
* Press **Play**. The system procedurally generates the board, dynamically scales the wooden plinth, and adjusts the orthographic camera framing and lighting to fit the layout[cite: 12].

---

<!-- ================================================================= -->
<!-- IMAGE PLACEHOLDER 3: CUSTOM OBSTACLE EDITOR WINDOW                -->
<!-- Recommended file: Screenshot of the docked Obstacle Editor tool   -->
<!-- Path: Documentation/Obstacle_Editor_Tool.png                      -->
<!-- ================================================================= -->
![Obstacle Editor Tool](Documentation/Obstacle_Editor_Tool.png)
*Custom editor matrix for painting walkable and blocked nodes directly into ScriptableObjects.*

### Using the Custom Obstacle Editor
* Open the custom Obstacle Editor window from the Unity menu bar (**Tools > Obstacle Editor**)[cite: 12].
* The tool reads the grid dimensions from the active `ObstacleData` ScriptableObject and displays an interactive grid of toggle buttons[cite: 12].
* Click any button to turn it on (blocking movement) or off (allowing movement)[cite: 12].
* The tool saves the layout directly to the ScriptableObject, allowing obstacles to appear automatically upon scene load[cite: 12].

---

## Art & Aesthetic Presentation

* **Tabletop Diorama:** The scene is framed as a physical tabletop game board with a dynamically scaled dark-walnut plinth supporting slate-gray checkerboard tiles.
* **Balanced 3-Point Lighting:** Angled directional illumination paired with soft ambient fill light ensures high tile readability and clear contact shadows without pitch-black void falloffs.
* **Minimalist HUD:** Clean, non-intrusive UI card displaying real-time coordinate tracking (`X: [val] | Z: [val]`) upon cursor hover.