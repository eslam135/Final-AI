// Action_PortalBeam.cs
using UnityEngine;

public class Action_PortalBeam : GoapAction
{
    private float _timer;
    private bool _firedPortal;
    private Vector3 _targetPosition; // captured player position at cast start

    private void Awake()
    {
        actionName = "PortalBeam";
        cost = 4f;

        preconditions.Add("playerDetected", true);
        preconditions.Add("wallBetweenPlayer", true);   // wall IS between
        preconditions.Add("portalBeamReady", true);
        preconditions.Add("hasPortalBeamMana", true);
    }

    public override void OnStart(EnemyAIContext ctx)
    {
        _timer = 0f;
        _firedPortal = false;

        ctx.TrySpendMana(ctx.portalBeamManaCost);
        ctx.lastPortalBeamTime = Time.time;

        // Capture player position at cast time 
        // (the beam targets WHERE the player WAS — gives them time to move)
        _targetPosition = ctx.player.position;

        // explicit play windup
        base.PlayWindup();

        ctx.animator?.SetTrigger("PortalBeam");
        Debug.Log($"[{ctx.name}] PORTAL BEAM — casting at {_targetPosition}!");    }

    public override bool Execute(EnemyAIContext ctx)
    {
        _timer += Time.deltaTime;

        // Fallback: Spawn the portal sequence at 0.3s if animation event missed
        if (!_firedPortal && _timer >= 0.3f)
        {
            OnAnimationHit(ctx); // reuse the same logic to spawn portals and play hit VFX/sound
        }

        // Total action duration = windup + warning delay + beam duration + buffer
        float totalDuration = 0.3f + ctx.portalWarningDelay + ctx.portalBeamDuration + 0.5f;
        return _timer >= totalDuration;
    }

    // Handle animation event (hit) — plays hit clip via base and spawns portal sequence
    public override void OnAnimationHit(EnemyAIContext ctx)
    {
        base.OnAnimationHit(ctx); // plays hitClip
        if (_firedPortal) return;
        _firedPortal = true;
        SpawnPortalSequence(ctx);
    }

    public override void OnEnd(EnemyAIContext ctx)
    {
        base.OnAnimationRecovery(ctx);
    }

    private void SpawnPortalSequence(EnemyAIContext ctx)
    {
        // 1. Entry portal at the enemy (beam goes IN here)
        if (ctx.portalEntryVFXPrefab != null)
        {
            Transform firePoint = ctx.portalFirePoint != null
                ? ctx.portalFirePoint
                : ctx.transform;

            GameObject entryPortal = Instantiate(
                ctx.portalEntryVFXPrefab,
                firePoint.position,
                Quaternion.identity);

            Destroy(entryPortal, ctx.portalWarningDelay + ctx.portalBeamDuration + 1f);
        }

        // 2. Exit portal above the target position (beam comes OUT here)
        Vector3 exitPos = _targetPosition + Vector3.up * ctx.portalSpawnHeight;

        if (ctx.portalExitVFXPrefab != null)
        {
            GameObject exitPortal = Instantiate(
                ctx.portalExitVFXPrefab,
                exitPos,
                Quaternion.LookRotation(Vector3.down) // facing downward
            );

            Destroy(exitPortal, ctx.portalWarningDelay + ctx.portalBeamDuration + 1f);
        }

        // 3. Spawn the actual damaging beam AFTER the warning delay
        //    We use a dedicated controller MonoBehaviour for this
        GameObject beamController = new GameObject("PortalBeamController");
        PortalBeamController pbc = beamController.AddComponent<PortalBeamController>();
        pbc.Init(
            beamPrefab: ctx.portalBeamVFXPrefab,
            spawnPosition: exitPos,
            targetGround: _targetPosition,
            warningDelay: ctx.portalWarningDelay,
            beamDuration: ctx.portalBeamDuration,
            beamRadius: ctx.portalBeamRadius,
            damage: ctx.portalBeamDamage,
            wallLayer: ctx.wallLayer
        );
    }
}