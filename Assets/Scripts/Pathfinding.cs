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
    
    // Getting the Tile coordinates from my Grid
    public Tile GetTile(int x , int z)
    {
        Vector2Int coordinates = new Vector2Int(x,z);

        if (gridtiles.TryGetValue(coordinates, out Tile tile))
        {
            return tile;
            
        }
        return null;
    }

    // Using Manhatten DIstance to calculate Distance between start and End (basically H cost)

    private const int MoveCost=10;
    // The multiplication with move cost (10) is done so that the cost is integer not decimal
    private int Distcost(Tile a , Tile b)
    {
        int xdist = Mathf.Abs(a.gridX - b.gridX);
        int zdist = Mathf.Abs(a.gridZ - b.gridZ);
        return MoveCost * (xdist+zdist);
    }

    // This is for finding the tile with the least cost to move to that tile ( best tile)
    private Tile lowestFcost(List<Tile> pathnodeList)
    {
        Tile lowestFcost = pathnodeList[0];
        
        for (int i = 1 ; i < pathnodeList.Count; i++)
        {
            if (pathnodeList[i].fcost < lowestFcost.fcost)
            {
                lowestFcost = pathnodeList[i];
            }
        }
        return lowestFcost;
    }

    //Func used for Finding the path Starting from end node and going to the start node
    private List<Tile> CalculatePath(Tile end)
    {
        List<Tile> path = new List<Tile>();
        path.Add(end);
        Tile currentnode = end;

        while (currentnode.previousTile != null)
        {
            path.Add(currentnode.previousTile);
            currentnode = currentnode.previousTile;
            
        }
        
        path.Reverse();
        return path;

    }

    // func used to get the values of tiles around a particular tile (4D)
    private List<Tile> FindingNeighbours(Tile  currentnode)
    {
        //List for directions
        List<Tile> neighboursList = new List<Tile>();
        if (currentnode.gridX - 1 >= 0)
        {
            neighboursList.Add(GetTile(currentnode.gridX-1, currentnode.gridZ));
        }
        if (currentnode.gridX + 1 < 10)
        {
            neighboursList.Add(GetTile(currentnode.gridX+1, currentnode.gridZ));
        }
        if (currentnode.gridZ - 1 >= 0)
        {
            neighboursList.Add(GetTile(currentnode.gridX, currentnode.gridZ -1));
        }
        if (currentnode.gridZ + 1 < 10)
        {
            neighboursList.Add(GetTile(currentnode.gridX, currentnode.gridZ +1));
        }

        neighboursList.RemoveAll(item => item == null);
        return neighboursList;
    }
}
