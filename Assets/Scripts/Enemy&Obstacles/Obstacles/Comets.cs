using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public enum typeOfComet
{
    small,
    medium,
    large
}
public class Comets : MonoBehaviour, IDamageable
{
    float cometsHealth, speed;
    public float TotalCometHealth;
    bool canMove;
    Animator animator;
    public typeOfComet cometType;
    public int coinValue;
    public int size;

    [SerializeField] GameEventsSO gameEventSO;
    private void Awake()
    {
        gameEventSO.OnGameOver += HandleGameOver;
    }
    private void OnDestroy()
    {
        gameEventSO.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver(bool isQuit)
    {
        if (this.gameObject.activeInHierarchy)
        {
            canMove = false;
            Destroy();
        }
    }
    public void SetCometData(string name, float totalHealth,typeOfComet cometType,int coinValue,int size)
    {
        this.gameObject.name = name;
        TotalCometHealth = totalHealth;
        this.cometType = cometType;
        this.coinValue = coinValue;
        this.size = size;
    }
    public void SetCometData(Comets comet)
    {
        this.gameObject.name = comet.gameObject.name;
        TotalCometHealth = comet.TotalCometHealth;
        this.cometType = comet.cometType;
        this.coinValue = comet.coinValue;
        size = comet.size;
        this.transform.localScale = Vector3.one * size;
    }

    private void OnEnable()
    {
        ResetCometHealth();
        canMove = true;
    }
    public void SetSpeed(float s)
    {
        speed = s;
    }
    void ResetCometHealth()
    {
        cometsHealth = TotalCometHealth;
    }
    private void OnDisable()
    {
        canMove = false;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            //transform.Rotate(0,0,rotationValue* Time.deltaTime,Space.Self);
        }
    }
    public void TakeDamage(float damageAmount)
    {
        if (!canMove) return;
        animator.SetTrigger("HitEffect");
        SoundManager.Instance.HitSound();
        cometsHealth -= damageAmount;
        if (cometsHealth <= 0)
        {
            canMove = false;
            gameEventSO.OnCometDestroyedAfterHit?.Invoke(this, cometType,coinValue);
            Destroy();
        }
    }
    public void Destroy()
    {
        gameEventSO.OnCometDestroyed?.Invoke(this, cometType, coinValue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var hitObj = other.GetComponent<PlayerData>();
        var hitObj1 = other.GetComponent<Comets>();
        if (hitObj != null)
        {
            hitObj.DeductPlayerLives();
        }
        if (gameObject.activeSelf)
        {
            if (hitObj1 != null)
            {
                if (cometType == typeOfComet.small)
                {
                    Destroy();
                }
                else if (cometType == typeOfComet.medium)
                {
                    if (hitObj1.cometType == typeOfComet.small)
                    {
                        hitObj1.Destroy();
                    }
                    else
                    {
                        Destroy();
                    }
                }
                else if (cometType == typeOfComet.large)
                {
                    if (hitObj1.cometType == typeOfComet.large)
                    {
                        hitObj1.Destroy();
                    }
                }
            }
        }
        if (other.tag == "Boundary")
        {
            canMove = false;
            Destroy();
        }
    }
}
