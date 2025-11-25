using AllIn1SpriteShader;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    float duration,speed;
    bool canMove;

    public PowerUps powerUpType;

    [SerializeField] GameEventsSO gameEventSO;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            gameEventSO.OnPowerUpPicked?.Invoke(duration,powerUpType,spriteRenderer.sprite);
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
        gameEventSO.OnPowerUpDestroyed?.Invoke(this);
    }
}
