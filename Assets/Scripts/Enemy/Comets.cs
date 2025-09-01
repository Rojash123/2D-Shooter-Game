using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float cometsHealth, speed, rotationValue;
    [SerializeField] float TotalCometHealth;
    bool canMove;
    Animator animator;
    public typeOfComet cometType;

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
        cometsHealth -= damageAmount;
        if (cometsHealth <= 0)
        {
            canMove = false;
            PlayerMovement.Instance.OnCometDestroyedAfterHit?.Invoke(this, cometType);
            Destroy();
        }
    }
    public void Destroy()
    {
        PlayerMovement.Instance.OnCometDestroyed?.Invoke(this, cometType);
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
