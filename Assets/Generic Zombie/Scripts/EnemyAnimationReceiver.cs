using UnityEngine;

public class EnemyAnimationReceiver : MonoBehaviour
{
    private EnemyController _controller;
    private EnemyContext _ctx;

    private void Awake()
    {
        _controller = GetComponentInParent<EnemyController>();
        _ctx = GetComponentInParent<EnemyContext>();
    }

    // Animation Event — place at the frame of impact
    public void OnAnimationHit()
    {
        if (_ctx == null || _ctx.isDead) return;

        _ctx.PlayHit();

        if (!_ctx.PlayerInAttackRange()) return;

        var playerHealth = _ctx.player?.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(_ctx.attackDamage);
    }

    // Animation Event — place at the last frame of the attack clip
    public void OnAnimationRecovery()
    {
        if (_ctx == null || _ctx.isDead) return;

        _ctx.PlayRecovery();
        _controller?.OnAttackFinished();
    }
}