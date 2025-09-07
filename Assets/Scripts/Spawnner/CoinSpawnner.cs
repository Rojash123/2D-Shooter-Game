using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnner : Singleton<CoinSpawnner>
{
    [SerializeField] GameObject coins;
    [SerializeField] Transform coinPoolParent, coinFinalPosition;

    private List<Coin> coinsObjectList=new List<Coin>();
    public int poolCount;

    private void Start()
    {
        for (int i = 0; i < poolCount; i++)
        {
            var coin = Instantiate(coins, coinPoolParent);
            coinsObjectList.Add(coin.GetComponent<Coin>());
            coin.SetActive(false);
        }
        PlayerMovement.Instance.OnCometDestroyedAfterHit += SendBacktoPool;
        PlayerMovement.Instance.OnEnemyDestroyedAfterHit += SpawnCoinEnemy;
        PlayerMovement.Instance.OnObstaclesDestroyedAfterHit += SpawnCoinObstacled;
    }
    private void OnDestroy()
    {
        if (PlayerMovement.Instance == null) return;
        PlayerMovement.Instance.OnCometDestroyedAfterHit -= SendBacktoPool;
        PlayerMovement.Instance.OnEnemyDestroyedAfterHit -= SpawnCoinEnemy;
        PlayerMovement.Instance.OnObstaclesDestroyedAfterHit -= SpawnCoinObstacled;
    }
    public void SendBacktoPool(Comets comet, typeOfComet type,int coinValue)
    {
        float delay = 0.1f;
        PlayerData.UpdateCoinValue(coinValue);
        coinValue=Mathf.Clamp(coinValue, 2, 6);
        for (int i = 0; i < coinValue; i++)
        {
            var data= coinsObjectList[0];
            data.transform.position = comet.transform.position+ new Vector3 (UnityEngine.Random.Range(-0.3f,0.1f), UnityEngine.Random.Range(-0.3f, 0.1f),0);
            data.gameObject.SetActive(true);
            data.AllowMove(delay);
            delay += 0.1f;
            coinsObjectList.RemoveAt(0);
        }
    }
    public void SpawnCoinEnemy(Enemy obj, int coinValue)
    {
        float delay = 0.1f;
        PlayerData.UpdateCoinValue(coinValue);
        coinValue = Mathf.Clamp(coinValue, 2, 4);

        for (int i = 0; i < coinValue; i++)
        {
            var data = coinsObjectList[0];
            data.transform.position = obj.transform.position + new Vector3(UnityEngine.Random.Range(-0.3f, 0.1f), UnityEngine.Random.Range(-0.3f, 0.1f), 0);
            data.gameObject.SetActive(true);
            data.AllowMove(delay);
            delay += 0.1f;
            coinsObjectList.RemoveAt(0);
        }
    }
    public void SpawnCoinObstacled(SpaceMaterials obj, int coinValue)
    {
        float delay = 0.1f;
        PlayerData.UpdateCoinValue(coinValue);
        coinValue = Mathf.Clamp(coinValue, 2, 4);
        for (int i = 0; i < coinValue; i++)
        {
            var data = coinsObjectList[0];
            data.transform.position = obj.transform.position + new Vector3(UnityEngine.Random.Range(-0.3f, 0.1f), UnityEngine.Random.Range(-0.3f, 0.1f), 0);
            data.gameObject.SetActive(true);
            data.AllowMove(delay);
            delay += 0.1f;
            coinsObjectList.RemoveAt(0);
        }
    }
    public void SendBacktoPool(Coin obj)
    {
        obj.gameObject.SetActive(false);
        coinsObjectList.Add(obj);
    }
}
