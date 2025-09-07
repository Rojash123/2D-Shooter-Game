using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class PlayerData : MonoBehaviour
{
    public int playerLives;

    public Action OnPlayerKilled;

    [SerializeField] TextMeshProUGUI scoreText, coinText;
    bool canCountScore;

    private float score;
    private int coin;
    private static int totalCoin;

    float fireRateDuration, invincibilityDuration, multiShotDuration;

    public float increasedMultiRate=0,increasedFireRate=0;
    public bool isIncinvibile=false;

    [SerializeField] Material shipMaterial;

    public GameObject invincibilty;

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

    public void HandleFireRatePowerUps(float duration, Sprite icon ,float incrementalFireRate)
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
    IEnumerator IHandleFireRate(powerUpIconHolderUI holder,float totalDuration)
    {
        holder.type = PowerUps.increasedFireRate;
        holder.isActive = true;
        holder.powerUpHolder.SetActive(true);
        while (fireRateDuration > 0)
        {
            fireRateDuration -= 0.5f;
            holder.slider.value = fireRateDuration/totalDuration;
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

    public void HandleMultiShotPowerUps(float duration, Sprite icon,int incrementalMultishot)
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

    void PlayerMovementStatus(bool status)
    {
        Score = 0;
        canCountScore = status;
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
            OnPlayerKilled?.Invoke();
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

