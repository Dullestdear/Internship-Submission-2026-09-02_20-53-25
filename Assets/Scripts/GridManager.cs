using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // For Mouse Input
using System.Collections.Generic;
using System.Collections;
using TMPro; // For Coordinates text

public class GridManager : MonoBehaviour
{
    // Player
    [SerializeField] private PlayerMovement player;

    // Enemy
    [SerializeField] private EnemyAI enemy;

    //Variables for Spawning the obstacles 
    [SerializeField] private GameObject obstacle;
    [SerializeField] private ObstacleData dataObstacle;

    // Variables deciding the dimension for the grid.
    private int width;
    private int height;

    //Reference to the Coordinates UI
    [SerializeField] private TextMeshProUGUI coordinatesText;

    // Reference to Grid Tiles
    [SerializeField] private Tile tile;

    // Collecting/storing the tiles 
    public Dictionary<Vector2Int,Tile> tiles;

    void Start()
    {
        if (dataObstacle != null)
        {
            width = dataObstacle.gridWidth;
            height = dataObstacle.gridHeight;
        }
        GenerateGrid(); // Calling the func to spawn grid
        SpawnObstacles(); // Calling the func to spawn obsctacles
        CameraAdjust(); // Calling the func to adjust camera location based on grid  
    }

    //Function to spawn/generate the grid using x and z axis

    void GenerateGrid()
    {
        tiles= new Dictionary<Vector2Int, Tile>();
        for(int x =0 ; x < width ; x++)
        {
            for(int z =0 ; z < height ; z++)
            {
                // Spawning the tiles
                var SpawnedTile = Instantiate(tile , new Vector3(x,0,z), Quaternion.identity);

                // Naming the Tiles
                SpawnedTile.name = $"x:{x},z:{z}";

                // For Pathfinding grid
                SpawnedTile.gridX = x;
                SpawnedTile.gridZ = z;

                // Creating the checkerboard pattern for the tiles
                var offset = (x%2 == 0 && z%2 != 0) || (x%2 != 0 && z%2 == 0); 
                SpawnedTile.Init(offset);

                //Storing the spawned tiles
                tiles[new Vector2Int(x,z)] = SpawnedTile;
            }
        }
    }

    // Collecting the tile from the current position
    public Tile GetTileAtPosition(Vector2Int pos)
    {
        if (tiles.TryGetValue(pos , out var tile))
        {
            return tile;
        }


        return null;
        
    }


     

    private Tile lastSelectedTile;

    void Update()
    {

        // Click and then the player moves to that tile input code
        if (Mouse.current.leftButton.wasPressedThisFrame && lastSelectedTile != null)
        {
            if(player != null && !player.isMoving && (enemy==null || !enemy.isMoving))
            {
                Pathfinding pathfinder = new Pathfinding(tiles);
                List<Tile> path = pathfinder.FindPath(player.currentX,player.currentZ
                ,lastSelectedTile.gridX,lastSelectedTile.gridZ);

                // MOOOOVE IT
                if (path != null)
                {
                    StartCoroutine(TurnSystem(path));
                }
            }
        }

        //Raycast based mouse input detectection for tile highlighting
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray,out RaycastHit hit))
        {
            Tile selectedTile = hit.collider.GetComponent<Tile>();
            

            if (selectedTile != null)
            {
                if (lastSelectedTile != null) lastSelectedTile.ToggleHighlight(false);
                selectedTile.ToggleHighlight(true);
                lastSelectedTile = selectedTile;

                // Name of the tile (coordinates) ( for Displaying on screen)
                coordinatesText.text = $"{selectedTile.name}";
                
            }
        }
        
        else if (lastSelectedTile != null)
        {   
            // Turning the highlight off when the cursor is not on the grid
            lastSelectedTile.ToggleHighlight(false);
            lastSelectedTile = null;

        }
    }

    void SpawnObstacles()
    {
        foreach(Vector2Int coordinate in dataObstacle.Obstacle)
        {
            if (tiles.TryGetValue(coordinate, out  Tile  tile))
            {
                //spawning on top of the tile not inside so using y = 0.1
                Vector3 spawnPosition = tile.transform.position + new Vector3(0,0.1f,0);
                Instantiate(obstacle , spawnPosition , Quaternion.identity);

                // Marking the tile as an obstacle
                tile.isWalkable = false;

                
            }
        }
    }

    private IEnumerator TurnSystem(List<Tile> path)
    {
        //Players turn to move 
        player.MoveAlongPath(path);

        //pause while moving
        while (player.isMoving)
        {
            yield return null;
        }

        //Enemy turn to move
        enemy.RunTurn();
    }

    private void CameraAdjust()
    {
        // Finding the centre of the Grid
        float CentreX = (float)dataObstacle.gridWidth/2f-0.5f;
        float CentreZ = (float)dataObstacle.gridHeight/2f-0.5f;

        //Camera Pos.
        float maxSize = Mathf.Max(dataObstacle.gridWidth,dataObstacle.gridHeight);
        Vector3 campos = new Vector3(CentreX, maxSize*1.2f, CentreZ-(maxSize*0.8f));
        Camera.main.transform.position = campos;
        Camera.main.transform.LookAt(new Vector3(CentreX,0,CentreZ));

    }

}
