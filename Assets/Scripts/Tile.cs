using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
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
    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false); 
    }


}
