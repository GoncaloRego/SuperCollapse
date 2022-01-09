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
    [SerializeField] public TileType tileType;
    [HideInInspector] public int totalTileTypes = Enum.GetNames(typeof(TileType)).Length;
    [SerializeField] public bool isInsideGrid = false;

    SpriteRenderer colorRenderer;
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
        SetTileName();
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
        gameObject.name = "(" + transform.position.x + "," + transform.position.y + ")";
    }

    public void MoveTileToGrid()
    {
        transform.position = new Vector3(transform.position.x, 0, 0f);
        isInsideGrid = true;
    }

    public void MoveTileOneLineUp()
    {
        float newPosition_Y = transform.position.y + 1f;
        transform.position = new Vector3(transform.position.x, newPosition_Y, 0f);
    }

    void OnMouseDown()
    {
        if (isInsideGrid == true)
        {
            Destroy(gameObject);
        }
    }
}
