using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public GameObject owner; // The object this HurtBox belongs to


    private void OnTriggerEnter(Collider collision)
    {
        // Check if the other collider is a HitBox
        Hitbox hitbox = collision.GetComponent<Hitbox>();
        if (hitbox == null) return;

        // Apply damage if the owner has a "TakeDamage" method
        if (owner.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(hitbox.damage);
            Debug.Log("ouch");
        }

        // Apply knockback if the owner has a "Knockback" method
        if (owner.TryGetComponent(out IKnockbackable knockbackable))
        {
            Vector3 hitPosition = collision.transform.position;
            knockbackable.ApplyKnockback(hitPosition, hitbox.knockbackForce);
        }
    }
}
