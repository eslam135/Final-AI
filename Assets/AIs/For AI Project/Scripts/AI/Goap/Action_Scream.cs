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
        cost = 1f;

        preconditions.Add("playerDetected", true);
        preconditions.Add("playerClose", true);
        preconditions.Add("screamReady", true);
        preconditions.Add("hasScreamMana", true);
    }

    public override void OnStart(EnemyAIContext ctx)
    {
        _timer = 0f;
        _appliedEffect = false;

        ctx.TrySpendMana(ctx.screamManaCost);
        ctx.lastScreamTime = Time.time;
        base.PlayWindup();

        ctx.animator?.SetTrigger("Scream");

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

        if (!_appliedEffect && _timer >= 0.3f)
        {
            _appliedEffect = true;
            OnAnimationHit(ctx);
        }

        return _timer >= 1.0f;
    }

    public override void OnAnimationHit(EnemyAIContext ctx)
    {
        base.OnAnimationHit(ctx);
        ApplyKnockback(ctx);
    }

    private void ApplyKnockback(EnemyAIContext ctx)
    {
        // Damage
        var hp = ctx.player.GetComponent<HealthView>().Health;
        if (hp != null)
            hp.TakeDamage(ctx.screamDamage);

        // Knockback via KnockbackReceiver — no Rigidbody needed
        var kb = ctx.player.GetComponent<KnockbackReceiver>();
        if (kb == null)
        {
            Debug.LogWarning($"[{ctx.name}] Player has no KnockbackReceiver — knockback skipped.");
            return;
        }

        Vector3 knockDir = (ctx.player.position - ctx.transform.position).normalized;
        knockDir.y = 0.3f;   // slight upward arc
        knockDir.Normalize();

        kb.ApplyKnockback(knockDir * ctx.screamKnockbackForce);
    }

    public override void OnEnd(EnemyAIContext ctx)
    {
        base.OnAnimationRecovery(ctx);
    }
}