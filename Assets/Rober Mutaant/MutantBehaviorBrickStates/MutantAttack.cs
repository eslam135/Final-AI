using UnityEngine;

public class MutantAttack : MonoBehaviour
{
    private Transform player;
    private float meleeRange = 3.0f;
    private float meleeDamage = 2.0f;    
    public void DealMeleeDamage()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= meleeRange)
        {
            HealthView playerHealth = player.GetComponent<HealthView>();
            if (playerHealth != null)
            {
                playerHealth.Health.TakeDamage(meleeDamage);
            }
        }
    }
}
