// PlayerHealth.cs
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0f);
        Debug.Log($"Player took {amount:F1} damage → HP: {health:F0}/{maxHealth}");

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Your death logic here
    }
}