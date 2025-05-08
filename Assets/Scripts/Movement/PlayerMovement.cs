using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public PlayerController controller;
    Camera mainCamera;
    Vector3 offset,initialPoint,initialPlayerPosition;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject spawnPosition,bulletPrefab;
    [SerializeField] Transform bulletPoolParent;

    bool CanSwipe,initalPointSet;
    [SerializeField] bool canFireBullets;

    [Range(50,100)]
    [SerializeField] float movementSpeed;
    public float bulletSpeed;
    [SerializeField] float fireRate,fireTime;


    private List<Bullets> bulletPool=new List<Bullets>();
    [SerializeField]private int poolSize = 50;


    private void Awake()
    {
        controller = new PlayerController();
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        controller.Enable();
    }
    private void OnDisable()
    {
        controller.Disable();
    }
    void Update()
    {
        MovementHandle();
        FireBullets();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        initialiZePool();
        fireTime = 0;
        controller.Movement.Touch.started += ctx => StartMovement(ctx);
        controller.Movement.Touch.canceled += ctx => EndMovement(ctx);
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

            float xWidth=spriteRenderer.bounds.extents.x;
            float yWidth =spriteRenderer.bounds.extents.y;

            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            Vector3 newPosition = initialPlayerPosition + targetPosition;

            // Clamp the new position within the screen bounds
            newPosition.x = Mathf.Clamp(newPosition.x, bottomLeft.x, topRight.x);
            newPosition.y = Mathf.Clamp(newPosition.y, bottomLeft.y, topRight.y-yWidth*1.5f);


            transform.position = Vector2.Lerp(transform.position,newPosition, movementSpeed * Time.deltaTime);
        }
    }
    // Update is called once per frame
    void FireBullets()
    {
        if (canFireBullets)
        {
            if(Time.time >= fireTime+1/fireRate)
            {
                fireTime =Time.time;
                var bullet=bulletPool[0].gameObject;
                bullet.transform.position = spawnPosition.transform.position;
                bullet.gameObject.SetActive(true);
                bulletPool.RemoveAt(0);
                bullet.GetComponent<Bullets>().canMoveForward = true;
            }
        }
    }

    private void initialiZePool()
    {
        for (int i = 0; i < poolSize; i++) 
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletPoolParent) ;
            bullet.SetActive(false);
            bullet.transform.localScale=Vector3.one;
            bulletPool.Add(bullet.GetComponent<Bullets>());
        }
    }

    public void GoBackToPoll(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Add(bullet.GetComponent<Bullets>());
    }

}
