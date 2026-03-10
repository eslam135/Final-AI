using UnityEngine;



public class PlayerColliderDamage : MonoBehaviour
{


    HealthView healthView;
    void Start()
    {
        healthView = GetComponent<HealthView>();
    }

    private void Update()
    {
        Debug.Log(healthView.Health.currentHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered the capsule trigger.");

        if (other.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            healthView.Health.TakeDamage(15.0f);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            healthView.Health.TakeDamage(10.0f);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Melee"))
        {
            healthView.Health.TakeDamage(5.0f);
        }
    }
}
