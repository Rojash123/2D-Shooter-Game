using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventsSO", menuName = "Scriptable Objects/GameEventsSO")]
public class GameEventsSO : ScriptableObject
{
    public Action OnCoinCollected;
    public Action<Comets, typeOfComet, int> OnCometDestroyed, OnCometDestroyedAfterHit;
    public Action<Enemy, int> OnEnemyDestroyed, OnEnemyDestroyedAfterHit;
    public Action<SpaceMaterials, int> OnObstaclesDestroyed, OnObstaclesDestroyedAfterHit;
    public Action<PowerUp> OnPowerUpDestroyed;
    public Action<float, PowerUps, Sprite> OnPowerUpPicked;
    public Action<SaveData> OnDataLoadedAndUpdated;

    public Action OnGameStart;
    public Action OnGameOver;
}
