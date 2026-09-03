using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    // line used to stop player inputs while the character is walking
    public bool isMoving {get;private set;}

    // player character's current postition/coords
    public int currentX;
    public int currentZ;

    // Func to move the player along the calculated path found using pathfinding
    public void MoveAlongPath(List<Tile> path)
    {
        if (!isMoving && path != null && path.Count > 0)
        {
            StartCoroutine(Move(path));
        }
    }

    private IEnumerator Move(List<Tile> path)
    {
        isMoving = true; // did this to make sure that the player is currently moving

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

            // New Tile Coords
            currentX = targetTile.gridX;
            currentZ = targetTile.gridZ;

        }
        isMoving = false;
    }


}
