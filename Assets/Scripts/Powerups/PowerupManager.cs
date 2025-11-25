using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField]
    private float powerUpSpawnInterval = 15;
    private float lastPowerUpSpawnTime = 0;
    private List<PowerUp> powerUpPool = new List<PowerUp>() { };
    public PowerUpsSO powerUpSo;
    public Transform powerUpParent;

    [SerializeField] GameEventsSO gameEventsSO;
    private void Start()
    {
        gameEventsSO.OnCometDestroyedAfterHit += SendBacktoPool;
        gameEventsSO.OnEnemyDestroyedAfterHit += SpawnCoinEnemy;
        gameEventsSO.OnObstaclesDestroyedAfterHit += SpawnCoinObstacle;
        gameEventsSO.OnPowerUpDestroyed += OnpowerUpDestroyed;
        gameEventsSO.OnDataLoadedAndUpdated +=OnSaveDataLoadUpdateScriptable;
        InitializePool();
    }
    void InitializePool()
    {
        for (int i = 0; i < 10; i++)
        {
            var obj = Instantiate(powerUpSo.powerUpPrefab, powerUpParent);
            obj.gameObject.SetActive(false);
            powerUpPool.Add(obj);
        }
    }

    private void OnDestroy()
    {
        if (PlayerMovement.Instance == null) return;
        gameEventsSO.OnCometDestroyedAfterHit -= SendBacktoPool;
        gameEventsSO.OnEnemyDestroyedAfterHit -= SpawnCoinEnemy;
        gameEventsSO.OnObstaclesDestroyedAfterHit -= SpawnCoinObstacle;
        gameEventsSO.OnPowerUpDestroyed -= OnpowerUpDestroyed;
        gameEventsSO.OnDataLoadedAndUpdated -= OnSaveDataLoadUpdateScriptable;
    }
    void OnpowerUpDestroyed(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(false);
        powerUp.transform.SetParent(powerUpParent);
        powerUpPool.Add(powerUp);
    }

    public void SendBacktoPool(Comets comet, typeOfComet type, int coinValue)
    {
        CheckPowerUpSpawnnable(comet.transform.position);
    }
    public void SpawnCoinEnemy(Enemy obj, int coinValue)
    {
        CheckPowerUpSpawnnable(obj.transform.position);
    }
    public void SpawnCoinObstacle(SpaceMaterials obj, int coinValue)
    {
        CheckPowerUpSpawnnable(obj.transform.position);
    }

    void OnSaveDataLoadUpdateScriptable(SaveData data)
    {

    }

    public void CheckPowerUpSpawnnable(Vector3 spawnPosition)
    {
        int data = Random.Range(0, 7);
        if (data > 1) return;

        if (Time.time - lastPowerUpSpawnTime > powerUpSpawnInterval)
        {
            lastPowerUpSpawnTime = Time.time;
            var pickedPowerup = powerUpPool[0];
            powerUpPool.RemoveAt(0);
            pickedPowerup.SetData(powerUpSo.GetRandomPowerUp(), 2f);
            pickedPowerup.transform.position = spawnPosition;
            pickedPowerup.gameObject.SetActive(true);
        }
    }

}
