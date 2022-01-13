using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] GameObject nextLineTilesBackground;
    [SerializeField] public Tile[] tiles;
    [SerializeField] public List<Tile> tilesToRemove;
    float nextLineBackground_y;
    float nextLineTilesTimeDelay = 1f;
    int nextLineNumberOfTiles = 7;
    int newLines = 0;
    bool lineAdded;
    bool tilesWereRemoved = false;
    int minAdjacentTiles = 3;
    int totalNumberOfTilesActive;

    [SerializeField] GameObject gridBackground;
    public int gridWidth = 7;
    public int gridHeight = 9;
    int emptyColumn;

    [SerializeField] [Range(10, 30)] int numberOfInitialTiles = 10;

    GameManager gameManager;
    UIController uiController;

    Dictionary<Vector2, Tile> grid;

    AudioSource audioSource;

    void Start()
    {
        tilesToRemove = new List<Tile>();
        nextLineBackground_y = nextLineTilesBackground.transform.position.y;
        gridBackground.transform.localScale = new Vector2(gridWidth, gridHeight);
        grid = new Dictionary<Vector2, Tile>();
        gameManager = FindObjectOfType<GameManager>();
        uiController = FindObjectOfType<UIController>();

        audioSource = GetComponent<AudioSource>();
        totalNumberOfTilesActive = numberOfInitialTiles;

        InstantiateInitialTiles();
        StartCoroutine(nameof(InstantiateNextLineTiles));
    }

    void Update()
    {
        if (lineAdded == true)
        {
            if (gameManager.PlayerLost() || gameManager.PlayerWon())
            {
                if (gameManager.PlayerLost())
                {
                    uiController.ShowPlayerLoseMessage();
                }
                else if (gameManager.PlayerWon())
                {
                    uiController.ShowPlayerWinMessage();
                }
            }
            else
            {
                StartCoroutine(nameof(InstantiateNextLineTiles));
            }
        }

        if (tilesWereRemoved == true)
        {
            UpdateTilesArray();
        }
    }

    void InstantiateInitialTiles()
    {
        int currentLinePos_X = 0;
        int currentLinePos_Y = 0;

        for (int x = 0; x < numberOfInitialTiles; x++)
        {
            float tileColor = Random.Range(0, tile.totalTileTypes);
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
            tile.SetTileColor();
            grid.Add(tilePosition, tile);
            Instantiate(tile.gameObject, tilePosition, Quaternion.identity);

            if (currentLinePos_X >= gridWidth)
            {
                currentLinePos_X = 0;
                currentLinePos_Y++;
            }
        }

        UpdateTilesArray();
    }

    IEnumerator InstantiateNextLineTiles()
    {
        lineAdded = false;
        yield return new WaitForSeconds(2f);

        for (int x = 0; x < nextLineNumberOfTiles; x++)
        {
            float tileColor = Random.Range(0, tile.totalTileTypes);

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
            tile.SetTileColor();

            Instantiate(tile.gameObject, new Vector2(tilePosition.x, tilePosition.y), Quaternion.identity);

            yield return new WaitForSeconds(nextLineTilesTimeDelay);
        }

        UpdateTilesArray();

        AddNewPlay();
        yield return new WaitForSeconds(nextLineTilesTimeDelay);
    }

    void AddNewPlay()
    {
        foreach (Tile t in tiles)
        {
            if (t.isInsideGrid == true)
            {
                t.MoveTileOneLineUp();
            }
            else if (t.isInsideGrid == false)
            {
                t.MoveTileToGrid();
            }
        }

        newLines++;
        lineAdded = true;
        totalNumberOfTilesActive += gridWidth;
        gameManager.DecrementOnePlay();
        uiController.UpdateTexts();

        if (gameManager.PlayerLost() || gameManager.PlayerWon())
        {
            if (gameManager.PlayerLost())
            {
                uiController.ShowPlayerLoseMessage();
            }
            else if (gameManager.PlayerWon())
            {
                uiController.ShowPlayerWinMessage();
            }
        }
    }

    public void RemoveTiles(Tile tileClicked)
    {
        AddAdjacentTiles(tileClicked);

        if (tileClicked.isInsideGrid == true && TileHasEnoughAdjacentTiles(tileClicked) == true)
        {
            gameManager.IncrementScore();
            uiController.UpdateTexts();

            PlayGridSound();

            if (gameManager.PlayerLost() || gameManager.PlayerWon())
            {
                if (gameManager.PlayerLost())
                {
                    uiController.ShowPlayerLoseMessage();
                }
                else if (gameManager.PlayerWon())
                {
                    uiController.ShowPlayerWinMessage();
                }
            }

            foreach (Tile t in tilesToRemove)
            {
                if (t != null)
                {
                    Destroy(t.gameObject);
                }
            }
        }
    }

    public bool TileHasEnoughAdjacentTiles(Tile tileClicked)
    {
        if (tilesToRemove.Count > minAdjacentTiles)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AddAdjacentTiles(Tile tileClicked)
    {
        Tile nextTile = tileClicked;
        tilesToRemove.Clear();

        foreach (Tile t in tiles)
        {
            if (TileIsAdjacent(tileClicked, t) == true || TileIsAdjacent(nextTile, t) == true)
            {
                if (tilesToRemove.Contains(tileClicked) == false)
                {
                    tilesToRemove.Add(tileClicked);
                }
                tilesToRemove.Add(t);
                nextTile = t;
            }
        }
    }

    bool TileIsFromSameLine(Tile tileClicked, Tile tileAdjacent)
    {
        if (tileAdjacent.transform.position.x == tileClicked.transform.position.x - 1 && Mathf.RoundToInt(tileAdjacent.transform.position.y) == Mathf.RoundToInt(tileClicked.transform.position.y)) // Adjacent is on the right
        {
            return true;
        }
        else if (tileAdjacent.transform.position.x == tileClicked.transform.position.x + 1 && Mathf.RoundToInt(tileAdjacent.transform.position.y) == Mathf.RoundToInt(tileClicked.transform.position.y)) // Adjacent is on the left
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool TileIsFromSameColumn(Tile tileClicked, Tile tileAdjacent)
    {
        if (tileAdjacent.transform.position.x == tileClicked.transform.position.x && Mathf.RoundToInt(tileAdjacent.transform.position.y) == Mathf.RoundToInt(tileClicked.transform.position.y + 1)) // Adjacent is on top
        {
            return true;
        }
        else if (tileAdjacent.transform.position.x == tileClicked.transform.position.x && Mathf.RoundToInt(tileAdjacent.transform.position.y) == Mathf.RoundToInt(tileClicked.transform.position.y - 1)) // Adjacent is on the bottom
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool TileIsAdjacent(Tile tileClicked, Tile tileAdjacent)
    {
        if ((TileIsFromSameLine(tileClicked, tileAdjacent) || TileIsFromSameColumn(tileClicked, tileAdjacent)) && tileAdjacent.isInsideGrid == true && tileAdjacent.tileType == tileClicked.tileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateTilesArray()
    {
        tiles = FindObjectsOfType<Tile>();
    }

    void PlayGridSound()
    {
        audioSource.Play();
    }
}
