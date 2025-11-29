using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float enemyHealthTotal, fireRate;
    public int coinValue;

    float bulletSpeed, lastSpawnTime, speed;
    [SerializeField] Transform spawnPoint;

    float enemyHealth;
    Animator animator;
    bool canMove;

    [SerializeField] GameEventsSO gameEventsSO;
    private void Awake()
    {
        gameEventsSO.OnGameOver += HandleGameOver;
    }
    private void OnDestroy()
    {
        gameEventsSO.OnGameOver -= HandleGameOver;
    }
    void HandleGameOver(bool isQuit)
    {
        if (this.gameObject.activeInHierarchy)
        {
            Destroy();
            canMove = false;
        }
    }


    public void SetEnemyData(float totalHealth,float fireRate,int coinValue)
    {
        enemyHealthTotal = totalHealth;
        this.fireRate = fireRate;
        this.coinValue = coinValue;
    }
    public void SetEnemyData(Enemy enemy)
    {
        enemyHealthTotal = enemy.enemyHealthTotal;
        fireRate = enemy.fireRate;
        coinValue = enemy.coinValue;
        ResetEnemyHealth();
    }

    public void SetSpeed(float s)
    {
        bulletSpeed = s+5;
        speed = s;
    }

    void FireBullet()
    {
        var obj = EnemyBulletSpawnner.Instance.GetBullet("small");
        obj.transform.position = spawnPoint.position;
        obj.SetBulletSpeed(bulletSpeed);
        obj.gameObject.SetActive(true);
        SoundManager.Instance.EnemyFire();
    }
    private void OnEnable()
    {
        ResetEnemyHealth();
        lastSpawnTime = 2f;
        canMove = true;
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            if (Time.time - lastSpawnTime > 1/fireRate)
            {
                lastSpawnTime = Time.time;
                FireBullet();
            }
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }
    void ResetEnemyHealth()
    {
        enemyHealth = enemyHealthTotal;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float damageAmount)
    {
        animator.SetTrigger("HitEffect");
        SoundManager.Instance.HitSound();
        enemyHealth -= damageAmount;
        if (enemyHealth <= 0)
        {
            gameEventsSO.OnEnemyDestroyedAfterHit?.Invoke(this, coinValue);
            Destroy();
        }
    }

    public void Destroy()
    {
        gameEventsSO.OnEnemyDestroyed?.Invoke(this, coinValue);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hitObj = other.GetComponent<PlayerData>();
        var hitObj1 = other.GetComponent<IDamageable>();
        if (hitObj != null)
        {
            hitObj.DeductPlayerLives();
        }
        if (hitObj1 != null && other.GetComponent<Obstacles>() == null)
        {
            hitObj1.Destroy();
        }
        if (other.tag == "Boundary")
        {
            canMove = false;
            Destroy();
        }
    }
}
