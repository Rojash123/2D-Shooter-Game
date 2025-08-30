using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PlayerData : MonoBehaviour
{
    public int playerLives;

    public Action OnPlayerKilled;

    [SerializeField] TextMeshProUGUI scoreText,coinText;
    bool canCountScore;

    private float score;

    private int coin;
    private static int totalCoin;

    Coroutine coinIncrement;
    float Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = value.ToString("f2");
        }
    }

    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            coinText.text = value.ToString();
        }
    }

    private void Awake()
    {
        OnPlayerKilled += PlayerMovement.Instance.PlayerKilled;
        PlayerMovement.Instance.OnPlayerMove += PlayerMovementStatus;
        PlayerMovement.Instance.OnCoinCollected += IncreaseCoin;

    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.OnPlayerMove -= PlayerMovementStatus;
        PlayerMovement.Instance.OnCoinCollected -= IncreaseCoin;
        OnPlayerKilled -= PlayerMovement.Instance.PlayerKilled;
    }

    private void Update()
    {
        if (canCountScore)
        {
            Score += Time.deltaTime;
        }
    }

    void PlayerMovementStatus(bool status)
    {
        Score = 0;
        canCountScore = status;
    }

    public void DeductPlayerLives()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            canCountScore = false;
            OnPlayerKilled();
        }
    }
    public static void UpdateCoinValue(int amount)
    {
        totalCoin += amount;
    }
    public void IncreaseCoin()
    {
        if (coinIncrement != null)
        {
            StopCoroutine(coinIncrement);
        }
        coinIncrement = StartCoroutine(I_IncreaseCoinAmount());
    }

    IEnumerator I_IncreaseCoinAmount()
    {
        while (Coin < totalCoin)
        {
            Coin += 1;
            yield return new WaitForSeconds(0.2f);
        }
    } 
}
