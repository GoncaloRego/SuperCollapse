using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playsRemainingText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject playerWon;
    [SerializeField] GameObject playerLost;

    void Start()
    {
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        scoreText.text = gameManager.playerScore.ToString();
        playsRemainingText.text = gameManager.playsRemaining.ToString();
    }

    public void ShowPlayerWinMessage()
    {
        playerWon.SetActive(true);
    }

    public void ShowPlayerLoseMessage()
    {
        playerLost.SetActive(true);
    }
}
