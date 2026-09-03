using Unity.VisualScripting;
using UnityEngine;


// For A* Pathfinding ( used here)
// G = Cost From Starting Node 
// H = Heuristic Cost to Reach End Node
// F = G + H ( F is Final State)





public class Tile : MonoBehaviour
{
    // Variables for pathfinding

    // true by default but if a obstacle is here ( on the tile) then it is false
    public bool isWalkable = true; 
    public int gcost;
    public int hcost;
    public int fcost
    {
        get{return gcost + hcost;}
    }
    // used for remembering previous tiles
    public Tile previousTile;



    // variables for colours of the grid and the highlight
    [SerializeField] private Color basecolor;
    [SerializeField] private Color offsetcolor;

    [SerializeField] private GameObject highlight;

    [SerializeField] private Renderer meshrenderer;

    // Changing the colour of materials for the grid prefab
    public void Init(bool offset)
    {
        meshrenderer.material.color = offset ? offsetcolor : basecolor; 
    }

    //functions to detect mouse input to highlight a grid tile
    public void ToggleHighlight(bool toggle)
    {
        highlight.SetActive(toggle);
    }


}
