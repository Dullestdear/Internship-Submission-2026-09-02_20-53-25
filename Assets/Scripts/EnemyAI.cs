using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, AIInterface
{
    [SerializeField] private float speed = 5f;
    public bool isMoving{ get; private set;}

    //Player and Grid 
    [SerializeField] private GridManager grid;
    [SerializeField] private PlayerMovement player;
    
    //Coordinates
    public int currentX;
    public int currentZ;

    //Inherited from AI Interface 
    public void RunTurn()
    {
        Tile targetTile = CTPtiles(); // Tiles close to the player in either of the 4 directions
        
        if (targetTile != null)
        {
            //Pathfinding algo will run here
            Pathfinding pathfinder = new Pathfinding(grid.tiles);
            List<Tile> path = pathfinder.FindPath(currentX,currentZ,
            targetTile.gridX,targetTile.gridZ);

            // MOOOOVE IT
            if (path !=null && path.Count > 0)
            {
                StartCoroutine(Move(path));
            }
        }
    }

    //func to find the tiles close to the player
    private Tile CTPtiles()
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0,1),new Vector2Int(0,-1),new Vector2Int(-1,0),new Vector2Int(1,0)
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int CheckPosition = new Vector2Int(player.currentX+dir.x, player.currentZ +dir.y);
            Tile neighbouringTile = grid.GetTileAtPosition(CheckPosition);

            if (neighbouringTile != null && neighbouringTile.isWalkable)
            {
                return neighbouringTile;
            }
        }
        return null;
    }

    private IEnumerator Move(List<Tile> path)
    {
        isMoving = true; // did this to make sure that the enemy is currently moving

        

        // Code to go through each tile one by one
        foreach (Tile targetTile in path)
        {
            // spawning 0.1f above so that the player doesnt spawn inside the tile
            Vector3 targetPosition = targetTile.transform.position+ new Vector3(0,0.1f,0);

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position , targetPosition 
                , speed*Time.deltaTime);
                yield return null;
            }

            // Exact Position in integers
            transform.position = targetPosition;

            

        }
        isMoving = false;
    }
}
