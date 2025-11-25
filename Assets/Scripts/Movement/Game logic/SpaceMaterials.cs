using UnityEngine;

public class SpaceMaterials : MonoBehaviour,IDamageable
{
    public float enemyHealthTotal;
    public int coinValue;

    [SerializeField] Transform spawnPoint;
    float speed;
    float enemyHealth;
    Animator animator;
    bool canMove;

    [SerializeField] GameEventsSO gameEventsSO;
    public void SetSpaceMaterialData(float totalHealth, int coinValue)
    {
        enemyHealthTotal = totalHealth;
        this.coinValue = coinValue;
    }
    public void SetSpaceMaterialData(SpaceMaterials spaceMaterial)
    {
        enemyHealthTotal = spaceMaterial.enemyHealthTotal;
        coinValue = spaceMaterial.coinValue;
        ResetMaterialHealth();
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
    private void OnEnable()
    {
        ResetMaterialHealth();
        canMove = true;
    }
    private void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
    void ResetMaterialHealth()
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
        enemyHealth -= damageAmount;
        if (enemyHealth <= 0)
        {
            gameEventsSO.OnObstaclesDestroyedAfterHit?.Invoke(this, coinValue);
            Destroy();
        }
    }

    public void Destroy()
    {
        gameEventsSO.OnObstaclesDestroyed?.Invoke(this, coinValue);
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
