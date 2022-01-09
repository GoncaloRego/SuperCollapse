using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] GameObject nextLineTilesBackground;
    [SerializeField] public Tile[] tiles;
    float nextLineBackground_y;
    float nextLineTilesTimeDelay = 1f;
    int nextLineNumberOfTiles = 7;
    int newLines = 0;
    bool lineAdded;

    [SerializeField] GameObject gridBackground;
    [SerializeField] int gridWidth = 7;
    [SerializeField] int gridHeight = 9;

    [SerializeField] [Range(10, 30)] int numberOfInitialTiles = 10;

    GameManager gameManager;

    void Start()
    {
        nextLineBackground_y = nextLineTilesBackground.transform.position.y;
        gridBackground.transform.localScale = new Vector2(gridWidth, gridHeight);

        gameManager = FindObjectOfType<GameManager>();

        InstantiateInitialTiles();
        InstantiateNextLineBlankTiles();
        StartCoroutine(nameof(InstantiateNextLineTiles));
    }

    void Update()
    {
        if (lineAdded == true)
        {
            if (tile == null)
            {
                return;
            }
            StartCoroutine(nameof(InstantiateNextLineTiles));
        }
    }

    void InstantiateInitialTiles()
    {
        int currentLinePos_X = 0;
        int currentLinePos_Y = 0;

        for (int x = 0; x < numberOfInitialTiles; x++)
        {
            float tileColor = Random.Range(0, tile.totalTileTypes - 1);
            Vector2 tilePosition = new Vector2(currentLinePos_X, currentLinePos_Y);
            currentLinePos_X++;

            if (tileColor == 0) // Blue
            {
                tile.tileType = TileType.Blue;
            }
            else if (tileColor == 1) // Red
            {
                tile.tileType = TileType.Red;
            }
            else if (tileColor == 2) // Green
            {
                tile.tileType = TileType.Green;
            }

            tile.isInsideGrid = true;
            Instantiate(tile.gameObject, tilePosition, Quaternion.identity);

            if (currentLinePos_X >= gridWidth)
            {
                currentLinePos_X = 0;
                currentLinePos_Y++;
            }
        }

        tiles = FindObjectsOfType<Tile>();
    }

    void InstantiateNextLineBlankTiles()
    {
        for (int i = 0; i < nextLineNumberOfTiles; i++)
        {
            Vector2 tilePosition = new Vector2(i, nextLineBackground_y);
            tile.tileType = TileType.Grey;

            tile.isInsideGrid = false;
            //Instantiate(tile.gameObject, new Vector2(tilePosition.x, tilePosition.y), Quaternion.identity);
        }
    }

    IEnumerator InstantiateNextLineTiles()
    {
        lineAdded = false;
        yield return new WaitForSeconds(3f);

        for (int x = 0; x < nextLineNumberOfTiles; x++)
        {
            float tileColor = Random.Range(0, tile.totalTileTypes - 1);

            Vector2 tilePosition = new Vector2(x, nextLineBackground_y);

            if (tileColor == 0) // Blue
            {
                tile.tileType = TileType.Blue;
            }
            else if (tileColor == 1) // Red
            {
                tile.tileType = TileType.Red;
            }
            else if (tileColor == 2) // Green
            {
                tile.tileType = TileType.Green;
            }

            tile.isInsideGrid = false;
            Instantiate(tile.gameObject, new Vector2(tilePosition.x, tilePosition.y), Quaternion.identity);

            tiles = FindObjectsOfType<Tile>();
            yield return new WaitForSeconds(nextLineTilesTimeDelay);
        }

        AddNewPlay();
    }

    void AddNewPlay()
    {
        foreach (Tile t in tiles)
        {
            if (t.tileType != TileType.Grey && t.isInsideGrid == true)
            {
                t.MoveTileOneLineUp();
            }
            else if (t.tileType != TileType.Grey && t.isInsideGrid == false)
            {
                t.MoveTileToGrid();
            }
        }

        newLines++;
        lineAdded = true;
    }
}
