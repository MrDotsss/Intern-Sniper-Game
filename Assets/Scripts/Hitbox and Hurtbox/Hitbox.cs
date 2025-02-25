using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the hitbox
    public float knockbackForce = 300f; // Knockback force applied

    private void Start()
    {
        // Ensure the collider is a trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }
}
