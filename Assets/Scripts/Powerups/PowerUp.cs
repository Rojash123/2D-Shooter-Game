using AllIn1SpriteShader;
using System.Linq;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    float duration,speed;
    bool canMove;

    public PowerUps powerUpType;
    [SerializeField] GameEventsSO gameEventsSO;
    private void Awake()
    {
        gameEventsSO.OnGameOver += HandleGameOver;
    }
    void HandleGameOver(bool isQuit)
    {
        if (this.gameObject.activeInHierarchy)
        {
            Destroy();
            canMove = false;
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameEventsSO.OnDataLoadedAndUpdated += HandleDataChanges;
    }
    private void OnDestroy()
    {
        gameEventsSO.OnDataLoadedAndUpdated -= HandleDataChanges;
        gameEventsSO.OnGameOver -= HandleGameOver;
    }

    void HandleDataChanges(SaveData data)
    {
        var powerup = data.powerupdata.FirstOrDefault(x => x.powerUpType == powerUpType);
        duration=powerup.duration;
    }

    public void SetData(PowerUpsClass data, float movingSpeed)
    {
        spriteRenderer.sprite = data.powerupIcon;
        duration = data.duration;
        powerUpType = data.powerUpType;
        canMove = true;
        speed=movingSpeed;
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hitObj = other.GetComponent<PlayerData>();
        if (hitObj != null)
        {
            gameEventsSO.OnPowerUpPicked?.Invoke(duration,powerUpType,spriteRenderer.sprite);
            canMove = false;
            Destroy();
        }
        if (other.tag == "Boundary")
        {
            canMove = false;
            Destroy();
        }
    }

    public void Destroy()
    {
        gameEventsSO.OnPowerUpDestroyed?.Invoke(this);
    }
}
