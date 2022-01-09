using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Blue,
    Red,
    Green,
    Grey
}

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject colorGameObject;
    SpriteRenderer colorRenderer;
    public TileType tileType;
    public int totalTileTypes = Enum.GetNames(typeof(TileType)).Length;
    GridManager gridManager;

    void Start()
    {
        SetTileName();
        SetTileColor();
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        SetTileColor();
    }

    void SetTileColor()
    {
        colorRenderer = colorGameObject.GetComponent<SpriteRenderer>();
        if (tileType == TileType.Blue)
        {
            colorRenderer.color = Color.blue;
        }
        else if (tileType == TileType.Red)
        {
            colorRenderer.color = Color.red;
        }
        else if (tileType == TileType.Green)
        {
            colorRenderer.color = Color.green;
        }
        else if (tileType == TileType.Grey)
        {
            colorRenderer.color = Color.gray;
        }
    }

    void SetTileName()
    {
        //gameObject.name = "(" + transform.position.x + "," + transform.position.y + ")";
    }

    public void MoveTileUp()
    {
        Debug.Log(transform.position);
    }

    void OnMouseDown()
    {
        // if (gridManager.grid.ContainsValue(this))
        // {
        Destroy(gameObject);
        // }
    }

    public void InstantiateTile(Tile tile, Vector2 position)
    {
        Instantiate(tile.gameObject, position, Quaternion.identity);
    }
}
