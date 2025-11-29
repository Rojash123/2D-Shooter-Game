using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
public class CometAndEnemySpawnner : MonoBehaviour
{
    private List<Comets> smallCometsPool = new List<Comets>();
    private List<Comets> largeCometsPool = new List<Comets>();
    private List<Comets> mediumCometsPool = new List<Comets>();

    [SerializeField] Transform cometPoolParent;
    [SerializeField] float cometSpawnRate, cometSpawnTime;

    List<Vector3> spawnPoints = new List<Vector3>();
    bool canSpawnComets, canSpawnMaterials, canSpawnEnemies;
    Camera cam;
    float minY;
    float cometSpeed = 2f;

    private List<Enemy> EnemyPool = new List<Enemy>();
    [SerializeField] Transform enemyPoolParent;

    private List<SpaceMaterials> obstaclePool = new List<SpaceMaterials>();
    [SerializeField] Transform obstaclePoolParent;

    [SerializeField] EnemyAndCometSO enemyAndCometSO;
    [SerializeField]GameEventsSO gameEventsSO;

    private void Awake()
    {
        cometSpawnTime = 0;
        cam = Camera.main;
        SetSpawnLimit();
        InitializePool();
    }
    private void Start()
    {
        gameEventsSO.OnCometDestroyed += SendBacktoPoolComet;
        gameEventsSO.OnEnemyDestroyed += SendBacktoEnemy;
        gameEventsSO.OnObstaclesDestroyed += SendBacktoPoolObstacles;
        gameEventsSO.OnGameStart += HandleSpawnOnGameStart;
        gameEventsSO.OnGameOver += HandleSpawnOnGameEnd;
    }
    void HandleSpawnOnGameStart()
    {
        canSpawnComets = true;
    }
    void HandleSpawnOnGameEnd(bool isQuit)
    {
        canSpawnComets = false;
        cometSpawnTime = 0;
        cometSpeed = 2f;
    }

