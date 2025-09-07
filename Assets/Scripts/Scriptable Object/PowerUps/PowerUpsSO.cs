using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpsSO", menuName = "Scriptable Objects/PowerUpsSO")]

public class PowerUpsSO : ScriptableObject
{
    public PowerUp powerUpPrefab;
    [Serializable]
    public class PowerUpsClass
    {
        public PowerUps powerUpType;
        public float duration;
        public bool isUnlocked;
        public Sprite powerupIcon;
        public bool isActive;
    }
    public  List<PowerUpsClass> powerUpstypes;

    public PowerUpsClass GetRandomPowerUp()
    {
        return powerUpstypes[UnityEngine.Random.Range(0, powerUpstypes.Count)];
    }
}

public enum PowerUps
{
    enhancedAttack,
    invincibility,
    increasedFireRate,
    None
}
