using UnityEngine;

public class HealthView : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    [SerializeField] private float maxHealth = 100.0f;
    public Health Health;
    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        Health = new Health(maxHealth);
        Health.OnDeath += HandleDeath;
        // Health.OnDamaged += HandleDamage;
        // Health.OnHealed += HandleHeal;
    }
    private void Update()
    {
        Debug.Log(Health.currentHealth);
    }
    private void HandleDeath()
    {
        gameObject.GetComponent<Animator>().Play("Death");
    }
    public void setFalse()
    {
        gameObject.SetActive(false);
    }
    public float GetHealth()
    {
        return Health.currentHealth;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered the capsule trigger.");

        if (other.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            Health.TakeDamage(15.0f);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Health.TakeDamage(10.0f);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Melee"))
        {
            Health.TakeDamage(5.0f);
        }
    }
}
