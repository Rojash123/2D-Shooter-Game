using UnityEngine;

public class Coin : MonoBehaviour
{
    bool canMove = false;
    [SerializeField] float speed;
    [SerializeField] GameEventsSO gameEventsSO;
    public void AllowMove(float delay)
    {
        Invoke(nameof(MoveAfterDelay),delay);
    }
    void MoveAfterDelay()
    {
        canMove = true;
    }
    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerMovement.Instance.transform.position, speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hitObj = other.GetComponent<PlayerData>();
        if (hitObj != null)
        {
            canMove = false;
            CoinSpawnner.Instance.SendBacktoPool(this);
            gameEventsSO.OnCoinCollected?.Invoke();
        }
    }
}
