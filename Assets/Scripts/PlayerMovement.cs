using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// This script is used for making the player move along the shortest path found using A*
// Also contains the moving animation of the player

public class PlayerMovement : MonoBehaviour
{
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
