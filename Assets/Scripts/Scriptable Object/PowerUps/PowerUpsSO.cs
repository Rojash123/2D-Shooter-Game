using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpsSO", menuName = "Scriptable Objects/PowerUpsSO")]

public class PowerUpsSO : ScriptableObject
{
    public PowerUp powerUpPrefab;
    
    public  List<PowerUpsClass> powerUpstypes;

    public PowerUpsClass GetRandomPowerUp()
    {
        return powerUpstypes[UnityEngine.Random.Range(0, powerUpstypes.Count)];
    }

    public void SetUpdatedData(SaveData data)
    {
        foreach (var item in data.powerupdata) 
        {
            var powerUp = powerUpstypes.FirstOrDefault(x => x.powerUpType == item.powerUpType);
            powerUp.UpdateValues(item);
        }
    }
}
[Serializable]
public class PowerUpsClass
{
    public PowerUps powerUpType;
    public float duration;
    public bool isUnlocked;
    public Sprite powerupIcon;
    public int level;

    public void UpdateValues(PowerupData data)
    {
        this.duration = data.duration;
        this.level = data.level;
        this.isUnlocked = true;
    }
}

public enum PowerUps
{
    enhancedAttack,
    invincibility,
    increasedFireRate,
    None
}
