// EnemySensor.cs
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    private EnemyAIContext _ctx;
    private WorldState _world;

    public void Init(EnemyAIContext ctx, WorldState world)
    {
        _ctx = ctx;
        _world = world;
    }

    public void Tick()
    {
        if (_ctx.player == null) return;

        float dist = Vector3.Distance(transform.position, _ctx.player.position);

        // ---- Player Detection ----
        bool playerDetected = dist <= _ctx.detectionRadius;
        _world.Set("playerDetected", playerDetected);

        // ---- Close Range (Scream) ----
        bool playerClose = dist <= _ctx.screamRange;
        _world.Set("playerClose", playerClose);

        // ---- Wall Check ----
        bool wallBetween = false;
        if (playerDetected)
        {
            Vector3 eyePos = transform.position + Vector3.up * 1.5f;
            Vector3 playerChest = _ctx.player.position + Vector3.up * 1.0f;
            wallBetween = Physics.Linecast(eyePos, playerChest);
        }
        _world.Set("wallBetweenPlayer", wallBetween);

        // ---- Ally Detection ----
        _ctx.nearbyAllies.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, _ctx.healRadius, _ctx.enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            var ally = hit.GetComponent<EnemyAIContext>();
            if (ally != null && ally.health < ally.maxHealth)
                _ctx.nearbyAllies.Add(ally);
        }
        _world.Set("alliesNeedHeal", _ctx.nearbyAllies.Count > 0);

        // ---- Cooldown Readiness ----
        _world.Set("screamReady", Time.time >= _ctx.lastScreamTime + _ctx.screamCooldown);
        _world.Set("laserReady", Time.time >= _ctx.lastLaserTime + _ctx.laserCooldown);
        _world.Set("portalBeamReady", Time.time >= _ctx.lastPortalBeamTime + _ctx.portalBeamCooldown);
        _world.Set("healReady", Time.time >= _ctx.lastHealTime + _ctx.healCooldown);

        // ---- Mana Checks ----
        _world.Set("hasScreamMana", _ctx.mana >= _ctx.screamManaCost);
        _world.Set("hasLaserMana", _ctx.mana >= _ctx.laserManaCost);
        _world.Set("hasPortalBeamMana", _ctx.mana >= _ctx.portalBeamManaCost);
    }
}