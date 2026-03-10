using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyContext))]
public class EnemyController : MonoBehaviour
{
    private enum State { Seek, Attack }
    private State _state = State.Seek;

    private EnemyContext _ctx;
    private NavMeshAgent _agent;

    private static readonly int HashAttack = Animator.StringToHash("Attack");
    private static readonly int HashDead = Animator.StringToHash("Dead");
    private static readonly int HashIsRunning = Animator.StringToHash("IsRunning");
    HealthView health;
    private void Awake()
    {
        _ctx = GetComponent<EnemyContext>();
        _agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthView>();
        _agent.speed = _ctx.moveSpeed;
        _agent.stoppingDistance = _ctx.attackRange * 0.85f;
    }

    private void Start()
    {
        // Always find player by tag — never assign manually
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            _ctx.player = p.transform;
        else
            Debug.LogWarning($"[{name}] No GameObject with tag 'Player' found!");
    }

    private void Update()
    {
        if (_ctx.isDead) return;
        if (health != null && health.GetHealth() <= 0)
        {
            _agent.enabled = false;
            return;
        }
        switch (_state)
        {
            case State.Seek: UpdateSeek(); break;
            case State.Attack: UpdateAttack(); break;
        }

        UpdateAnimator();
    }

    // ── SEEK — always chasing ─────────────────────────────────────────────────
    private void UpdateSeek()
    {
        if (_ctx.player == null) return;
        if (!_agent.isOnNavMesh) return;
        _agent.SetDestination(_ctx.player.position);
        FaceTarget(_ctx.player.position);

        if (_ctx.PlayerInAttackRange() && _ctx.AttackOffCooldown())
            EnterAttack();
    }

    // ── ATTACK ────────────────────────────────────────────────────────────────
    private void EnterAttack()
    {
        _state = State.Attack;
        _ctx.isAttacking = true;
        _ctx.lastAttackTime = Time.time;

        _agent.ResetPath();
        FaceTarget(_ctx.player.position);

        _ctx.PlayWindup();
        _ctx.animator?.SetTrigger(HashAttack);
    }

    private void UpdateAttack()
    {
        // Waits for OnAnimationRecovery() → OnAttackFinished()
        FaceTarget(_ctx.player.position);
    }

    public void OnAttackFinished()
    {
        _ctx.isAttacking = false;
        _state = State.Seek;
    }

    // ── DEATH ─────────────────────────────────────────────────────────────────
    public void Die()
    {
        if (_ctx.isDead) return;
        _ctx.isDead = true;

        //_ctx.animator?.SetTrigger(HashDead);
    }

    // ── HELPERS ───────────────────────────────────────────────────────────────
    private void FaceTarget(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0f;
        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            _ctx.rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateAnimator()
    {
        if (_ctx.animator == null) return;
        _ctx.animator.SetBool(HashIsRunning, _agent.velocity.magnitude > 0.1f);
    }

  
}