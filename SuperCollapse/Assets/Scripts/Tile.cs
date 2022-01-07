using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Blue,
    Red,
    Green
}

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject colorGameObject;
    Renderer colorRenderer;
    public TileType tileType;

    void Start()
    {
        SetTileName();
        SetTileColors();
    }

    void SetTileColors()
    {
        colorRenderer = colorGameObject.GetComponent<Renderer>();
        if (tileType == TileType.Blue)
        {
            colorRenderer.material.SetColor("_Color", Color.blue);
        }
        else if (tileType == TileType.Red)
        {
            colorRenderer.material.SetColor("_Color", Color.red);
        }
        else if (tileType == TileType.Green)
        {
            colorRenderer.material.SetColor("_Color", Color.green);
        }
    }

    void SetTileName()
    {
        gameObject.name = "(" + transform.position.x + "," + transform.position.y + ")";
    }
}
