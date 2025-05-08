using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comets : MonoBehaviour, IDamageable
{
    [SerializeField] float cometsHealth;
    [SerializeField] GameObject cometsPool;

    private enum typeOfComet
    {
        small,
        medium,
        large
    }
    [SerializeField] typeOfComet cometType;

    public void Damage(float damageAmount)
    {
        cometsHealth -= damageAmount;
        if (cometsHealth <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {

    }
}