    private void OnDestroy()
    {
        gameEventsSO.OnCometDestroyed -= SendBacktoPoolComet;
        gameEventsSO.OnEnemyDestroyed -= SendBacktoEnemy;
        gameEventsSO.OnObstaclesDestroyed -= SendBacktoPoolObstacles;
        gameEventsSO.OnGameStart -= HandleSpawnOnGameStart;
        gameEventsSO.OnGameOver -= HandleSpawnOnGameEnd;
    }
    public void SendBacktoEnemy(Enemy enemy, int coinValue)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.position = enemyPoolParent.transform.position;
        EnemyPool.Add(enemy);
    }
    public void SendBacktoPoolObstacles(SpaceMaterials obstacles, int coinValue)
    {
        obstacles.gameObject.SetActive(false);
        obstacles.transform.position = obstaclePoolParent.transform.position;
        obstaclePool.Add(obstacles);
    }

    public void SendBacktoPoolComet(Comets comet, typeOfComet type, int coinValue)
    {
        comet.gameObject.SetActive(false);
        comet.transform.position = cometPoolParent.transform.position;
        switch (type)
        {
            case typeOfComet.small:
                smallCometsPool.Add(comet);
                break;

            case typeOfComet.medium:
                mediumCometsPool.Add(comet);
                break;

            case typeOfComet.large:
                largeCometsPool.Add(comet);
                break;

            default:
                break;
        }
    }
    void SetSpawnLimit()
    {
        float initalPoint = 0.1f;
        for (int i = 0; i < 4; i++)
        {
            spawnPoints.Add(cam.ViewportToWorldPoint(new Vector3(initalPoint, 0, cam.nearClipPlane)));
            initalPoint += 0.28f;
        }
        Vector3 spawnPoint = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        minY = spawnPoint.y;

    }
    private void InitializePool()
    {
        InitializePool(enemyAndCometSO.GetComet(typeOfComet.small), smallCometsPool, 40, cometPoolParent);
        InitializePool(enemyAndCometSO.GetComet(typeOfComet.medium), mediumCometsPool, 20, cometPoolParent);
        InitializePool(enemyAndCometSO.GetComet(typeOfComet.large), largeCometsPool, 20, cometPoolParent);

        List<Enemy> enemeyList = enemyAndCometSO.GetEnemyList();
        foreach (var obj in enemeyList)
        {
            InitializePool(obj, EnemyPool, 5, enemyPoolParent);
        }

        List<SpaceMaterials> spaceMaterial = enemyAndCometSO.GetSpaceMaterialList();
        foreach (var obj in spaceMaterial)
        {
            InitializePool(obj, obstaclePool, 5, obstaclePoolParent);
        }
    }
    void InitializePool<T>(T cometPrefab, List<T> Pool, int poolCount, Transform parent) where T : MonoBehaviour
    {
        for (int i = 0; i < poolCount; i++)
        {
            var data = Instantiate(cometPrefab, parent);

            var comet = cometPrefab.GetComponent<Comets>();
            var enemy = cometPrefab.GetComponent<Enemy>();
            var spaceMaterials = cometPrefab.GetComponent<SpaceMaterials>();
            if (comet != null)
            {
                data.GetComponent<Comets>().SetCometData(comet);
            }
            if (enemy != null)
            {
                data.GetComponent<Enemy>().SetEnemyData(enemy);
            }
            if (spaceMaterials != null)
            {
                data.GetComponent<SpaceMaterials>().SetSpaceMaterialData(spaceMaterials);
            }
            data.gameObject.SetActive(false);
            Pool.Add(data.GetComponent<T>());
        }
        cometPrefab.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (canSpawnComets)
        {
            cometSpeed += Time.deltaTime * 0.1f;
            if (Time.time > cometSpawnTime + 1 / cometSpawnRate)
            {
                List<int> spawnPointsList = new List<int>() { 0, 1, 2, 3 };
                cometSpawnTime = Time.time;

                int value = Random.Range(0, spawnPointsList.Count);
                if (cometSpeed < 5)
                {
                    LaunchPickedObjects(PickComet(cometSpeed), value);

                    value = Random.Range(0, spawnPointsList.Count);
                    LaunchPickedObjects(PickComet(cometSpeed), value);

                    value = Random.Range(0, spawnPointsList.Count);
                    LaunchPickedObjects(PickComet(cometSpeed), value);
                }
                else if (cometSpeed < 9)
                {
                    var randomSpawnner = Random.Range(0, 10);

                    if (randomSpawnner < 9)
                    {
                        LaunchPickedObjects(PickComet(cometSpeed), value);
                    }
                    else
                    {
                        LaunchPickedObjects(PickObstacles(cometSpeed), value);
                    }

                    randomSpawnner = Random.Range(0, 10);
                    value = Random.Range(0, spawnPointsList.Count);
                    if (randomSpawnner < 9)
                    {
                        LaunchPickedObjects(PickComet(cometSpeed), value);
                    }
                    else
                    {
                        LaunchPickedObjects(PickObstacles(cometSpeed), value);
                    }

                    randomSpawnner = Random.Range(0, 10);
                    value = Random.Range(0, spawnPointsList.Count);
                    if (randomSpawnner < 9)
                    {
                        LaunchPickedObjects(PickComet(cometSpeed), value);
                    }
                    else
                    {
                        LaunchPickedObjects(PickEnemy(cometSpeed), value);
                    }
                }
                else
                {
                    var randomSpawnner = Random.Range(0, 20);
                    if (randomSpawnner < 16)
                    {
                        LaunchPickedObjects(PickComet(cometSpeed), value);
                    }
                    else if (randomSpawnner < 18)
                    {
                        LaunchPickedObjects(PickObstacles(cometSpeed), value);
                    }
                    else
                    {
                        LaunchPickedObjects(PickEnemy(cometSpeed), value);
                    }

                    randomSpawnner = Random.Range(0, 20);
                    value = Random.Range(0, spawnPointsList.Count);
                    if (randomSpawnner < 16)
                    {
                        LaunchPickedObjects(PickComet(cometSpeed), value);
                    }
                    else if (randomSpawnner < 18)
                    {
                        LaunchPickedObjects(PickObstacles(cometSpeed), value);
                    }
                    else
                    {
                        LaunchPickedObjects(PickEnemy(cometSpeed), value);
                    }

                    randomSpawnner = Random.Range(0, 20);
                    value = Random.Range(0, spawnPointsList.Count);
                    if (randomSpawnner < 16)
                    {
                        LaunchPickedObjects(PickComet(cometSpeed), value);
                    }
                    else if (randomSpawnner < 18)
                    {
                        LaunchPickedObjects(PickObstacles(cometSpeed), value);
                    }
                    else
                    {
                        LaunchPickedObjects(PickEnemy(cometSpeed), value);
                    }
                }
            }
        }
        void LaunchPickedObjects<T>(T pickedComet, int spawnPointIndex) where T : MonoBehaviour
        {
            if (pickedComet == null) return;
            Vector2 spawnPoint = new Vector2(spawnPoints[spawnPointIndex].x, minY + Random.Range(4, 10));
            pickedComet.transform.position = spawnPoint;
            pickedComet.GetComponent<IDamageable>().SetSpeed(Mathf.Clamp((cometSpeed / 7), 2, 9f));
            pickedComet.gameObject.SetActive(true);
        }
        Comets PickComet(float cometSpeed)
        {
            Comets pickedComet;
            if (cometSpeed < 3f)
            {
                if (smallCometsPool.Count <= 0) return null;
                pickedComet = smallCometsPool[0];
                smallCometsPool.RemoveAt(0);
                return pickedComet;
            }
            else
            {
                var picked = Random.Range(1, 10);

                if (picked > 8)
                {
                    if (largeCometsPool.Count <= 0) return null;
                    pickedComet = largeCometsPool[0];
                    largeCometsPool.RemoveAt(0);
                    return pickedComet;
                }
                else if (picked > 6)
                {
                    if (mediumCometsPool.Count <= 0) return null;
                    pickedComet = mediumCometsPool[0];
                    mediumCometsPool.RemoveAt(0);
                    return pickedComet;
                }
                else if (picked > 2)
                {
                    if (smallCometsPool.Count <= 0) return null;
                    pickedComet = smallCometsPool[0];
                    smallCometsPool.RemoveAt(0);
                    return pickedComet;
                }
                return null;
            }
        }
        Enemy PickEnemy(float cometSpeed)
        {
            if (EnemyPool.Count <= 0) return null;
            Enemy pickedEnemy;
            int random = Random.Range(0, EnemyPool.Count - 1);
            pickedEnemy = EnemyPool[random];
            EnemyPool.RemoveAt(random);
            return pickedEnemy;
        }
        SpaceMaterials PickObstacles(float cometSpeed)
        {
            if (obstaclePool.Count <= 0) return null;
            SpaceMaterials obstacle;
            int random = Random.Range(0, obstaclePool.Count - 1);
            obstacle = obstaclePool[random];
            obstaclePool.RemoveAt(random);
            return obstacle;
        }
    }
}
