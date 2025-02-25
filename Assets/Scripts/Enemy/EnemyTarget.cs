using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount)
    {
        Destroy(gameObject);
    }
}
