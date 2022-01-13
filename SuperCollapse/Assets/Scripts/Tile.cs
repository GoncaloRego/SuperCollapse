using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Blue,
    Red,
    Green,
}

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject colorGameObject;
    [SerializeField] public TileType tileType;
    [HideInInspector] public int totalTileTypes = Enum.GetNames(typeof(TileType)).Length;
    [SerializeField] public bool isInsideGrid = false;
    Rigidbody2D tileRigidbody;

    SpriteRenderer colorRenderer;
    GridManager gridManager;
    GameManager gameManager;
    UIController uIController;

    float gravityScale = 2f;

    bool collided = false;

    void Start()
    {
        tileRigidbody = GetComponent<Rigidbody2D>();
        gridManager = FindObjectOfType<GridManager>();
        gameManager = FindObjectOfType<GameManager>();
        uIController = FindObjectOfType<UIController>();

        if (TileIsInsideGrid() == true)
        {
            EnableGravity();
        }
    }

    public void SetTileColor()
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
    }

    public void MoveTileToGrid()
    {
        transform.position = new Vector2(transform.position.x, 0f);
        isInsideGrid = true;
        EnableGravity();
    }

    public void MoveTileOneLineUp()
    {
        int newPosition_Y = Mathf.RoundToInt(transform.position.y) + 1;
        transform.position = new Vector2(transform.position.x, newPosition_Y);
    }

    public void EnableGravity()
    {
        tileRigidbody.gravityScale = gravityScale;
    }

    bool TileIsInsideGrid()
    {
        if (Mathf.RoundToInt(transform.position.y) >= 0f && Mathf.RoundToInt(transform.position.y) < gridManager.gridHeight)
        {
            return true;
        }
        return false;
    }

    void OnMouseDown()
    {
        gridManager.UpdateTilesArray();
        gridManager.RemoveTiles(this);
    }
}
