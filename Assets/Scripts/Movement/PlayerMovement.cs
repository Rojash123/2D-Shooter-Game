using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public PlayerController controller;
    PlayerData _playerData;
    Camera mainCamera;
    Vector3 offset, initialPoint, initialPlayerPosition;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject[] spawnPosition;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletPoolParent;

    bool CanSwipe, initalPointSet;
    [SerializeField] bool canFireBullets;

    [Range(50, 100)]
    [SerializeField] float movementSpeed;
    public float bulletSpeed;
    [SerializeField] float fireRate, fireTime;

    private List<Bullets> bulletPool = new List<Bullets>();
    [SerializeField] private int poolSize = 50;
    [SerializeField] public int bulletValue;
    [SerializeField] CometAndEnemySpawnner spawnner;

    public ParticleSystem centreExplosion, Sparks;

    #region Events

    public Action<bool> OnPlayerMove;
    public Action OnCoinCollected;

    public Action<Comets, typeOfComet, int> OnCometDestroyed, OnCometDestroyedAfterHit;
    public Action<Enemy, int> OnEnemyDestroyed, OnEnemyDestroyedAfterHit;
    public Action<SpaceMaterials, int> OnObstaclesDestroyed, OnObstaclesDestroyedAfterHit;

    public Action<PowerUp> OnPowerUpDestroyed;
    public Action<float, PowerUps, Sprite> OnPowerUpPicked;

    #endregion

    private void Awake()
    {
        controller = new PlayerController();
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        controller.Enable();
        OnPowerUpPicked += HandlePowerUpPicked;
    }
    private void OnDisable()
    {
        controller.Disable();
        OnPowerUpPicked += HandlePowerUpPicked;
    }
    void Update()
    {
        MovementHandle();
        FireBullets();
    }
    void Start()
    {
        _playerData = GetComponentInChildren<PlayerData>();
        Application.targetFrameRate = 60;
        initialiZePool();
        fireTime = 0;
        controller.Movement.Touch.started += ctx => StartMovement(ctx);
        controller.Movement.Touch.canceled += ctx => EndMovement(ctx);
        spawnner.StartSpawnning();
        OnPlayerMove?.Invoke(true);
    }

    void StartMovement(InputAction.CallbackContext context)
    {
        Vector2 screenPos = controller.Movement.Position.ReadValue<Vector2>();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.nearClipPlane));

        initialPlayerPosition = transform.position;
        offset = transform.position - new Vector3(worldPos.x, worldPos.y, transform.position.z);
        CanSwipe = true;
    }
    void EndMovement(InputAction.CallbackContext context)
    {
        initalPointSet = false;
        CanSwipe = false;
    }
    void MovementHandle()
    {
        if (CanSwipe)
        {
            Vector2 screenPosition = controller.Movement.LivePosition.ReadValue<Vector2>();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));

            if (!initalPointSet)
            {
                initialPoint = worldPosition;
                initalPointSet = true;
            }
            var targetPosition = (worldPosition - initialPoint);

            float xWidth = spriteRenderer.bounds.extents.x;
            float yWidth = spriteRenderer.bounds.extents.y;

            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            Vector3 newPosition = initialPlayerPosition + targetPosition;

            // Clamp the new position within the screen bounds
            newPosition.x = Mathf.Clamp(newPosition.x, bottomLeft.x, topRight.x);
            newPosition.y = Mathf.Clamp(newPosition.y, bottomLeft.y, topRight.y - yWidth * 1.5f);


            transform.position = Vector2.Lerp(transform.position, newPosition, movementSpeed * Time.deltaTime);
        }
    }
    void FireBullets()
    {
        if (canFireBullets)
        {
            if (Time.time >= fireTime + 1 / (fireRate + _playerData.increasedFireRate))
            {
                fireTime = Time.time;
                if (bulletValue == 1)
                {
                    if (_playerData.increasedFireRate <= 0)
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            if (i != 0)
                            {
                                var bullet = bulletPool[i].gameObject;
                                bullet.transform.position = spawnPosition[i].transform.position;
                                bullet.gameObject.SetActive(true);
                                bulletPool.RemoveAt(i);
                                bullet.GetComponent<Bullets>().canMoveForward = true;
                            }
                        }
                        return;
                    }
                }

                for (int i = 0; i <= bulletValue + _playerData.increasedMultiRate; i++)
                {
                    var bullet = bulletPool[i].gameObject;
                    bullet.transform.position = spawnPosition[i].transform.position;
                    bullet.gameObject.SetActive(true);
                    bulletPool.RemoveAt(i);
                    bullet.GetComponent<Bullets>().canMoveForward = true;
                }
            }
        }
    }

    private void initialiZePool()
    {
        InitializeBulletPool();
    }

    void InitializeBulletPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletPoolParent);
            bullet.SetActive(false);
            bullet.transform.localScale = Vector3.one;
            bulletPool.Add(bullet.GetComponent<Bullets>());
        }
    }

    public void GoBackToPoll(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Add(bullet.GetComponent<Bullets>());
    }

    public void PlayerKilled()
    {
        mainCamera.GetComponent<CameraShake>().Shake();
        centreExplosion.transform.position = this.transform.position;

        centreExplosion.Play();
        Sparks.Play();

        this.gameObject.SetActive(false);
        canFireBullets = false;
        CanSwipe = false;
    }

    public void HandlePowerUpPicked(float duration, PowerUps type, Sprite powerUpIcon)
    {
        switch (type)
        {
            case PowerUps.increasedFireRate:
                float increasedfireRate = fireRate * 2;
                Mathf.Clamp(increasedfireRate, 2, 10);
                _playerData.HandleFireRatePowerUps(duration, powerUpIcon, increasedfireRate - fireRate);
                break;

            case PowerUps.invincibility:
                _playerData.HandleInvincibiltyPowerUps(duration, powerUpIcon);
                break;

            case PowerUps.enhancedAttack:
                _playerData.HandleMultiShotPowerUps(duration, powerUpIcon, 2);
                break;
        }
    }
}
