// GoapAgent.cs
using System.Collections.Generic;
using UnityEngine;

public class GoapAgent : MonoBehaviour
{
    public float replanInterval = 0.25f;

    private EnemyAIContext _ctx;
    private WorldState _world;
    private EnemySensor _sensor;
    private GoapPlanner _planner;
    private List<GoapAction> _actions;
    private GoapAction _currentAction;
    private float _replanTimer;

    private void Awake()
    {
        _ctx = GetComponent<EnemyAIContext>();
        _world = new WorldState();
        _planner = new GoapPlanner();

        _sensor = gameObject.AddComponent<EnemySensor>();
        _sensor.Init(_ctx, _world);

        _actions = new List<GoapAction>(GetComponents<GoapAction>());
    }

    private void Start()
    {
        if (_ctx.player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(p.transform);
            if (p != null)
            {
                _ctx.player = p.transform;
                _ctx.playerRigidbody = p.GetComponent<Rigidbody>();
            }

        }
    }

    private bool _actionInProgress = false;

    private void Update()
    {
        _sensor.Tick();

        // Only replan if no action is running or we're in a non-blocking action
        if (!_actionInProgress)
        {
            _replanTimer -= Time.deltaTime;
            if (_replanTimer <= 0f || _currentAction == null)
            {
                _replanTimer = replanInterval;
                Replan();
            }
        }

        if (_currentAction != null)
        {
            bool finished = _currentAction.Execute(_ctx);
            if (finished)
            {
                Debug.Log($"[{name}] Finished → {_currentAction.actionName}");
                _currentAction.OnEnd(_ctx);
                _currentAction = null;
                _actionInProgress = false;
            }
        }
    }

    private void Replan()
    {
        GoapAction best = _planner.Plan(_actions, _world);

        if (best == _currentAction) return;

        // If current action is Idle, we can always interrupt it
        // If it's an attack, let it finish
        if (_currentAction != null && _currentAction.actionName != "Idle")
            return; // don't interrupt attacks

        if (_currentAction != null)
            _currentAction.OnEnd(_ctx);

        _currentAction = best;

        if (_currentAction != null)
        {
            _actionInProgress = _currentAction.actionName != "Idle";
            _currentAction.OnStart(_ctx);
            Debug.Log($"[{name}] Plan → {_currentAction.actionName} | Mana: {_ctx.mana:F1}");
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (_ctx == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _ctx.detectionRadius);

        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, _ctx.screamRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _ctx.healRadius);

        if (_ctx.player != null)
        {
            Vector3 eyePos = transform.position + Vector3.up * 1.5f;
            Vector3 playerChest = _ctx.player.position + Vector3.up * 1f;
            bool wall = Physics.Linecast(eyePos, playerChest, _ctx.wallLayer);
            Gizmos.color = wall ? Color.red : Color.cyan;
            Gizmos.DrawLine(eyePos, playerChest);
        }
    }
}