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
    }

    public void SendBacktoPool(Comets comet, typeOfComet type)
    {
        int coinValue = 0;
        switch (type)
        {
            case typeOfComet.small:
                coinValue = 2;
                break;

            case typeOfComet.medium:
                coinValue = 5;
                break;

            case typeOfComet.large:
                coinValue = 15;
                break;

            default:
                break;
        }
        float delay = 0.1f;
        PlayerData.UpdateCoinValue(coinValue);
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

    public void SendBacktoPool(Coin obj)
    {
        obj.gameObject.SetActive(false);
        coinsObjectList.Add(obj);
    }
}
