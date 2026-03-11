using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using System;
public class Health : IHealth, IDamageable
{
    public float currentHealth { get; private set; }
    public float maxHealth { get; private set; }
    public bool IsAlive => currentHealth > 0.0f;
    private bool isDeadTriggered;
    public event Action OnDeath;
    public event Action<float> OnDamaged;
    public event Action<float> OnHealed;

    public Health(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
    }
    public void TakeHeal(float amount)
    {
        if (!IsAlive) return;
        this.currentHealth += amount;
        this.currentHealth = Mathf.Min(this.currentHealth, maxHealth);
        OnHealed?.Invoke(amount);
    }
    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        this.currentHealth -= amount;
        this.currentHealth = Mathf.Max(this.currentHealth, 0.0f);
        OnDamaged?.Invoke(amount);
        if(currentHealth == 0.0f && !isDeadTriggered)
        {
            isDeadTriggered = true;
            OnDeath?.Invoke();
        }
    }
}
