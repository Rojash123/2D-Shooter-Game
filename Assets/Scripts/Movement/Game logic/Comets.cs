using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfComet
{
    small,
    medium,
    large
}
public class Comets : MonoBehaviour, IDamageable
{
    [SerializeField] float cometsHealth,speed,rotationValue;
    [SerializeField] float TotalCometHealth;
    bool canMove;
    Animator animator;

    private void OnEnable()
    {
        ResetCometHealth();
        canMove = true;
    }

    void ResetCometHealth()
    {
        cometsHealth = TotalCometHealth;
    }
    private void OnDisable()
    {
        canMove= false;
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
            //transform.Rotate(0f, 0f, rotationValue * Time.deltaTime);
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
            Destroy();
        }
    }

    public void Destroy()
    {
        PlayerMovement.Instance.OnCometDestroyed?.Invoke(this,cometType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boundary")
        {
            canMove = false;
            Destroy();
        }
    }
}
