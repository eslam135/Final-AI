using UnityEngine;

public class HealthView : MonoBehaviour
{
    float maxHealth = 100.0f;
    public Health Health;
    void Awake()
    {
        Health = new Health(maxHealth);
        Health.OnDeath += HandleDeath;
        Health.OnDamaged += HandleDamage;
        Health.OnHealed += HandleHeal;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Health.TakeDamage(20.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Health.TakeHeal(20.0f);
        }
    }
    private void HandleDamage(float amount)
    {
        Debug.Log($"Took damage: {amount} Current Health: {Health.currentHealth}");
    }
    private void HandleHeal(float amount)
    {
        Debug.Log($"Took Heal: {amount} Current Health {Health.currentHealth}");
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

}
