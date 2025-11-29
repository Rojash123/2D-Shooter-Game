using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public string userName;
    public float highScore;
    public int coin;
    public List<PowerupData> powerupdata;
    public bool isPowerUpUnlocked;
    public int bulletlevel;
    public float timeSpent;

    public SaveData(float highScore,int coin, List<PowerupData>powerupdata,bool isUnlocked,int bullet,float timeSpent,string userName)
    {
        this.highScore = highScore;
        this.coin = coin;
        this.powerupdata = powerupdata;
        isPowerUpUnlocked = isUnlocked;
        bulletlevel = bullet;
        this.timeSpent = timeSpent;
        this.userName = userName;
    }
}

[Serializable]
public class PowerupData
{
    public PowerUps powerUpType;
    public int level;
    public float duration;

    public PowerupData(PowerUps type, float duration, int level)
    {
        powerUpType = type;
        this.level = level;
        this.duration = duration;
    }
}
