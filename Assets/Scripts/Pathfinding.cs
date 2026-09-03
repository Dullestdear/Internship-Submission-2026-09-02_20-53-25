using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pathfinding
{
    
    private Dictionary<Vector2Int , Tile> gridtiles;

    public Pathfinding(Dictionary<Vector2Int, Tile> existingGrid)
    {
        this.gridtiles = existingGrid;
    }
    
    public Tile GetTile(int x , int z)
    {
        Vector2Int coordinates = new Vector2Int(x,z);

        if (gridtiles.TryGetValue(coordinates, out Tile tile))
        {
            return tile;
            
        }
        return null;
    }
}
