using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGui : MonoBehaviour
{
    public static GameGui Instance;
    
    private int coins = 0;
    private int score = 0;
    private int lives = 3;

    public Text coinText;
    public Text scoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        coins = 0;
        SyncUi();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SyncUi();
    }

    public void AddGoldenCoins(int amount)
    {
        coins += amount;
        SyncUi();
    }

    public void AddScore(int amount)
    {
        score += amount;
        SyncUi();
    }

    public void AddLives(int amount)
    {
        lives += amount;
        Debug.Log("Lives: " + lives);
    }

    void SyncUi()
    {
        coinText.text = coins.ToString("00");
        scoreText.text = score.ToString("000000");
    }
}