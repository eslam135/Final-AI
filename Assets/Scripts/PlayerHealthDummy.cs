using UnityEngine;

public class PlayerHealthDummy : MonoBehaviour
{
    public int currentHealth = 100;

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took " + damageAmount + " damage! Health is now: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("PLAYER IS DEAD!");
        }
    }
}