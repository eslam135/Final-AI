using UnityEngine;

public class PlayerColliderDamage : MonoBehaviour
{
    HealthView healthView;

    void Start()
    {
        healthView = GetComponent<HealthView>();
        healthView.Health.OnDeath += GameManager.Instance.RestartScene;
    }

    private void Update()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(healthView.Health.currentHealth);
        }
    }

    void OnTriggerEnter(Collider other)
    {
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