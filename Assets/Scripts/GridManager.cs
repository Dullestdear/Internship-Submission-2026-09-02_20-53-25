using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Variables deciding the dimension for the grid.
    [SerializeField] private int width;
    [SerializeField] private int height;


    // Reference to Grid Tiles
    [SerializeField] private Tile tile;

    
    void Start()
    {
        GenerateGrid(); // Calling the function to spawn grid
    }

    //Function to spawn/generate the grid using x and z axis

    void GenerateGrid()
    {
        for(int x =0 ; x < width ; x++)
        {
            for(int z =0 ; x < height ; z++)
            {
                // Spawning the tiles
                var SpawnedTile = Instantiate(tile , new Vector3(x,z), Quaternion.identity);

                // Naming the Tiles
                SpawnedTile.name = $"Tile {x} {z}";
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
