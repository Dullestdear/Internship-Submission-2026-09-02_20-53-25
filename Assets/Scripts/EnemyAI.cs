using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, AIInterface
{
    public bool isMoving{ get; private set;}

    //Player and Grid 
    [SerializeField] private GridManager grid;
    [SerializeField] private PlayerMovement player;
    
    //Coordinates
    public int currentX;
    public int currentZ;

    //Enemy Movement
    private IEnumerator Start()
    {
        yield return null; // used here for waiting a single frame

        //Spawning of the enemy (using coords)
        Vector2Int StartingPosition = new Vector2Int(currentX , currentZ);
        Tile startTile = grid.GetTileAtPosition(StartingPosition);

        transform.position = startTile.transform.position + new Vector3(0, 0.1f, 0);
    }

    void Update()
    {
        if (isMoving)
        {
            //Code for making the enemy look at the player
            Vector3 directionToPlayer =(player.transform.position-transform.position).normalized;
            
            //locking the Y axis to prevent any form of tilts and movement based errors

            directionToPlayer.y =0;

            if(directionToPlayer!= Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation= Quaternion.Slerp(transform.rotation,targetRotation
                ,5f*Time.deltaTime);
            }            
        }
        
    }



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
        isMoving = true; // did this to make sure that the player is currently moving

        // Code to go through each tile one by one
        foreach (Tile targetTile in path)
        {

            //Animation Start Pos
            Vector3 startPos=transform.position;

            // spawning 0.1f above so that the player doesnt spawn inside the tile
            Vector3 targetPosition = targetTile.transform.position+ new Vector3(0,0.1f,0);

            //Animation Var
            float jumpTime = 0.3f;
            float jumpHeight = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime<jumpTime)
            {
                elapsedTime += Time.deltaTime;
                float percentage = elapsedTime/jumpTime;

                //Rotation based on where the player the going
                Vector3 MovingDirect = (targetPosition - transform.position).normalized;

                // No SLANT BUG
                MovingDirect.y = 0f;
                
                if (MovingDirect!= Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(MovingDirect);
                    transform.rotation= Quaternion.Slerp(transform.rotation,targetRotation
                    ,10f*Time.deltaTime);
                }

                //Hop movment code using sine
                Vector3 latestPos= Vector3.Lerp(startPos, targetPosition,percentage);
                latestPos.y += Mathf.Sin(percentage*Mathf.PI)*jumpHeight;
                transform.position=latestPos;


                
                yield return null;
            }

            // Exact Position in integers
            transform.position = targetPosition;

            // New Tile Coords
            currentX = targetTile.gridX;
            currentZ = targetTile.gridZ;

        }
        isMoving = false;
    }
}
