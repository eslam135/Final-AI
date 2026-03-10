// Action_Scream.cs
using UnityEngine;

public class Action_Scream : GoapAction
{
    private float _timer;
    private bool _appliedEffect;
    private GameObject _vfxInstance;

    private void Awake()
    {
        actionName = "Scream";
        cost = 1f; // highest priority when valid (player is close = danger)

        preconditions.Add("playerDetected", true);
        preconditions.Add("playerClose", true);
        preconditions.Add("screamReady", true);
        preconditions.Add("hasScreamMana", true);
    }

    public override void OnStart(EnemyAIContext ctx)
    {
        _timer = 0f;
        _appliedEffect = false;

        // Spend mana
        ctx.TrySpendMana(ctx.screamManaCost);
        ctx.lastScreamTime = Time.time;
        base.PlayWindup();
        // Animation
        ctx.animator?.SetTrigger("Scream");
        // Spawn scream VFX (expanding ring / shockwave)
        if (ctx.screamVFXPrefab != null)
        {
            Vector3 spawnPos = ctx.transform.position + Vector3.up * 1f;
            _vfxInstance = Instantiate(ctx.screamVFXPrefab, spawnPos, ctx.transform.rotation);
            _vfxInstance.transform.SetParent(ctx.transform);
            Destroy(_vfxInstance, 2f);
        }

        Debug.Log($"[{ctx.name}] SCREAM — knockback!");
    }

    public override bool Execute(EnemyAIContext ctx)
    {
        _timer += Time.deltaTime;

        // Apply knockback at the peak of the scream (0.3s in)
        if (!_appliedEffect && _timer >= 0.3f)
        {
            _appliedEffect = true;
            OnAnimationHit(ctx);
        }

        // Action duration
        return _timer >= 1.0f;
    }
    public override void OnAnimationHit(EnemyAIContext ctx)
    {
        base.OnAnimationHit(ctx); // plays hitClip
        ApplyKnockback(ctx);
    }
    private void ApplyKnockback(EnemyAIContext ctx)
    {
        if (ctx.playerRigidbody == null)
        {
            ctx.playerRigidbody = ctx.player.GetComponent<Rigidbody>();
        }

        if (ctx.playerRigidbody != null)
        {
            // Direction from enemy to player
            Vector3 knockDir = (ctx.player.position - ctx.transform.position).normalized;
            knockDir.y = 0.3f; // slight upward arc
            knockDir.Normalize();

            ctx.playerRigidbody.AddForce(knockDir * ctx.screamKnockbackForce, ForceMode.Impulse);
        }

        // Also apply damage
        var hp = ctx.player.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(ctx.screamDamage);

        // Screen shake hook (implement in your camera controller)
        // CameraShake.Instance?.Shake(0.5f, 0.3f);
    }
    

    public override void OnEnd(EnemyAIContext ctx)
    {
        base.OnAnimationRecovery(ctx);
        // VFX auto-destroys
    }
}