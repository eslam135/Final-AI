// Action_LaserBeam.cs
using UnityEngine;

public class Action_LaserBeam : GoapAction
{
    private float _timer;
    private bool _fired;

    private void Awake()
    {
        actionName = "LaserBeam";
        cost = 3f;

        preconditions.Add("playerDetected", true);
        preconditions.Add("wallBetweenPlayer", false);   // clear line of sight
        preconditions.Add("laserReady", true);
        preconditions.Add("hasLaserMana", true);
    }

    public override void OnStart(EnemyAIContext ctx)
    {
        _timer = 0f;
        _fired = false;

        ctx.TrySpendMana(ctx.laserManaCost);
        ctx.lastLaserTime = Time.time;

        // explicit play windup (keeps consistent style with other actions)
        base.PlayWindup();

        // Face the player
        FacePlayer(ctx);

        ctx.animator?.SetTrigger("LaserBeam");
        Debug.Log($"[{ctx.name}] LASER BEAM — direct fire!");
    }

    public override bool Execute(EnemyAIContext ctx)
    {
        _timer += Time.deltaTime;

        // Keep facing the player during windup
        if (_timer < 0.4f)
            FacePlayer(ctx);

        // Fallback: Fire the laser at 0.4s (match animation) if animation event missed
        if (!_fired && _timer >= 0.4f)
        {
            _fired = true;
            FireLaser(ctx);
        }

        return _timer >= 1.0f;
    }

    // Handle animation event (hit) — audio already played by base.OnAnimationHit
    public override void OnAnimationHit(EnemyAIContext ctx)
    {
        base.OnAnimationHit(ctx); // plays hitClip
        if (_fired) return;
        _fired = true;
        FireLaser(ctx);
    }

    public override void OnEnd(EnemyAIContext ctx)
    {
        base.OnAnimationRecovery(ctx);
    }

    private void FireLaser(EnemyAIContext ctx)
    {
        Transform firePoint = ctx.laserFirePoint != null ? ctx.laserFirePoint : ctx.transform;

        Vector3 origin = firePoint.position;
        Vector3 direction = (ctx.player.position - origin).normalized;

        GameObject proj = Instantiate(ctx.laserProjectilePrefab, origin, Quaternion.identity);

        LaserProjectile lp = proj.GetComponent<LaserProjectile>();
        if (lp != null)
        {
            lp.Init(direction, ctx.laserSpeed, ctx.laserDamage);
        }
      
        Debug.DrawRay(origin, direction * 10f, Color.red, 2f);
    }

    private void FacePlayer(EnemyAIContext ctx)
    {
        Vector3 dir = (ctx.player.position - ctx.transform.position);
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
            ctx.transform.rotation = Quaternion.Slerp(
                ctx.transform.rotation,
                Quaternion.LookRotation(dir.normalized),
                Time.deltaTime * 12f);
    }
}