using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.WSA;
public class PlayerData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText, coinText, scoreTextGameOver, coinTextGameOver,coinTextDouble;
    [SerializeField] Button doubleCoinButton;
    [SerializeField] Material shipMaterial;
    [SerializeField] GameEventsSO gameEventSO;
    [SerializeField] UIEventsSO uiEventSO;

    [Serializable]
    public class powerUpIconHolderUI
    {
        public Image icon;
        public Slider slider;
        public GameObject powerUpHolder;
        public PowerUps type;
        public bool isActive;
    }
    public List<powerUpIconHolderUI> powerUpHolders;

    public int playerLives;
    public float increasedMultiRate = 0, increasedFireRate = 0;
    public bool isIncinvibile = false;
    public GameObject invincibilty;

    bool canCountScore;
    private float score;
    private int coin;
    int gameCount;
    private static int totalCoin;
    float fireRateDuration, invincibilityDuration, multiShotDuration;
    private float gameStartTime;

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
        gameEventSO.OnGameStart += HandleGameStart;
        gameEventSO.OnCoinCollected += IncreaseCoin;
        gameEventSO.OnGameOver += HandleGameOver;

        doubleCoinButton.onClick.AddListener(() => 
        {
            uiEventSO.OnDoubleReward(coin);
        });
    }
    private void OnDestroy()
    {
        gameEventSO.OnGameStart -= HandleGameStart;
        gameEventSO.OnCoinCollected -= IncreaseCoin;
        gameEventSO.OnGameOver -= HandleGameOver;
    }
    void HandleGameStart()
    {
        Score = 0;
        Coin = 0;
        canCountScore = true;
        gameStartTime = Time.time;
        gameCount++;
    }
    void HandleGameOver(bool isQuit)
    {
        SoundManager.Instance.PlayBackGroundMusicMenu();
        DisableAllPowerUp();
        if (isQuit)
        {
            canCountScore = false;
            return;
        }
        SaveDataManager.Instance.HandleScoresAndCoin(coin, score, Time.time - gameStartTime);
        scoreTextGameOver.text = score.ToString("f2");
        coinTextGameOver.text = coin.ToString();
        coinTextDouble.text = coin.ToString();
        doubleCoinButton.interactable = true;
        if (gameCount % 4 == 0 && gameCount>2)
        {
            AdsManager.Instance.ShowInterstitialAds();
        }
    }

    void DisableAllPowerUp()
    {
        StopAllCoroutines();
        foreach (var holder in powerUpHolders)
        {
            holder.type = PowerUps.None;
            invincibilty.SetActive(false);
            holder.powerUpHolder.SetActive(false);
            isIncinvibile = false;
            increasedMultiRate = 0;
            increasedFireRate = 0;
        }
    }

    private void OnDisable()
    {
        shipMaterial.SetFloat("_OverlayBlend", 0);
    }

    private void Update()
    {
        if (canCountScore)
        {
            Score += Time.deltaTime;
        }
    }

    public void HandleFireRatePowerUps(float duration, Sprite icon, float incrementalFireRate)
    {
        increasedFireRate = incrementalFireRate;
        fireRateDuration = duration;
        var data = powerUpHolders.FirstOrDefault(x => x.type == PowerUps.increasedFireRate && x.isActive);
        if (data == null)
        {
            data = powerUpHolders.FirstOrDefault(x => !x.isActive);
            if (data == null)
            {
                return;
            }
            data.icon.sprite = icon;
            shipMaterial.SetFloat("_OverlayBlend", 1);
            StartCoroutine(IHandleFireRate(data, duration));
        }
    }
    IEnumerator IHandleFireRate(powerUpIconHolderUI holder, float totalDuration)
    {
        holder.type = PowerUps.increasedFireRate;
        holder.isActive = true;
        holder.powerUpHolder.SetActive(true);
        while (fireRateDuration > 0)
        {
            fireRateDuration -= 0.5f;
            holder.slider.value = fireRateDuration / totalDuration;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        holder.isActive = false;
        holder.type = PowerUps.None;
        holder.powerUpHolder.SetActive(false);
        increasedFireRate = 0;
        shipMaterial.SetFloat("_OverlayBlend", 0);
    }
    public void HandleInvincibiltyPowerUps(float duration, Sprite icon)
    {
        isIncinvibile = true;
        invincibilty.SetActive(true);
        invincibilityDuration = duration;
        var data = powerUpHolders.FirstOrDefault(x => x.type == PowerUps.invincibility && x.isActive);
        if (data == null)
        {
            data = powerUpHolders.FirstOrDefault(x => !x.isActive);
            if (data == null)
            {
                return;
            }
            data.icon.sprite = icon;
            StartCoroutine(IHandleInvincibilty(data, duration));
        }
    }
    IEnumerator IHandleInvincibilty(powerUpIconHolderUI holder, float totalDuration)
    {
        holder.type = PowerUps.invincibility;
        holder.isActive = true;
        holder.powerUpHolder.SetActive(true);
        while (invincibilityDuration > 0)
        {
            invincibilityDuration -= 0.5f;
            holder.slider.value = invincibilityDuration / totalDuration;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        holder.isActive = false;
        holder.type = PowerUps.None;
        invincibilty.SetActive(false);

        holder.powerUpHolder.SetActive(false);
        isIncinvibile = false;
    }
    public void HandleMultiShotPowerUps(float duration, Sprite icon, int incrementalMultishot)
    {
        increasedMultiRate = incrementalMultishot;
        multiShotDuration = duration;
        var data = powerUpHolders.FirstOrDefault(x => x.type == PowerUps.enhancedAttack && x.isActive);
        if (data == null)
        {
            data = powerUpHolders.FirstOrDefault(x => !x.isActive);
            if (data == null)
            {
                return;
            }
            data.icon.sprite = icon;
            StartCoroutine(IHandleMultiShot(data, duration));
        }
        else
        {
        }
    }
    IEnumerator IHandleMultiShot(powerUpIconHolderUI holder, float totalDuration)
    {
        holder.type = PowerUps.enhancedAttack;
        holder.isActive = true;
        holder.powerUpHolder.SetActive(true);
        while (multiShotDuration > 0)
        {
            multiShotDuration -= 0.5f;
            holder.slider.value = multiShotDuration / totalDuration;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        holder.isActive = false;
        holder.powerUpHolder.SetActive(false);
        holder.type = PowerUps.None;
        increasedMultiRate = 0;
    }

    public void DeductPlayerLives()
    {
        if (isIncinvibile) return;
        playerLives--;
        if (playerLives <= 0)
        {
            canCountScore = false;
            foreach (var powerUp in powerUpHolders)
            {
                powerUp.powerUpHolder.SetActive(false);
            }
            gameEventSO.OnGameOver?.Invoke(false);
            this.gameObject.SetActive(false);
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
        SoundManager.Instance.CoinCollect();
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

