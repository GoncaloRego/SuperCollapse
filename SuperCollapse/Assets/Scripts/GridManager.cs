using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] List<Tile> tiles;
    [SerializeField] int numberOfInitialTiles = 10;

    void Start()
    {
        tiles = new List<Tile>();
        InstantiateInitialTiles();
    }

    void Update()
    {

    }

    void InstantiateInitialTiles()
    {
        for (int i = 0; i < numberOfInitialTiles; i++)
        {
            Debug.Log("Entrei");
            Tile newTile = new Tile();
            if (i % 2 == 0)
            {
                newTile.tileType = TileType.Blue;
            }
            else
            {
                newTile.tileType = TileType.Green;
            }

            tiles.Add(newTile);

            Debug.Log(tiles.Count);
        }
    }
}
