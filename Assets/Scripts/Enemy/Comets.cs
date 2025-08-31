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


    [SerializeField] typeOfComet cometType;
    public void Damage(float damageAmount)
    {
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
        if (hitObj != null) 
        {
            hitObj.DeductPlayerLives();
        }
        if (other.tag == "Boundary")
        {
            canMove = false;
            Destroy();
        }
    }
}
