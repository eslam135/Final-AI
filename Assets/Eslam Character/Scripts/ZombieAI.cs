using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private enum State
    {
        Chase,
        Melee,
        Flee,
        CastSpell
    }

    public Transform player;
    private NavMeshAgent agent;
    private Animator anim;
    private HealthView health;

    public float meleeRange = 2f;
    public float escapeRange = 12f;
    public float spellCooldown = 3f;

    public List<ParticleSystem> spellEffects;
    public Transform castPoint;

    private State currentState;

    private Vector3 fleeDestination;
    private bool fleeTargetSet;

    private float lastSpellTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        health = GetComponent<HealthView>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool lowHP = health != null && health.GetHealth() < 20;
        if (health != null && health.GetHealth() <= 0)
        {
            agent.enabled = false;
            return;
        }

        if (lowHP)
        {
            if (distance < escapeRange)
            {
                currentState = State.Flee;
            }
            else
            {
                currentState = State.CastSpell;
            }
        }
        else
        {
            if (distance <= meleeRange)
                currentState = State.Melee;
            else
                currentState = State.Chase;
        }

        switch (currentState)
        {
            case State.Chase:
                RotateToPlayer();
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool("running", true);
                break;

            case State.Melee:
                RotateToPlayer();
                agent.isStopped = true;
                anim.SetBool("running", false);
                anim.Play("Attack");
                break;

            case State.Flee:
                UpdateFlee();
                break;

            case State.CastSpell:
                UpdateCast();
                break;
        }
    }

    void RotateToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        if (dir == Vector3.zero) return;

        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, 5f * Time.deltaTime);
    }

    void RotateToDirection(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;

        if (dir == Vector3.zero) return;

        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, 6f * Time.deltaTime);
    }

    void UpdateFlee()
    {
        if (!fleeTargetSet)
        {
            Vector3 dir = (transform.position - player.position).normalized;
            Vector3 rawTarget = transform.position + dir * escapeRange;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rawTarget, out hit, 5f, NavMesh.AllAreas))
                fleeDestination = hit.position;
            else
                fleeDestination = rawTarget;

            agent.isStopped = false;
            agent.SetDestination(fleeDestination);
            anim.SetBool("running", true);

            fleeTargetSet = true;
        }

        RotateToDirection(fleeDestination);
    }

    void UpdateCast()
    {
        fleeTargetSet = false;

        RotateToPlayer();

        agent.isStopped = true;
        anim.SetBool("running", false);

        if (Time.time >= lastSpellTime + spellCooldown)
        {
            anim.Play("Shoot");
            lastSpellTime = Time.time;
        }
    }

    public void PlaySpellEffect()
    {
        float rand = Random.Range(0f, 1f);
        ParticleSystem spellEffect = null;

        if (rand >= 0.5f) spellEffect = spellEffects[0];
        else spellEffect = spellEffects[1];

        if (spellEffect != null)
        {
            if (rand >= 0.5f)
            {
                spellEffect.transform.position = castPoint.position;
                spellEffect.transform.rotation = castPoint.rotation;
                spellEffect.Play();
            }else
            {
                spellEffect.transform.position = player.position;
                spellEffect.transform.rotation = castPoint.rotation;
                spellEffect.Play();
            }
        }
    }

    public void EndCast() { }

    public void EndMelee() { }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, escapeRange);
    }
}