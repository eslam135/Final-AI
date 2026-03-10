// DEBUG_WorldStateLogger.cs
// Add this to the enemy alongside everything else. Remove when done.
using UnityEngine;
using System.Collections.Generic;

public class DEBUG_WorldStateLogger : MonoBehaviour
{
    private WorldState _world;
    private EnemyAIContext _ctx;
    private GoapAgent _agent;
    private EnemySensor _sensor;

    private string _log = "";

    private void Start()
    {
        _ctx = GetComponent<EnemyAIContext>();
        _agent = GetComponent<GoapAgent>();

        // We need access to the world state. 
        // Easiest: make WorldState accessible from GoapAgent.
        // For now, we rebuild the checks manually.
    }

    private void Update()
    {
        if (_ctx == null || _ctx.player == null) return;

        float dist = Vector3.Distance(transform.position, _ctx.player.position);
        Vector3 eyePos = transform.position + Vector3.up * 1.5f;
        Vector3 playerChest = _ctx.player.position + Vector3.up * 1f;
        bool wall = Physics.Linecast(eyePos, playerChest, _ctx.wallLayer);

        _log = $"--- {name} DEBUG ---\n";
        _log += $"Distance to player: {dist:F1}\n";
        _log += $"Detection radius: {_ctx.detectionRadius}\n";
        _log += $"Scream range: {_ctx.screamRange}\n";
        _log += $"\n";
        _log += $"playerDetected: {dist <= _ctx.detectionRadius}\n";
        _log += $"playerClose: {dist <= _ctx.screamRange}\n";
        _log += $"wallBetweenPlayer: {wall}\n";
        _log += $"wallLayer mask value: {_ctx.wallLayer.value}\n";
        _log += $"\n";
        _log += $"Mana: {_ctx.mana:F1} / {_ctx.maxMana}\n";
        _log += $"hasScreamMana (>={_ctx.screamManaCost}): {_ctx.mana >= _ctx.screamManaCost}\n";
        _log += $"hasLaserMana (>={_ctx.laserManaCost}): {_ctx.mana >= _ctx.laserManaCost}\n";
        _log += $"hasPortalBeamMana (>={_ctx.portalBeamManaCost}): {_ctx.mana >= _ctx.portalBeamManaCost}\n";
        _log += $"\n";
        _log += $"screamReady: {Time.time >= _ctx.lastScreamTime + _ctx.screamCooldown}\n";
        _log += $"laserReady: {Time.time >= _ctx.lastLaserTime + _ctx.laserCooldown}\n";
        _log += $"portalBeamReady: {Time.time >= _ctx.lastPortalBeamTime + _ctx.portalBeamCooldown}\n";
        _log += $"healReady: {Time.time >= _ctx.lastHealTime + _ctx.healCooldown}\n";
        _log += $"\n";
        _log += $"Allies need heal: {_ctx.nearbyAllies.Count > 0} (count: {_ctx.nearbyAllies.Count})\n";
        _log += $"\n";

        // Check what player has
        Rigidbody prb = _ctx.player.GetComponent<Rigidbody>();
        _log += $"Player has Rigidbody: {prb != null}\n";
        if (prb != null)
        {
            _log += $"  isKinematic: {prb.isKinematic}\n";
            _log += $"  useGravity: {prb.useGravity}\n";
        }
        CharacterController cc = _ctx.player.GetComponent<CharacterController>();
        _log += $"Player has CharacterController: {cc != null}\n";
        _log += $"Player has PlayerHealth: {_ctx.player.GetComponent<PlayerHealth>() != null}\n";

        // Which actions exist on this object
        GoapAction[] actions = GetComponents<GoapAction>();
        _log += $"\nActions on this object ({actions.Length}):\n";
        foreach (var a in actions)
        {
            _log += $"  - {a.actionName} (cost {a.cost})\n";
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 500, 600), _log);
    }
}