using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int playerScore = 0;
    [HideInInspector] public int playsRemaining = 100;

    GridManager gridManager;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void DecrementOnePlay()
    {
        playsRemaining--;
    }

    public void IncrementScore()
    {
        playerScore++;
    }

    public bool PlayerLost()
    {
        foreach (Tile tile in gridManager.tiles)
        {
            if (tile.transform.position.y >= gridManager.gridHeight - 1)
            {
                return true;
            }
        }
        return false;
    }

    public bool PlayerWon()
    {
        if (playsRemaining <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StopGame()
    {
    }
}
