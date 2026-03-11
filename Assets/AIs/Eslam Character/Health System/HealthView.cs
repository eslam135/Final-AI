using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    public void HandleDeath()
    {
        gameObject.GetComponent<Animator>()?.Play("Death");
        ScoreManager.Instance.AddScore();
        UIManager.Instance.UpdateScore();
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
