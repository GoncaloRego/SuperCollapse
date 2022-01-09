using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] List<Tile> tiles;
    [SerializeField] int numberOfInitialTiles;

    [SerializeField] GameObject nextLineTilesBackground;
    float nextLineBackground_y;
    float nextLineTilesTimeDelay = 0.5f;

    [SerializeField] GameObject gridBackground;
    [SerializeField] public Dictionary<Vector2Int, Tile> grid = new Dictionary<Vector2Int, Tile>();
    [SerializeField] int gridWidth = 7;
    [SerializeField] int gridHeight = 9;

    void Start()
    {
        tiles = new List<Tile>();

        nextLineBackground_y = nextLineTilesBackground.transform.position.y;
        gridBackground.transform.localScale = new Vector2(gridWidth, gridHeight);

        InstantiateInitialTiles();
        InstantiateNextLineBlankTiles();
        StartCoroutine(nameof(InstantiateNextLineTiles));
    }

    void Update()
    {

    }

    void InstantiateInitialTiles()
    {
        for (int i = 0; i < numberOfInitialTiles; i++)
        {
            float tileColor = Random.Range(0, tile.totalTileTypes - 1);

            Vector2Int tilePosition = new Vector2Int(i, 0);

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

            tiles.Add(tile);
            grid.Add(tilePosition, tile);
            tile.InstantiateTile(tile, new Vector2(tilePosition.x, tilePosition.y));
        }
    }

    void InstantiateNextLineBlankTiles()
    {
        for (int i = 0; i < numberOfInitialTiles; i++)
        {
            Vector2 tilePosition = new Vector2(i, nextLineBackground_y);
            tile.tileType = TileType.Grey;

            tile.InstantiateTile(tile, new Vector2(tilePosition.x, tilePosition.y));
        }
    }

    IEnumerator InstantiateNextLineTiles()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < numberOfInitialTiles; i++)
        {
            float tileColor = Random.Range(0, tile.totalTileTypes - 1);

            Vector2 tilePosition = new Vector2(i, nextLineBackground_y);

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

            tiles.Add(tile);
            tile.InstantiateTile(tile, new Vector2(tilePosition.x, tilePosition.y));

            yield return new WaitForSeconds(nextLineTilesTimeDelay);
        }

        MoveNextLineTilesToGrid();
    }

    void MoveNextLineTilesToGrid()
    {
        //tiles.Reverse();
        List<Tile> tempTilesToMove = new List<Tile>();

        for (int x = 0; x < gridWidth; x++)
        {
            tempTilesToMove.Add(tiles[x]);
        }

        foreach (Tile tile in tiles)
        {
            tile.MoveTileUp();
        }

        //tiles.Reverse();
    }
}
