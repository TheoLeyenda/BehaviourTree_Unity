using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float HealthPoints = 30;

    private void Awake()
    {
        if (HealthPoints < 0)
            HealthPoints = 0;
    }

    public virtual bool OnTakeDamage(float Damage, Transform Instigator)
    {
        HealthPoints = HealthPoints - Damage;
        bool isDead = HealthPoints <= 0;
        if (isDead)
        {
            Die();
        }

        return isDead;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
