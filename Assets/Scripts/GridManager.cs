using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // For Mouse Input
using System.Collections.Generic;
using TMPro; // For Coordinates text

public class GridManager : MonoBehaviour
{
    // Variables deciding the dimension for the grid.
    [SerializeField] private int width;
    [SerializeField] private int height;

    //Reference to the Coordinates UI
    [SerializeField] private TextMeshProUGUI coordinatesText;

    // Reference to Grid Tiles
    [SerializeField] private Tile tile;

    // Collecting/storing the tiles 
    private Dictionary<Vector2Int,Tile> tiles;

    void Start()
    {
        GenerateGrid(); // Calling the function to spawn grid
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


    //Raycast based mouse input detectection for tile highlighting 

    private Tile lastHoveredTile;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile hoveredTile = hit.collider.GetComponent<Tile>();
            
            if (hoveredTile != null)
            {
                // triggered if the cursor moved to a new tile
                if (lastHoveredTile != hoveredTile)
                {
                    if (lastHoveredTile != null) lastHoveredTile.ToggleHighlight(false);
                    
                    hoveredTile.ToggleHighlight(true);
                    lastHoveredTile = hoveredTile;

                    // UI Element
                    coordinatesText.text = $"{hoveredTile.name}";
                }
            }
        }
        else if (lastHoveredTile != null)
        {
            // Turning off the highlight if mouse moves off the grid entirely
            lastHoveredTile.ToggleHighlight(false);
            lastHoveredTile = null;
        }
    }

}
