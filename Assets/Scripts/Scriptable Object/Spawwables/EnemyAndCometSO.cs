using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnemyAndCometSO;

[CreateAssetMenu(fileName = "EnemyAndCometSO", menuName = "Scriptable Objects/EnemyAndCometSO")]
public class EnemyAndCometSO : ScriptableObject
{
    [SerializeField] Comets cometPrefab;
    [Serializable]
    public class EnemyData
    {
        public string enemyName;
        public Enemy enemy;
        public int coinvalue;
        public float maxHealth;
        public float fireRate;
    }
    public List<EnemyData> enemies;

    [Serializable]
    public class CometData
    {
        public string cometName;
        public float maxHealth;
        public int size;
        public int coinvalue;
        public typeOfComet cometType;
    }
    public List<CometData> comets;

    [Serializable]
    public class SpaceMaterialData
    {
        public string materilaName;
        public SpaceMaterials obstacles;
        public int coinvalue;
        public float maxHealth;
    }
    public List<SpaceMaterialData> spaceMaterialData;

    public Comets GetComet(typeOfComet cometType)
    {
        CometData cometData= comets.FirstOrDefault(s=>s.cometType==cometType);
        cometPrefab.SetCometData(cometData.cometName,cometData.maxHealth, cometData.cometType,cometData.coinvalue,cometData.size);
        return cometPrefab;
    }
    public List<Enemy> GetEnemyList()
    {
        List<Enemy> list = new List<Enemy>();
        foreach (var enemyData in enemies)
        {
            enemyData.enemy.SetEnemyData(enemyData.maxHealth, enemyData.fireRate, enemyData.coinvalue);
            list.Add(enemyData.enemy);
        }
        return list;
    }
    public List<SpaceMaterials> GetSpaceMaterialList()
    {
        List<SpaceMaterials> list = new List<SpaceMaterials>();
        foreach (var spaceMaterial in spaceMaterialData)
        {
            spaceMaterial.obstacles.SetSpaceMaterialData(spaceMaterial.maxHealth, spaceMaterial.coinvalue);
            list.Add(spaceMaterial.obstacles);
        }
        return list;
    }

}
