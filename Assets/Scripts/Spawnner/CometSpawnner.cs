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

    bool canSpawnComets;
    Camera cam;

    float minX, maxX, minY;

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
        Vector3 leftBound = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 rightBound = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        Vector3 spawnPoint = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));

        minY = spawnPoint.y;
        minX = leftBound.x;
        maxX = rightBound.x;
    }

    private void initialiZePool()
    {
        Debug.Log("pool initilalizing");
        InitializeCometPool(smallCometPrefab, smallCometsPool);
        InitializeCometPool(mediumCometPrefab, mediumCometsPool);
        InitializeCometPool(largeCometPrefab, largeCometsPool);
        Debug.Log("pool initialized");
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
            if (Time.time > cometSpawnTime + 1 / cometSpawnRate)
            {

                cometSpawnTime = Time.time;
                int randomCometValue = Random.Range(0, 10);
                Comets pickedComet;

                if (randomCometValue < 6)
                {
                    if (smallCometsPool.Count == 0) return;
                    pickedComet = smallCometsPool[0];
                    smallCometsPool.RemoveAt(0);
                }
                else if (randomCometValue < 9)
                {
                    if (mediumCometsPool.Count == 0) return;
                    pickedComet = mediumCometsPool[0];
                    mediumCometsPool.RemoveAt(0);
                }
                else
                {
                    if (largeCometsPool.Count == 0) return;
                    pickedComet = largeCometsPool[0];
                    largeCometsPool.RemoveAt(0);
                }
                Vector2 spawnPoint = new Vector2(Random.Range(minX, maxX), minY + 10f);
                pickedComet.transform.position = spawnPoint;
                pickedComet.gameObject.SetActive(true);
            }
        }
    }

    Comets GetSmallComet()
    {
        return smallCometsPool[0];
    }
    Comets GetMediumComet()
    {
        return mediumCometsPool[0];
    }
    Comets GetLargeComets()
    {
        return largeCometsPool[0];
    }
}
