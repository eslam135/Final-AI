using UnityEngine;

public class EnemyContext : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Transform player; // auto-found by tag in EnemyController
    public Animator animator;
    public AudioSource audioSource;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 8f;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float health = 100f;
    public int attackDamage = 15;

    [Header("Attack")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    [HideInInspector] public float lastAttackTime = -99f;

    [Header("Audio")]
    public AudioClip windupClip;
    public AudioClip hitClip;
    public AudioClip recoveryClip;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDead;

    public bool PlayerInAttackRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public bool AttackOffCooldown()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0f, health - amount);
    }

    public void PlayWindup() => PlayClip(windupClip);
    public void PlayHit() => PlayClip(hitClip);
    public void PlayRecovery() => PlayClip(recoveryClip);

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}