using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData" , menuName = "ScriptableObjects/Obstacle")]
public class ObstacleData : ScriptableObject
{

    // Grid Dimensions
    public int gridHeight = 10;
    public int gridWidth =10;
    
    //Storing the coordinates and other data of all the tiles 
    public List<Vector2Int> Obstacle = new List<Vector2Int>();
    
}
