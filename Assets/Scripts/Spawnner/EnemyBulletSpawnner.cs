using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawnner : Singleton<EnemyBulletSpawnner>
{
    public GameObject regularBullet, mediumBullet, largeBullet;

    private List<EnemyBullet> regularBulletPool = new();
    private List<EnemyBullet> MediumBulletPool=new();
    private List<EnemyBullet> largeBulletPool = new();

    [SerializeField] Transform regularBulletParent, mediumBulletParent, largeBulletParent;

    public Action<EnemyBullet,string> OnBulletDestroyed;

    private void Start()
    {
        OnBulletDestroyed += BulletDestroyed;
        InitializeBulletPool();
    }
    private void OnDestroy()
    {
        OnBulletDestroyed -= BulletDestroyed;
    }
    void InitializeBulletPool()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject bullet = GameObject.Instantiate(regularBullet, regularBulletParent);
            bullet.SetActive(false);
            bullet.transform.localScale = Vector3.one;
            bullet.GetComponent<EnemyBullet>().SetType("Small");
            regularBulletPool.Add(bullet.GetComponent<EnemyBullet>());

            bullet = GameObject.Instantiate(mediumBullet, mediumBulletParent);
            bullet.SetActive(false);
            bullet.transform.localScale = Vector3.one;
            bullet.GetComponent<EnemyBullet>().SetType("Medium");

            MediumBulletPool.Add(bullet.GetComponent<EnemyBullet>());

            bullet = GameObject.Instantiate(largeBullet, largeBulletParent);
            bullet.SetActive(false);
            bullet.transform.localScale = Vector3.one;
            bullet.GetComponent<EnemyBullet>().SetType("Large");
            largeBulletPool.Add(bullet.GetComponent<EnemyBullet>());
        }
    }
    void BulletDestroyed(EnemyBullet bullet,string Type)
    {
        bullet.gameObject.SetActive(false);
        switch (Type)
        {
            case "Small":
                bullet.transform.position = regularBulletParent.transform.position;
                regularBulletPool.Add(bullet.GetComponent<EnemyBullet>());
                break;

            case "Medium":
                bullet.transform.position = mediumBulletParent.transform.position;
                MediumBulletPool.Add(bullet.GetComponent<EnemyBullet>());
                break;

            case "Large":
                bullet.transform.position = largeBulletParent.transform.position;
                largeBulletPool.Add(bullet.GetComponent<EnemyBullet>());
                break;

            default:
                break;
        }
    }

    public EnemyBullet GetBullet(string type)
    {
        var obj = regularBulletPool[0];
        regularBulletPool.RemoveAt(0);
        return obj;
    }
}
