using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private bool canMoveForward;
    string bulletType;
    float bulletSpeed;
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
            EnemyBulletSpawnner.Instance.OnBulletDestroyed(this, bulletType);
            canMoveForward = false;
        }
    }

    public void SetType(string type)
    {
        bulletType = type;
    }
    public void SetBulletSpeed(float speed)
    {
        bulletSpeed=speed;
        canMoveForward = true;
    }
    private void Update()
    {
        if (canMoveForward)
        {
            transform.position += new Vector3(0, -1, 0) * Time.deltaTime * bulletSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hitObj = other.GetComponent<PlayerData>();
        if (hitObj != null)
        {
            canMoveForward = false;
            hitObj.DeductPlayerLives();
            EnemyBulletSpawnner.Instance.OnBulletDestroyed(this, bulletType);
        }
        if (other.tag == "Boundary")
        {
            canMoveForward = false;
            EnemyBulletSpawnner.Instance.OnBulletDestroyed(this, bulletType);
        }
    }
}
