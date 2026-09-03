using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding
{
    
    private Dictionary<Vector2Int , Tile> gridtiles;

    public Pathfinding(Dictionary<Vector2Int, Tile> existingGrid)
    {
        this.gridtiles = existingGrid;
    }

    public List<Tile> FindPath(int startX , int StartZ , int endX , int endZ)
    {
        // Starting tile and ending tile
        Tile startNode = GetTile(startX,StartZ);
        Tile endNode = GetTile(endX,endZ);

        List<Tile> openList = new List<Tile> {startNode};
        List<Tile> closedList = new List<Tile>();

        //Refreshing before the calc
        foreach(var i in gridtiles)
        {
            Tile pathNode = i.Value;
            pathNode.gcost = 9999999;
            pathNode.previousTile = null;
        }

        //First Node
        startNode.gcost=0;
        startNode.hcost = Distcost(startNode, endNode);

        //Searching algo
        while (openList.Count > 0)
        {
            //Best Tile
            Tile currentNode = lowestFcost(openList);

            // if Goal reached
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            // Checked nodes go to closed from open
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Checking all 4 directions
            foreach (Tile neighbourNode in FindingNeighbours(currentNode))
            {
                // go over obstacles and alreadt visted nodes
                if ( closedList.Contains(neighbourNode) || !neighbourNode.isWalkable)
                {
                    continue;
                }

                // Cost to move
                int MoveGcost = currentNode.gcost + Distcost(currentNode, neighbourNode);

                // Checking whichever is faster
                if (MoveGcost < neighbourNode.gcost)
                {
                    neighbourNode.previousTile = currentNode;
                    neighbourNode.gcost = MoveGcost;
                    neighbourNode.hcost = Distcost(neighbourNode, endNode);
                    
                    // Add to open list if it is faster so that it can check again
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }

            }
        }
        return null;

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

        // left ,right,up,down
        Tile left = GetTile(currentnode.gridX-1 , currentnode.gridZ);
        Tile right = GetTile(currentnode.gridX+1 , currentnode.gridZ);
        Tile up = GetTile(currentnode.gridX , currentnode.gridZ +1);
        Tile down = GetTile(currentnode.gridX , currentnode.gridZ-1);

        //if train to add tiles
        if (left != null)
        {
            neighboursList.Add(left);
        }
        if (right != null)
        {
            neighboursList.Add(right);
        }
        if (up != null)
        {
            neighboursList.Add(up);
        }
        if (down != null)
        {
            neighboursList.Add(down);
        }
        return neighboursList;
    }
}
