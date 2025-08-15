using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullets:MonoBehaviour
{
    public bool canMoveForward;
    public float damageAmount;
    private void Update()
    {
        if (canMoveForward)
        {
            transform.position += new Vector3(0,1,0)* Time.deltaTime * PlayerMovement.Instance.bulletSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.Damage(damageAmount);
            PlayerMovement.Instance.GoBackToPoll(this.gameObject);
            canMoveForward = false;
        }
    }
}
