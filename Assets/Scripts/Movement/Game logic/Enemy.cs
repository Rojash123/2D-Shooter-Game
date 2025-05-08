using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    [SerializeField] float enemyHealth;
    [SerializeField] GameObject enemyPool;

    public void Damage(float damageAmount)
    {
        enemyHealth-= damageAmount;
        if(enemyHealth <= 0)
        {
            Destroy();
        }
        
    }

    public void Destroy()
    {

    }
}
