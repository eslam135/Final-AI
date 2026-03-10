using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    private float damage;
    private Rigidbody rb;

    public void Init(Vector3 dir, float spd, float dmg)
    {
        damage = dmg;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = dir.normalized * spd; // Apply immediately, not in Start()
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}