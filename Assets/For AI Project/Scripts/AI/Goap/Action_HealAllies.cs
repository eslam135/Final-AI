// Action_HealAllies.cs
using UnityEngine;

public class Action_HealAllies : GoapAction
{
    private float _timer;
    private bool _healed;

    private void Awake()
    {
        actionName = "HealAllies";
        cost = 2f;

        preconditions.Add("alliesNeedHeal", true);
        preconditions.Add("healReady", true);
    }

    public override void OnStart(EnemyAIContext ctx)
    {
        _timer = 0f;
        _healed = false;
        ctx.lastHealTime = Time.time;

        // explicit play windup
        base.PlayWindup();

        ctx.animator?.SetTrigger("Heal");
        Debug.Log($"[{ctx.name}] Healing allies!");
    }

    public override bool Execute(EnemyAIContext ctx)
    {
        _timer += Time.deltaTime;

        // Fallback: heal at 0.6s if animation event missed
        if (!_healed && _timer >= 0.6f)
        {
            _healed = true;
            HealAllies(ctx);
        }

        return _timer >= 1.2f;
    }

    // Animation event: perform heal on hit
    public override void OnAnimationHit(EnemyAIContext ctx)
    {
        base.OnAnimationHit(ctx); // plays hitClip
        if (_healed) return;
        _healed = true;
        HealAllies(ctx);
    }

    public override void OnEnd(EnemyAIContext ctx)
    {
        base.OnAnimationRecovery(ctx);
    }

    private void HealAllies(EnemyAIContext ctx)
    {
        foreach (var ally in ctx.nearbyAllies)
        {
            ally.health = Mathf.Min(ally.health + ctx.healAmount, ally.maxHealth);
            Debug.Log($"  Healed {ally.name} → {ally.health}/{ally.maxHealth}");
        }
    }
}   