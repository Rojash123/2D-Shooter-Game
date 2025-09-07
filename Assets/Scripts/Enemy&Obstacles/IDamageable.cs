using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void SetSpeed(float Speed);
    void TakeDamage(float damageAmount);
    void Destroy();
}
