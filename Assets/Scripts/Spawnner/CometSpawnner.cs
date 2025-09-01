using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawnner : MonoBehaviour
{
    [SerializeField] GameObject smallCometPrefab, mediumCometPrefab, largeCometPrefab;

    private List<Comets> smallCometsPool = new List<Comets>();
    private List<Comets> largeCometsPool = new List<Comets>();
    private List<Comets> mediumCometsPool = new List<Comets>();

    [SerializeField] Transform cometPoolParent;
    [SerializeField] float cometSpawnRate, cometSpawnTime;

    List<Vector3> spawnPoints = new List<Vector3>();

    float lasttimeMediumCometSpawnTime = 0;
    float lasttimeLargeCometSpawnTime = 0;

    bool canSpawnComets;
    Camera cam;

    float minY;
    float cometSpeed = 2f;

    private void Awake()
    {
        cometSpawnTime = 0;
        cam = Camera.main;
        SetSpawnLimit();
        initialiZePool();
    }
    private void Start()
    {
        PlayerMovement.Instance.OnCometDestroyed += SendBacktoPool;
    }

    public void SendBacktoPool(Comets comet, typeOfComet type)
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
    private void initialiZePool()
    {
        InitializeCometPool(smallCometPrefab, smallCometsPool);
        InitializeCometPool(mediumCometPrefab, mediumCometsPool);
        InitializeCometPool(largeCometPrefab, largeCometsPool);
    }
    void InitializeCometPool(GameObject cometPrefab, List<Comets> Pool)
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject comet = GameObject.Instantiate(cometPrefab, cometPoolParent);
            comet.SetActive(false);
            Pool.Add(comet.GetComponent<Comets>());
        }
    }

    public void StartSpawnning()
    {
        canSpawnComets = true;
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
                LaunchPickedComets(PickComet(cometSpeed), value);

                value = Random.Range(0, spawnPointsList.Count);
                LaunchPickedComets(PickComet(cometSpeed), value);

                value = Random.Range(0, spawnPointsList.Count);
                LaunchPickedComets(PickComet(cometSpeed), value);
            }
        }
    }

    void LaunchPickedComets(Comets pickedComet, int spawnPointIndex)
    {
        if (pickedComet == null) return;
        Vector2 spawnPoint = new Vector2(spawnPoints[spawnPointIndex].x, minY + Random.Range(4, 10));
        pickedComet.transform.position = spawnPoint;
        pickedComet.SetSpeed(Mathf.Clamp((cometSpeed / 3), 2, 9f));
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
}
