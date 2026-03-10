using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Banshee/Melee Attack")]
public class Action_Melee : GOAction
{
    [InParam("target")] public Transform target;

    [InParam("attackDuration")] public float attackDuration = 2.0f;
    [InParam("hitTime")] public float hitTime = 0.5f;
    [InParam("meleeDamage")] public int meleeDamage = 10;
    [InParam("cooldownTime")] public float cooldownTime = 2.5f;

    [InParam("damageRadius")] public float damageRadius = 2.5f;

    private NavMeshAgent agent;
    private Animator anim;
    private BansheeMemory memory;
    private float timer = 0f;
    private bool hasHit = false;
    private bool isValidAttack = false;

    public override void OnStart()
    {
        memory = gameObject.GetComponent<BansheeMemory>();

        if (memory != null && Time.time >= memory.nextMeleeTime)
        {
            isValidAttack = true;
            memory.nextMeleeTime = Time.time + cooldownTime;

            agent = gameObject.GetComponent<NavMeshAgent>();
            anim = gameObject.GetComponentInChildren<Animator>();
            timer = 0f;
            hasHit = false;

            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
            if (anim != null)
            {
                anim.SetBool("isMelee", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isThrowing", false);
                anim.SetBool("isScreaming", false);
            }
        }
        else
        {
            isValidAttack = false;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (gameObject.GetComponent<HealthView>().Health.currentHealth <= 0) return TaskStatus.FAILED;

        if (!isValidAttack) return TaskStatus.FAILED;

        if (target != null)
        {
            Vector3 direction = (target.position - gameObject.transform.position).normalized;
            direction.y = 0;
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }

        timer += Time.deltaTime;

        if (timer >= hitTime && !hasHit)
        {
            hasHit = true;
            if (target != null && Vector3.Distance(gameObject.transform.position, target.position) <= damageRadius)
            {
                var playerHealth = target?.GetComponent<HealthView>().Health;
                if (playerHealth != null) playerHealth.TakeDamage(meleeDamage);
            }
        }

        if (timer >= attackDuration)
        {
            if (anim != null) anim.SetBool("isMelee", false);
            return TaskStatus.COMPLETED;
        }
        return TaskStatus.RUNNING;
    }

    public override void OnAbort()
    {
        if (anim != null) anim.SetBool("isMelee", false);
    }
}