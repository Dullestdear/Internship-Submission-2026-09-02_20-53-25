using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData" , menuName = "ScriptableObjects/Obstacle")]
public class ObstacleData : ScriptableObject
{
    //Storing the coordinates and other data of all the tiles 
    public List<Vector2Int> Obstacle = new List<Vector2Int>();
    
}
