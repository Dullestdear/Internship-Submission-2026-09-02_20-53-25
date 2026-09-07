using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // For Mouse Input
using System.Collections.Generic;
using System.Collections;
using TMPro; // For Coordinates text

// This the MAIN Script for this project . It is used for Generating the Grid , Spawning Obstacles.
// It also handles the mouse raycast movement as well as the turn based system , camera 
// and the lighting


public class GridManager : MonoBehaviour
{   
    //Base Dimensions
    [SerializeField] private Transform boardBase;

    // dimensions for bounds-checking
    public int Width => width;
    public int Height => height;

    [SerializeField] private float tileSurfaceY = 0.5f; // this should be the hieght of the tile prefab

    // to clean up the hierarchy
    private Transform tilesContainer;
    private Transform obstaclesContainer;
    private BoxCollider boardCollider;

    // Player
    [SerializeField] private PlayerMovement player;

    //Lighting
    [SerializeField] private float intensityPerTile = 25f;
    [SerializeField] private Light boardLight;


    //Camera 
    [SerializeField] private float camZoom = 0.4f;
    [SerializeField] private float offset = 0.4f; 

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
        lightAdjust(); //Calling the func to adjust light intensity
        AdjustBoardBase(); //Calling the func to Adjust back board Size
    }

    //Function to adjust wooden board size
    private void AdjustBoardBase()
    {
        if (boardBase == null) return;

        float centerX = (width - 1) / 2f;
        float centerZ = (height - 1) / 2f;

        // Center under the grid and add a 0.4 border padding
        boardBase.position = new Vector3(centerX, -0.5f, centerZ);
        boardBase.localScale = new Vector3(width + 0.4f, 1f, height + 0.4f);
    }
    //Function to spawn/generate the grid using x and z axis
    void GenerateGrid()
    {
        tiles= new Dictionary<Vector2Int, Tile>();

        // Grouping the tiles together 
        tilesContainer = new GameObject("Tiles Container").transform;
        tilesContainer.SetParent(transform);

        for(int x =0 ; x < width ; x++)
        {
            for(int z =0 ; z < height ; z++)
            {
                // Spawning the tiles
                var SpawnedTile = Instantiate(tile , new Vector3(x,0,z), Quaternion.identity
                ,tilesContainer);

                // Naming the Tiles
                SpawnedTile.name = $"x:{x},z:{z}";

                // For Pathfinding grid
                SpawnedTile.gridX = x;
                SpawnedTile.gridZ = z;

                // Creating the checkerboard pattern for the tiles
                var offset = (x%2==0&&z%2!= 0) || (x%2!=0 &&z%2==0); 
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
        // safety for no/unassigned camera
        if (Mouse.current==null || Camera.main == null) return; 

        // Click and then the player moves to that tile input code
        
        //Raycast based mouse input detectection for tile highlighting
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, tileSurfaceY, 0f));
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            // FloorToInt(pos + 0.5f) means uniform centered tile :[-0.5,0.5) to0
            int targetX = Mathf.FloorToInt(hitPoint.x + 0.5f);
            int targetZ = Mathf.FloorToInt(hitPoint.z + 0.5f);

            // to confirm coordinates are within valid grid bounds
            if (targetX >= 0 && targetX < width && targetZ >= 0 && targetZ < height)
            {
                Tile selectedTile = GetTileAtPosition(new Vector2Int(targetX, targetZ));

                if (selectedTile != null)
                {
                    if (lastSelectedTile != null && lastSelectedTile != selectedTile)
                    {
                        lastSelectedTile.ToggleHighlight(false);
                    }

                    selectedTile.ToggleHighlight(true);
                    lastSelectedTile = selectedTile;

                    coordinatesText.text =$"X: {selectedTile.gridX}  |  Z: {selectedTile.gridZ}";
                }

                // Click to execute move via TurnManager
                if (Mouse.current.leftButton.wasPressedThisFrame && lastSelectedTile != null && lastSelectedTile.isWalkable)
                {
                    if (TurnManager.Instance != null && TurnManager.Instance.CanPlayerAct)
                    {
                        Pathfinding pathfinder = new Pathfinding(tiles);
                        List<Tile> path = pathfinder.FindPath(player.currentX, player.currentZ, lastSelectedTile.gridX, lastSelectedTile.gridZ);

                        if (path != null)
                        {
                            TurnManager.Instance.PlayerMove(path);
                        }
                    }
                }
            }
            else if (lastSelectedTile != null)
            {
                lastSelectedTile.ToggleHighlight(false);
                lastSelectedTile = null;
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
        obstaclesContainer = new GameObject("Obstacles Container").transform;
        obstaclesContainer.SetParent(transform);

        foreach(Vector2Int coordinate in dataObstacle.Obstacle)
        {
            if (tiles.TryGetValue(coordinate, out  Tile  tile))
            {
                //spawning on top of the tile not inside so using y = 0.1
                Vector3 spawnPosition = tile.transform.position + new Vector3(0,0.1f,0);
                Instantiate(obstacle , spawnPosition , Quaternion.identity
                , obstaclesContainer);

                // Marking the tile as an obstacle
                tile.isWalkable = false;

                
            }
        }
    }


    private void CameraAdjust()
    {
        // Finding the centre of the Grid
        float CentreX = (float)dataObstacle.gridWidth/2f-0.5f;
        float CentreZ = (float)dataObstacle.gridHeight/2f-0.5f;

        // Isometric View 
        Camera.main.transform.rotation= Quaternion.Euler(30f,45f,0f);

        //Camera Pos.
        float dist = 20f;
        Vector3 campos = new Vector3(CentreX-dist-offset,dist, CentreZ-dist-offset);
        
        // aspect ratio so tall 5x11 grids fit on screen
        float diagonal = (width + height) * 0.7071f;
        float aspect = (float)Screen.width / Screen.height;
        float requiredSize = (diagonal / 2f) + 1f;

        Camera.main.transform.position = campos;
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = Mathf.Max(requiredSize,requiredSize/aspect);
        

    }

    private void lightAdjust()
    {
        // Finding the centre of the Grid
        float CentreX = (float)dataObstacle.gridWidth/2f-0.5f;
        float CentreZ = (float)dataObstacle.gridHeight/2f-0.5f;

        // Isometric View 
        boardLight.transform.position= new Vector3(CentreX , boardLight.transform.position.y, 
        CentreZ);

        float maxSize = Mathf.Max(dataObstacle.gridWidth, dataObstacle.gridHeight);

        //Camera Pos.
        boardLight.intensity = maxSize * intensityPerTile;
        
    }

}
