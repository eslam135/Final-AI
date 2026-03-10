using UnityEngine;

public class BansheeHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    public bool isDead = false;

    [ContextMenu("Debug: Kill Banshee")]
    public void DebugKill()
    {
        TakeDamage(100f);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Banshee took damage! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }
}